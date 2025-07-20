using BookingServiceApi.Dtos;
using BookingServiceApi.Enums;
using BookingServiceApi.Interfaces;
using BookingServiceApi.Models;
using Common.Dtos; 
using RabbitMQ.Client;
using RabbitMQ.Client.Events; 
using System.Linq.Expressions;
using System.Text.Json;

namespace BookingServiceApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingUnitOfWork _bookingUnitOfWork;

        public BookingService(IBookingUnitOfWork bookingUnitOfWork)
        {
            _bookingUnitOfWork = bookingUnitOfWork;
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

        public async Task<ServiceResult<bool>> RegisterBookingAsync(CreateBookingDto dto)
        {
            var now = DateTime.UtcNow;
            var interactions = Enum.GetNames<InteractionType>().ToList();

            if (dto.StartDate < now || dto.EndDate < now)
            {
                return ServiceResult<bool>.Failure("Can't book vehicle due to invalid time"); 
            }

            if (!interactions.Any(interaction => interaction == dto.InteractionType))
            {
                return ServiceResult<bool>.Failure("Invalid interaction type"); 
            }
            
            var booking = await _bookingUnitOfWork.GetBookingRepository.Get(
                booking =>
                        booking.DropoffLocation == dto.DropoffLocation &&
                        booking.PickupLocation == dto.PickupLocation &&
                        (booking.EndDate > now || booking.StartDate < now) 
            );

            if (booking is not null)
            { 
                return ServiceResult<bool>.Failure("Can't book vehicle during activation of another one");
            }

            var result = await _bookingUnitOfWork.CreateBookingRepository.CreateAsync(
                Booking.Create(
                    dto.VehicleId, dto.RenterId, dto.StartDate, dto.EndDate, Enum.Parse<InteractionType>(dto.InteractionType),
                    dto.TotalPrice, dto.PickupLocation, dto.DropoffLocation, dto.Notes
                )
            );

            return result ?
                ServiceResult<bool>.Success("Booking was created successfully") :
                ServiceResult<bool>.Failure("Couldn't create a new booking");
        }

        public async Task<ServiceResult<bool>> UpdateBookingStatusAsync(Guid id)
        {
            var booking = await _bookingUnitOfWork.GetBookingRepository.Get(booking => booking.Id == id);

            if (booking is null)
                return ServiceResult<bool>.Failure($"No booking was found using this id: {id}");

            if (booking.IsCancelled)
            {
                booking.MarkAsCompleted(); 
            }
            else
            {
                booking.Cancel();
            }

            var result = await _bookingUnitOfWork.UpdateBookingRepository.UpdateAsync(booking);

            return result ?
                ServiceResult<bool>.Success("Booking was updated!") :
                ServiceResult<bool>.Failure("Couldn't update booking");
        }
         
        public async Task ConsumeBookingsUpdateFromVehicleService()
        {
            Console.WriteLine($"Start Prepare for Messages from RabbitMq at : {DateTime.UtcNow}");

            //RabbitMq connection
            //Port : 5672
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "messages",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine($"Waiting for messages from RabbitMq at : {DateTime.UtcNow}");

            var consumer = new AsyncEventingBasicConsumer(channel);
            try
            {
                consumer.ReceivedAsync += async (sender, eventArgs) =>
                {
                    byte[] body = eventArgs.Body.ToArray();

                    List<(Guid vehicleId, Guid userId)> viewedBookings =
                            JsonSerializer.Deserialize<List<(Guid vehicleId, Guid userId)>>(body);

                    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                };
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while getting messages at : {DateTime.UtcNow} - exception : {ex.Message}");
            }
            await channel.BasicConsumeAsync("messages", autoAck: false, consumer);

            Console.WriteLine($"Messages was received from RabbitMq at : {DateTime.UtcNow}");
        }

        public async Task ResponseToValidationRequest()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "validate-booking",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var bookingId = JsonSerializer.Deserialize<Guid>(ea.Body.ToArray());

                    bool isValidUser = await _bookingUnitOfWork.GetBookingRepository.Get(bookingId) is not null;

                    var responseBytes = JsonSerializer.SerializeToUtf8Bytes(isValidUser);

                    var props = new BasicProperties
                    {
                        CorrelationId = ea.BasicProperties.CorrelationId
                    };

                    props.CorrelationId = ea.BasicProperties.CorrelationId;

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: ea.BasicProperties.ReplyTo,
                        mandatory: false,
                        basicProperties: props,
                        body: responseBytes
                    );
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Validation response failed: {ex.Message}");
                }
            };

            await channel.BasicConsumeAsync(
                queue: "validate-booking",
                autoAck: false,
                consumer: consumer
            );

            Console.WriteLine("Response was sent to consumer successfully");
        }
    }
}
