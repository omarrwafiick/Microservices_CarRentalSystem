
 
using System.ComponentModel.DataAnnotations;

namespace PaymentServiceApi.Dtos
{
    public record CreatePaymentDto
    {
        [Required]
        public Guid BookingId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Range(0.01, 1_000_000)]
        public decimal Amount { get; set; }

        [Required]
        public string Method { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [StringLength(100)]
        public string TransactionId { get; set; } 

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }  

        [MaxLength(250)]
        public string Notes { get; set; }
    }

    public record GetPaymentDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }

        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }

        public DateTime PaidAt { get; set; }

        public string TransactionId { get; set; }
        public string ReferenceCode { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public record UpdatePaymentStatusDto
    {
        [Required]
        public Guid PaymentId { get; set; }

        [Required]
        public string NewStatus { get; set; }

        [MaxLength(250)]
        public string AdminNote { get; set; }
    }
}
