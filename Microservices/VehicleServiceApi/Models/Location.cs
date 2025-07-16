 
namespace VehicleServiceApi.Models
{
    public class Location : BaseEntity
    {
        private Location()
        { 
        }
        private Location(
            string name, 
            string district, 
            string city, 
            string country, 
            double longitude, 
            double latitude)
        {
            Id = Guid.NewGuid();
            Name = name;
            District = district;
            City = city;
            Country = country;
            Longitude = longitude;
            Latitude = latitude;
            IsActive = true;
        }
        public static Location Factory(
            string name, 
            string district, 
            string city,
            string country, 
            double longitude, 
            double latitude) =>
             new Location(
                 name, 
                 district, 
                 city, 
                 country, 
                 longitude, 
                 latitude);

        public string Name { get; private set; }  
        public string District { get; private set;  }
        public string City { get; private set; }
        public string Country { get; private set; } 
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        public bool IsActive { get; private set; }

        public void DeactivateLocation()
        {
            IsActive = false;   
        }

        public void ActivateLocation()
        {
            IsActive = true;
        }

        public List<Vehicle> Vehicles { get; private set; } = new();  
    }
}
