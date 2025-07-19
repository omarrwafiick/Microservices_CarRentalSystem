using Common.Dtos;
using Common.Interfaces;
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
        private readonly IGetAllRepository<PaymentRecord> _getAllRepository;
        private readonly IGetRepository<PaymentRecord> _getRepository;
        private readonly ICreateRepository<PaymentRecord> _createRepository;
        private readonly IUpdateRepository<PaymentRecord> _updateRepository; 
        public PaymentService(
            IGetAllRepository<PaymentRecord> getAllRepository, 
            IGetRepository<PaymentRecord> getRepository, 
            ICreateRepository<PaymentRecord> createRepository, 
            IUpdateRepository<PaymentRecord> updateRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository; 
        }

        public async Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsAsync()
        {
            var result = await _getAllRepository.GetAll(); 

            return result.Any() ?
                ServiceResult<List<PaymentRecord>>.Success("Payment records was found!", result.ToList()) :
                ServiceResult<List<PaymentRecord>>.Failure("Payment records was not found");
        }

        public async Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsByConditionAsync(Expression<Func<PaymentRecord, bool>> condition)
        {
            var result = await _getAllRepository.GetAll(condition);

            return result.Any() ?
                ServiceResult<List<PaymentRecord>>.Success("Payment records was found!", result.ToList()) :
                ServiceResult<List<PaymentRecord>>.Failure("Payment records was not found");
        }

        public async Task<List<PaymentSummaryDto>> GetPaymentSummary()
        {
            var paymentRecords = await _getAllRepository.GetAll();

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

        public async Task<ServiceResult<bool>> RegisterPaymentRecordsAsync(CreatePaymentDto dto)
        {
            if (Guid.TryParse(dto.UserId, out Guid userId) || 
                Guid.TryParse(dto.BookingId, out Guid bookingId))
            {
                return ServiceResult<bool>.Failure("Invalid ids");
            }

            if(dto.PaidAt > DateTime.UtcNow)
            {
                return ServiceResult<bool>.Failure("Invalid paid at date and time");
            }

            await ValidateEntityViaMediator(userId, "validate-user");

            await ValidateEntityViaMediator(bookingId, "validate-booking");

            var paymentRecordExists = await _getRepository.Get(record =>
                record.UserId == userId && record.BookingId == bookingId && record.PaidAt == dto.PaidAt);

            if (paymentRecordExists is not null)
            {
                return ServiceResult<bool>.Failure("Payment record already exists");
            }

            if (!ValidateEnumValue<PaymentMethod>(dto.Method) ||
                !ValidateEnumValue<CurrencyType>(dto.Currency) ||
                !ValidateEnumValue<PaymentStatus>(dto.Status) )
            {
                return ServiceResult<bool>.Failure("Invalid enum values");
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

            var result = await _createRepository.CreateAsync(newRecord);

            return result ?
                ServiceResult<bool>.Success("Payment record was created successfully") :
                ServiceResult<bool>.Failure("Failed to create new payment record");
        }

        public async Task<ServiceResult<bool>> UpdatePaymentRecordsAsync(Guid id, UpdatePaymentStatusDto dto)
        {
            var paymentRecord = await _getRepository.Get(id);

            if (paymentRecord is null)
            {
                return ServiceResult<bool>.Failure("Payment record was not found");
            }

            if (ValidateEnumValue<PaymentMethod>(dto.NewStatus))
            {
                return ServiceResult<bool>.Failure("Invalid enum values");
            }

            paymentRecord.UpdatePayment(Enum.Parse<PaymentStatus>(dto.NewStatus), dto.AdminNote);

            var result = await _updateRepository.UpdateAsync(paymentRecord);

            return result ?
                ServiceResult<bool>.Success("Payment record was updated successfully") :
                ServiceResult<bool>.Failure("Failed to update payment record");
        }

        private bool ValidateEnumValue<T>(string value) where T : struct, Enum
        {
            var names = Enum.GetNames(typeof(T));

            return names.Contains(value);
        }

        public async Task<ServiceResult<bool>> ValidateEntityViaMediator(Guid Id, string routingKey)
        { 
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
             
            return isValidUser
                ? ServiceResult<bool>.Success("", isValidUser)
                : ServiceResult<bool>.Failure("");
        }  

    }
}
