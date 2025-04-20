using System.ComponentModel.DataAnnotations;

namespace VehicleServiceApi.Dtos
{
    public record GetVehicleDto(Guid Id, string LicensePlate, string Make, string Model, int Year, string Type, decimal DailyRate, bool IsAvailable, string Location);
    public record CreateVehicleDto(
        [Required] [Length(7,7)] string LicensePlate, 
        [Required] string Make,
        [Required] string Model, 
        [Required] [Range(1900,2025)] int Year, 
        [Required] string Type,
        [Required][Range(1, 10)] decimal DailyRate, 
        [Required] bool IsAvailable, 
        [Required] string Location);
    public record UpdateVehicleDto(
        [Required] Guid Id,
        [Required][Length(7, 7)] string LicensePlate,
        [Required] string Type,
        [Required] decimal DailyRate,
        [Required] bool IsAvailable,
        [Required] string Location);
}
