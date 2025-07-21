using System.ComponentModel.DataAnnotations;
 

namespace VehicleServiceApi.Dtos
{
    public record GetVehicleDto
    {
        public Guid Id { get; set; } 
        public string LicensePlate { get; set; } 
        public string Model { get; set; }
        public int Year { get; set; } 
        public string VehicleType { get; set; }
        public string VehicleStatus { get; set; } 
        public decimal DailyRate { get; set; }
        public decimal Mileage { get; set; } 
        public string FuelType { get; set; }
        public string Transmission { get; set; } 
        public bool IsGpsEnabled { get; set; }
        public bool IsElectric { get; set; } 
        public DateTime LastServiceDate { get; set; }
        public int ServiceIntervalKm { get; set; } 
        public string LocationName { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }  
        public List<string> ImageUrls { get; set; } = new();
    }
    
    public record CreateVehicleDto
    {
        [Required]
        public string OwnerId { get; set; }

        [Required]
        public string CurrentLocationId { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string LicensePlate { get; set; }

        [Required]
        [StringLength(17, MinimumLength = 11, ErrorMessage = "VIN must be between 11 and 17 characters.")]
        public string VIN { get; set; }
         

        [Required] 
        public string ModelId { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        public string VehicleType { get; set; }

        [Required]
        public DateTime RegistrationExpiryDate { get; set; }

        [Required]
        [StringLength(50)]
        public string InsurancePolicyNumber { get; set; }

        [Required]
        public DateTime InsuranceExpiryDate { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal DailyRate { get; set; }

        [Range(0, 1000000)]
        public decimal Mileage { get; set; }

        [Required]
        public string FuelType { get; set; }

        [Required]
        public string Transmission { get; set; }

        [Range(0, 100)]
        public int PopularityScore { get; set; }

        public bool IsGpsEnabled { get; set; }
        public bool IsElectric { get; set; }
        public bool IsActive { get; set; }

        public DateTime LastServiceDate { get; set; }

        [Range(1000, 50000)]
        public int ServiceIntervalKm { get; set; }
    }
  
    public record RecommendationDto(
        [Required]
        Guid userId,
        [Required]
        string vehicleType,
        [Required]
        DateTime rentalStartDate,
        [Required]
        DateTime rentalEndDate,
        [Required]
        string district,
        [Required]
        string city,
        List<UserBookingRecordDto> userBookings,
        List<UserBookingRecordDto> bookingRecords,
        List<GetMaintenanceRecordDto> maintenanceRecords
    );
 

    public enum InteractionType
    {
        VIEWED,
        BOOKED,
        CANCELLED,
        DISLIKED
    }

}
