using PaymentService.Models;
using PaymentServiceApi.Dtos;

namespace PaymentServiceApi.Extensions
{
    public static class PaymentExtensions
    {
        public static GetPaymentDto MapFromDomainToDto(this Payment domain)
        {
            return new GetPaymentDto(
                domain.Id,
                domain.BookingId,
                domain.UserId,
                domain.Amount,
                domain.PaymentMethodId,
                domain.PaymentStatusId
                );
        }

        public static Payment MapFromDtoToDomain(this CreatePaymentDto dto)
        {
            return new Payment
            {
                BookingId = dto.BookingId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                PaymentMethodId = dto.PaymentMethodId,
                PaymentStatusId = dto.PaymentStatusId
            };
        }
    }
}
