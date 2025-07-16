

using VehicleServiceApi.Enums;

namespace VehicleServiceApi.Models
{
    public class Vehicle : BaseEntity
    {
        private Vehicle()
        {
        }
         
        private Vehicle(
            Guid ownerId,
            Guid currentLocationId,
            string licensePlate,
            string vin,
            string make,
            string model,
            int year,
            VehicleType vehicleType,
            DateTime registrationExpiryDate,
            string insurancePolicyNumber,
            DateTime insuranceExpiryDate,
            decimal dailyRate,
            decimal mileage,
            FuelType fuelType,
            TransmissionType transmission,
            bool isGpsEnabled,
            bool isElectric,
            bool isActive,
            DateTime lastServiceDate,
            int serviceIntervalKm)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            CurrentLocationId = currentLocationId;
            LicensePlate = licensePlate;
            VIN = vin;
            Make = make;
            Model = model;
            Year = year;
            VehicleType = vehicleType;
            VehicleStatus = VehicleStatus.Available;
            RegistrationExpiryDate = registrationExpiryDate;
            InsurancePolicyNumber = insurancePolicyNumber;
            InsuranceExpiryDate = insuranceExpiryDate;
            DailyRate = dailyRate;
            Mileage = mileage;
            FuelType = fuelType;
            Transmission = transmission;
            IsGpsEnabled = isGpsEnabled;
            IsElectric = isElectric;
            IsActive = isActive;
            LastServiceDate = lastServiceDate;
            ServiceIntervalKm = serviceIntervalKm;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        } 
        public static Vehicle Factory(
            Guid ownerId,
            Guid currentLocationId,
            string licensePlate,
            string vin,
            string make,
            string model,
            int year,
            VehicleType vehicleType,
            DateTime registrationExpiryDate,
            string insurancePolicyNumber,
            DateTime insuranceExpiryDate,
            decimal dailyRate,
            decimal mileage,
            FuelType fuelType,
            TransmissionType transmission,
            bool isGpsEnabled,
            bool isElectric,
            bool isActive,
            DateTime lastServiceDate,
            int serviceIntervalKm) =>
            new Vehicle(
                ownerId,
                currentLocationId,
                licensePlate,
                vin,
                make,
                model,
                year,
                vehicleType,
                registrationExpiryDate,
                insurancePolicyNumber,
                insuranceExpiryDate,
                dailyRate,
                mileage,
                fuelType,
                transmission,
                isGpsEnabled,
                isElectric,
                isActive,
                lastServiceDate,
                serviceIntervalKm); 
        public Guid OwnerId { get; private set; }
        public Guid CurrentLocationId { get; private set; }
        public Location Location { get; private set; } 
        public string LicensePlate { get; private set; }
        public string VIN { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; } 
        public VehicleType VehicleType { get; private set; }
        public VehicleStatus VehicleStatus { get; private set; } 
        public DateTime RegistrationExpiryDate { get; private set; }
        public string InsurancePolicyNumber { get; private set; }
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
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
