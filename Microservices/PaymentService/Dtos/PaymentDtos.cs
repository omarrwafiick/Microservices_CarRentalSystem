

using System.ComponentModel.DataAnnotations;

namespace PaymentServiceApi.Dtos
{
    public record GetPaymentDto(Guid Id, Guid BookingId, Guid UserId, decimal Amount, Guid PaymentMethodId, Guid PaymentStatusId);
    public record CreatePaymentDto(
        [Required] Guid BookingId, 
        [Required] Guid UserId, 
        [Required] decimal Amount, 
        [Required] Guid PaymentMethodId, 
        [Required] Guid PaymentStatusId);
}
