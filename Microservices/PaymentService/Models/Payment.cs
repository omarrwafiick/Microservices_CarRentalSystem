namespace PaymentService.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaidAt = DateTime.UtcNow;
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        PayPal,
        Wallet
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
}
