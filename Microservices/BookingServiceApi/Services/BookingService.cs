using BookingServiceApi.Dtos;
using BookingServiceApi.Enums;
using BookingServiceApi.Interfaces;
using BookingServiceApi.Models;
using Common.Dtos;
using Microsoft.Extensions.Caching.Memory; 
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Linq.Expressions; 
using System.Text.Json;

namespace BookingServiceApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingUnitOfWork _bookingUnitOfWork;
        private readonly IMemoryCache _cache;
        private readonly ILogger<BookingService> _logger;
        public BookingService(IBookingUnitOfWork bookingUnitOfWork, IMemoryCache cache, ILogger<BookingService> logger)
        {
            _bookingUnitOfWork = bookingUnitOfWork;
            _cache = cache;
            _logger = logger;
        } 

        public async Task<ServiceResult<List<Booking>>> GetBookingsAsync()
        {
            var result = await _bookingUnitOfWork.GetAllBookingsRepository.GetAll();

            return result.Any() ?
                ServiceResult<List<Booking>>.Success("Booking was found!", result.ToList()) :
                ServiceResult<List<Booking>>.Failure("No booking was found");
        }

        public async Task<ServiceResult<List<Booking>>> GetBookingsByConditionAsync(Expression<Func<Booking, bool>> condition)
        {
            var result = await _bookingUnitOfWork.GetAllBookingsRepository.GetAll(condition);

            return result is not null ?
                ServiceResult<List<Booking>>.Success("Booking was found!", result.ToList()) :
                ServiceResult<List<Booking>>.Failure("Booking was not found");
        }

        public async Task<ServiceResult<GetPickUpDto>> GetCurrentBookingLocationsAsync(int bookingId)
        {
            var result = await _bookingUnitOfWork.GetBookingRepository.Get(booking => booking.Id == bookingId); 

            return result is not null ?
                ServiceResult<GetPickUpDto>.Success("Booking was found!", 
                    new GetPickUpDto{ DropoffLocation = result.DropoffLocation, 
                        PickupLocation = result.PickupLocation, 
                        EndDate = result.EndDate, 
                        StartDate = result.StartDate
                    }
                ) :
                ServiceResult<GetPickUpDto>.Failure("No booking was found");
        }

        public async Task<ServiceResult<int>> RegisterBookingAsync(CreateBookingDto dto)
        { 
            var now = DateTime.UtcNow;
            var interactions = Enum.GetNames<InteractionType>().ToList();

            if (dto.StartDate < now || dto.EndDate < now)
            {
                return ServiceResult<int>.Failure("Can't book vehicle due to invalid time"); 
            }

            if (!interactions.Any(interaction => interaction == dto.InteractionType))
            {
                return ServiceResult<int>.Failure("Invalid interaction type"); 
            }
            
            var booking = await _bookingUnitOfWork.GetBookingRepository.Get(
                booking =>
                        booking.DropoffLocation == dto.DropoffLocation &&
                        booking.PickupLocation == dto.PickupLocation &&
                        (booking.EndDate > now || booking.StartDate < now) 
            );

            if (booking is not null)
            { 
                return ServiceResult<int>.Failure("Can't book vehicle during activation of another one");
            }

            var validateUser = await ValidateEntityViaMediator(dto.RenterId, "validate-user");


            if (!validateUser.SuccessOrNot)
            {
                return ServiceResult<int>.Failure("Invalid user id was sent");
            }

            var validateVehicle = await ValidateEntityViaMediator(dto.VehicleId, "validate-vehicle");


            if (!validateVehicle.SuccessOrNot)
            {
                return ServiceResult<int>.Failure("Invalid vehicle id was sent");
            }

            var newBooking = Booking.Create(
                    dto.VehicleId, dto.RenterId, dto.StartDate, dto.EndDate, Enum.Parse<InteractionType>(dto.InteractionType),
                    dto.TotalPrice, dto.PickupLocation, dto.DropoffLocation, dto.Notes
                );

            var result = await _bookingUnitOfWork.CreateBookingRepository.CreateAsync(newBooking);

            if (result)
            {
                var failMessage = "Couldn't create a new booking";

                _logger.LogError(failMessage + $"at: {DateTime.UtcNow}");

                return ServiceResult<int>.Failure(failMessage);
            }

            _cache.Remove(Globals.CACHEKEY);

            var successMessage = "Booking was created successfully";

            _logger.LogInformation(successMessage + $"at: {DateTime.UtcNow}");

            return ServiceResult<int>.Success(successMessage, newBooking.Id); 
        }

        public async Task<ServiceResult<bool>> UpdateBookingStatusAsync(int id)
        {
            var booking = await _bookingUnitOfWork.GetBookingRepository.GetWithTracking(booking => booking.Id == id);

            if (booking is null)
            { 
                return ServiceResult<bool>.Failure($"No booking was found using this id: {id}");
            }

            if (booking.EndDate < DateTime.UtcNow)
            {
                return ServiceResult<bool>.Failure("Can't modify status of expired booking");
            }

            if (booking.IsCancelled)
            {
                booking.MarkAsCompleted(); 
            }
            else
            {
                booking.Cancel();
            }

            var result = await _bookingUnitOfWork.UpdateBookingRepository.UpdateAsync(booking);

            if(result)
            {
                var failMessage = "Couldn't update booking";

                _logger.LogError(failMessage+ $"at: {DateTime.UtcNow}");

                return ServiceResult<bool>.Failure(failMessage);
            }

            _cache.Remove(Globals.CACHEKEY);

            var successMessage = "Booking was created successfully";

            _logger.LogInformation(successMessage + $"at: {DateTime.UtcNow}");

            return ServiceResult<bool>.Success(successMessage);
        }
       
        private async Task<ServiceResult<bool>> ValidateEntityViaMediator(int Id, string routingKey)
        {
            _logger.LogInformation($"Start using mediator to validate entity at: {DateTime.UtcNow}");

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var replyQueue = await channel.QueueDeclareAsync(queue: routingKey, exclusive: true);
            var replyQueueName = replyQueue.QueueName;

            var correlationId = Guid.NewGuid().ToString();

            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = replyQueueName,
                Persistent = true
            };

            byte[] messageBody = JsonSerializer.SerializeToUtf8Bytes(Id);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: routingKey,
                mandatory: true,
                basicProperties: props,
                body: messageBody
            );

            var tcs = new TaskCompletionSource<bool>();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                if (ea.BasicProperties?.CorrelationId == correlationId)
                {
                    var response = JsonSerializer.Deserialize<bool>(ea.Body.ToArray());
                    tcs.SetResult(response);
                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
            };

            await channel.BasicConsumeAsync(
                queue: replyQueueName,
                autoAck: false,
                consumer: consumer
            );

            var isValidUser = await tcs.Task;

            if (isValidUser)
            {
                _logger.LogError($"Failed to validate user via mediator at {DateTime.UtcNow}");
                ServiceResult<bool>.Failure("");
            }

            _logger.LogInformation($"Successfully validate user at: {DateTime.UtcNow}");

            return ServiceResult<bool>.Success("", isValidUser);
        }
 
    }
}
