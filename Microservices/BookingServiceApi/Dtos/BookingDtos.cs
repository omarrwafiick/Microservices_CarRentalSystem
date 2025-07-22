 
using System.ComponentModel.DataAnnotations;

namespace BookingServiceApi.Dtos
{
    public record CreateBookingDto
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int RenterId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string InteractionType { get; set; }

        [Required]
        [Range(0.01, 1_000_000)]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(200)]
        public string PickupLocation { get; set; }

        [Required]
        [StringLength(200)]
        public string DropoffLocation { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
    }

    public record GetBookingDto
    {
        public int Id { get; set; }

        public int VehicleId { get; set; } 

        public int RenterId { get; set; } 

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string InteractionType { get; set; }

        public decimal TotalPrice { get; set; }

        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string Notes { get; set; }

        public DateTime RecordedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
    } 
   
}
