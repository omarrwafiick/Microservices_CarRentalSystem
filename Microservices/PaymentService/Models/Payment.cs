using Common.Interfaces;
using PaymentServiceApi.Models;

namespace PaymentService.Models
{
    public class Payment : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentMethod Method { get; set; }
        public Guid PaymentStatusId { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaidAt = DateTime.UtcNow;
    } 
}
