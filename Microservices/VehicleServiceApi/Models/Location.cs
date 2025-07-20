
using Common.Models;

namespace VehicleServiceApi.Models
{
    public class Location : BaseEntity
    {
        private Location()
        { 
        } 
        public static Location Factory(
            string name, 
            string district, 
            string city,
            string country, 
            double longitude, 
            double latitude) =>
             new Location
             {

                 Name = name,
                 District = district,
                 City = city,
                 Country = country,
                 Longitude = longitude,
                 Latitude = latitude,
                 IsActive = true
             };

        public string Name { get; private set; }  
        public string District { get; private set;  }
        public string City { get; private set; }
        public string Country { get; private set; } 
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        public bool IsActive { get; private set; }

        public List<MaintenanceCenter> MaintenanceCenters { get; private set; } = new();

        public List<Vehicle> LocationVehicles { get; private set; } = new();
        public void DeactivateLocation()
        {
            IsActive = false;  
        }

        public void ActivateLocation()
        {
            IsActive = true;
        }

        public void UpdateAddress(string newDistrict, string newCity, string newCountry)
        {
            District = newDistrict;
            City = newCity;
            Country = newCountry;
        }

        public void Updatecoordinates(double newLongitude, double newLatitude)
        {
            Longitude = newLongitude; 
            Latitude = newLatitude;
        }

        public List<Vehicle> Vehicles { get; private set; } = new();  
    }
}
