using Common.Interfaces;

namespace VehicleServiceApi.Models
{
    public class Location : IBaseEntity
    {
        public Guid Id { get; set; } 
        public List<Vehicle> Vehicles { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
