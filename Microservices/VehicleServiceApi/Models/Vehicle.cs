using Common.Interfaces;

namespace VehicleServiceApi.Models
{
    public class Vehicle: IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid CurrentLocationId { get; set; }
        public Location Location { get; set; }
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal DailyRate { get; set; }
        public int PopularityScore { get; set; }
        public VehicleType VehicleType { get; set; }
        public VehicleStatus VehicleStatus { get; set; }
    }
}
