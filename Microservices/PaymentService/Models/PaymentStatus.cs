using Common.Interfaces;

namespace PaymentServiceApi.Models
{
    public class PaymentStatus : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
