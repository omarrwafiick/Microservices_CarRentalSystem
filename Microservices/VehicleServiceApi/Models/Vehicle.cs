using VehicleServiceApi.Enums;
using Common.Models;

namespace VehicleServiceApi.Models
{
    public class Vehicle : BaseEntity
    {
        private Vehicle()
        {
        }
          
        public static Vehicle Factory(
            int ownerId,
            int currentLocationId,
            string licensePlate,
            string vin,
            int ModelId,
            int year,
            VehicleType vehicleType,
            DateTime registrationExpiryDate,
            long insurancePolicyNumber,
            DateTime insuranceExpiryDate,
            decimal dailyRate,
            decimal mileage,
            FuelType fuelType,
            TransmissionType transmission,
            bool isGpsEnabled,
            bool isElectric, 
            DateTime lastServiceDate,
            int serviceIntervalKm) =>
            new Vehicle
            { 
                OwnerId = ownerId,
                CurrentLocationId = currentLocationId,
                LicensePlate = licensePlate,
                VIN = vin, 
                ModelId = ModelId,
                Year = year,
                VehicleType = vehicleType,
                VehicleStatus = VehicleStatus.Available,
                RegistrationExpiryDate = registrationExpiryDate,
                InsurancePolicyNumber = insurancePolicyNumber,
                InsuranceExpiryDate = insuranceExpiryDate,
                DailyRate = dailyRate,
                Mileage = mileage,
                FuelType = fuelType,
                Transmission = transmission,
                IsGpsEnabled = isGpsEnabled,
                IsElectric = isElectric,
                IsActive = true,
                LastServiceDate = lastServiceDate,
                ServiceIntervalKm = serviceIntervalKm,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }; 

        public int OwnerId { get; private set; }
        public int CurrentLocationId { get; private set; }
        public Location Location { get; private set; } 
        public string LicensePlate { get; private set; }
        public string VIN { get; private set; } 
        public int ModelId { get; private set; }
        public VehicleModel Model { get; private set; }
        public int Year { get; private set; } 
        public VehicleType VehicleType { get; private set; }
        public VehicleStatus VehicleStatus { get; private set; } 
        public DateTime RegistrationExpiryDate { get; private set; }
        public long InsurancePolicyNumber { get; private set; }
        public DateTime InsuranceExpiryDate { get; private set; } 
        public decimal DailyRate { get; private set; }
        public decimal Mileage { get; private set; }
        public FuelType FuelType { get; private set; }
        public TransmissionType Transmission { get; private set; } 
        public int PopularityScore { get; private set; }
        public bool IsGpsEnabled { get; private set; }
        public bool IsElectric { get; private set; }
        public bool IsActive { get; private set; }  
        public DateTime LastServiceDate { get; private set; }
        public int ServiceIntervalKm { get; private set; } 
        public List<VehicleImages> VehicleImages { get; private set; } = new();
        public List<MaintenanceCenter> MaintenanceCenters { get; private set; } = new();
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public void UpdatePopularityScore(int newValue)
        {
            PopularityScore = newValue;
            MarkAsUpdated();
        }

        public void UpdateDailyRate(decimal newValue)
        {
            DailyRate = newValue;
            MarkAsUpdated();
        }

        public void Deactivate()
        {
            if (IsActive)
            { 
                IsActive = false;
            }
        }

        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
            }
        }

        public void UpdateStatus(VehicleStatus newVehicleStatus)
        {
            VehicleStatus = newVehicleStatus;
            MarkAsUpdated();
        }

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
