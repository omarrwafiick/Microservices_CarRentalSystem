using System.ComponentModel.DataAnnotations;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Dtos
{
    public record GetVehicleDto(Guid Id, string LicensePlate, string Make, string Model, int Year, VehicleType VehicleType, decimal DailyRate, VehicleStatus VehicleStatus, Guid LocationId);
    public record CreateVehicleDto(
        [Required] [Length(7,7)] string LicensePlate, 
        [Required] string Make,
        [Required] string Model, 
        [Required] [Range(1900,2025)] int Year, 
        [Required] VehicleType VehicleType,
        [Required][Range(1, 10)] decimal DailyRate,  
        [Required] VehicleStatus VehicleStatus, 
        [Required] Guid LocationId);
    public record UpdateVehicleDto( 
        [Required] VehicleType VehicleType,
        [Required] decimal DailyRate,
        [Required] VehicleStatus VehicleStatus,
        [Required] Guid LocationId);

    public record RecommendationDto(
        [Required]
        Guid userId,
        [Required]
        VehicleType vehicleType,
        [Required]
        DateTime rentalStartDate,
        [Required]
        DateTime rentalEndDate,
        [Required]
        string district,
        [Required]
        string city,
        List<UserBookingRecords> userBookings,
        List<UserBookingRecords> bookingRecords
        );

    public record UserBookingRecords(
        Guid Id,
        Guid VehicleId,
        Guid RenterId,
        Guid? ProviderId,
        DateTime StartDate,
        DateTime EndDate,
        InteractionType InteractionType,
        Guid BookingStatusId, 
        DateTime RecordedAt,
        decimal TotalCost
    );

    public enum InteractionType
    {
        VIEWED,
        BOOKED,
        CANCELLED,
        DISLIKED
    }

    public record LocationDto(
        string District,
        string City,
        double Longitude,
        double Latitude
    ); 
}
