
 
using System.ComponentModel.DataAnnotations;

namespace PaymentServiceApi.Dtos
{
    public record CreatePaymentDto
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0.01, 1_000_000)]
        public decimal Amount { get; set; }

        [Required]
        public string Method { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [StringLength(100)]
        public int TransactionId { get; set; } 

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }  

        [MaxLength(250)]
        public string Notes { get; set; }

        [Required]
        public DateTime PaidAt { get; set; }
    }

    public record GetPaymentDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }

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
        public string NewStatus { get; set; }

        [MaxLength(250)]
        public string AdminNote { get; set; }
    }

    public record PaymentSummaryDto
    {
        public int UserId { get; set; } 
        public int TotalBookings { get; set; }
        public decimal TotalAmountPaid { get; set; } 
        public decimal PendingAmount { get; set; }
        public decimal RefundedAmount { get; set; }
        public DateTime SummaryFrom { get; set; }
        public DateTime SummaryTo { get; set; }
    }

}
