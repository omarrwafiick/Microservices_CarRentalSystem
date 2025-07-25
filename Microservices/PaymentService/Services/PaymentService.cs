using Common.Dtos;
using Microsoft.Extensions.Caching.Memory;
using PaymentService.Models;
using PaymentServiceApi.Dtos;
using PaymentServiceApi.Enums;
using PaymentServiceApi.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Linq.Expressions;
using System.Text.Json;

namespace PaymentServiceApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentUnitOfWork _paymentUnitOfWork;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;
        public PaymentService(IPaymentUnitOfWork paymentUnitOfWork, ILogger logger, IMemoryCache cache)
        {
            _paymentUnitOfWork = paymentUnitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsAsync(HttpContext context)
        {
            _logger.LogInformation($"Request to get all payment records with ip: {context.Connection.RemoteIpAddress} - at: {DateTime.UtcNow}");
            
            var result = await _paymentUnitOfWork.GetAllPaymentRepository.GetAll(); 

            return result.Any() ?
                ServiceResult<List<PaymentRecord>>.Success("Payment records was found!", result.ToList()) :
                ServiceResult<List<PaymentRecord>>.Failure("Payment records was not found");
        }

        public async Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsByConditionAsync(HttpContext context, Expression<Func<PaymentRecord, bool>> condition)
        {
            _logger.LogInformation($"Request to get all payment records with ip: {context.Connection.RemoteIpAddress} - at: {DateTime.UtcNow}");

            var result = await _paymentUnitOfWork.GetAllPaymentRepository.GetAll(condition);

            return result.Any() ?
                ServiceResult<List<PaymentRecord>>.Success("Payment records was found!", result.ToList()) :
                ServiceResult<List<PaymentRecord>>.Failure("Payment records was not found");
        }

        public async Task<List<PaymentSummaryDto>> GetPaymentSummary(HttpContext context)
        {
            _logger.LogInformation($"Request to get payment summary records with ip: {context.Connection.RemoteIpAddress} - at: {DateTime.UtcNow}");

            var paymentRecords = await _paymentUnitOfWork.GetAllPaymentRepository.GetAll();

            var recordsGroupedByUserId = paymentRecords.GroupBy(x => x.UserId).ToList();

            var PaymentSummaryList = new List<PaymentSummaryDto>();

            foreach (var record in recordsGroupedByUserId) {
                PaymentSummaryList.Add(
                    new PaymentSummaryDto
                    {
                        UserId = record.FirstOrDefault().Id,

                        TotalBookings = record.Select(payment => payment.BookingId).Count(),

                        TotalAmountPaid = record.Select(payment => payment.Amount).Sum(),

                        PendingAmount = record.Where(payment => payment.Status == PaymentStatus.Pending)
                        .Select(payment => payment.Amount).Sum(),

                        RefundedAmount = record.Where(payment => payment.Status == PaymentStatus.Refunded)
                        .Select(payment => payment.Amount).Sum(),

                        SummaryFrom = record.FirstOrDefault().PaidAt,

                        SummaryTo = record.LastOrDefault().PaidAt,
                    }
                );
            }

            return PaymentSummaryList;
        }

        public async Task<ServiceResult<int>> RegisterPaymentRecordsAsync(HttpContext context, CreatePaymentDto dto)
        {
            _logger.LogInformation($"Request to add payment records with ip: {context.Connection.RemoteIpAddress} - at: {DateTime.UtcNow}");

            if (dto.PaidAt > DateTime.UtcNow)
            {
                return ServiceResult<int>.Failure("Invalid paid at date and time");
            }
            
            var userId = dto.UserId;
            var bookingId = dto.BookingId;

            await ValidateEntityViaMediator(userId, "validate-user");

            await ValidateEntityViaMediator(bookingId, "validate-booking");

            var paymentRecordExists = await _paymentUnitOfWork.GetPaymentRepository.Get(record =>
                record.UserId == userId && record.BookingId == bookingId && record.PaidAt == dto.PaidAt);

            if (paymentRecordExists is not null)
            {
                return ServiceResult<int>.Failure("Payment record already exists");
            }

            if (!ValidateEnumValue<PaymentMethod>(dto.Method) ||
                !ValidateEnumValue<CurrencyType>(dto.Currency) ||
                !ValidateEnumValue<PaymentStatus>(dto.Status) )
            {
                return ServiceResult<int>.Failure("Invalid enum values");
            }

            var newRecord = PaymentRecord.Create(
                bookingId,
                userId,
                dto.Amount,
                Enum.Parse<PaymentMethod>(dto.Method),
                Enum.Parse<PaymentStatus>(dto.Status),
                dto.TransactionId,
                Enum.Parse<CurrencyType>(dto.Currency),
                dto.PaidAt,
                dto.Notes
            );

            var result = await _paymentUnitOfWork.CreatePaymentRepository.CreateAsync(newRecord);

            if (!result)
            {
                _logger.LogError($"Failed to add new payment to system at: {DateTime.UtcNow}");
                return ServiceResult<int>.Failure("Failed to create new payment record");
            }
            _cache.Remove(Globals.CACHEKEY);

            _logger.LogInformation($"Successfully added new payment to system at: {DateTime.UtcNow} with id: {newRecord.Id}");

            return ServiceResult<int>.Success("Payment record was created successfully", newRecord.Id);
        }

        public async Task<ServiceResult<bool>> UpdatePaymentRecordsAsync(HttpContext context,int id, UpdatePaymentStatusDto dto)
        {
            var paymentRecord = await _paymentUnitOfWork.GetPaymentRepository.GetWithTracking(id);

            if (paymentRecord is null)
            {
                return ServiceResult<bool>.Failure("Payment record was not found");
            }

            if (ValidateEnumValue<PaymentMethod>(dto.NewStatus))
            {
                return ServiceResult<bool>.Failure("Invalid enum values");
            }

            paymentRecord.UpdatePayment(Enum.Parse<PaymentStatus>(dto.NewStatus), dto.AdminNote);

            var result = await _paymentUnitOfWork.UpdatePaymentRepository.UpdateAsync(paymentRecord);

            if (!result)
            {
                _logger.LogError($"Failed to update payment to system at: {DateTime.UtcNow} - with id: {paymentRecord.Id}");
                return ServiceResult<bool>.Failure("Failed to update payment record");
            }
            _cache.Remove(Globals.CACHEKEY);

            _logger.LogInformation($"Successfully updated payment to system at: {DateTime.UtcNow} with id: {paymentRecord.Id}");

            return ServiceResult<bool>.Success("Payment record was updated successfully");
        }

        private bool ValidateEnumValue<T>(string value) where T : struct, Enum
        {
            var names = Enum.GetNames(typeof(T));

            return names.Contains(value);
        }

        public async Task<ServiceResult<bool>> ValidateEntityViaMediator(int Id, string routingKey)
        {
            _logger.LogInformation($"Started to validate entity via mediator with id: {Id} - queue key: {routingKey} - at: {DateTime.UtcNow}");

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
             
            if (!isValidUser)
            {
                _logger.LogError($"Failed to validate entity with id: {Id} - queue key: {routingKey} - at: {DateTime.UtcNow}"); 
                ServiceResult<bool>.Failure("");
            }
            _logger.LogInformation($"Entity was validated successfully via mediator with id: {Id} - queue key: {routingKey} - at: {DateTime.UtcNow}");

            return ServiceResult<bool>.Success("", isValidUser);
        }  

    }
}
