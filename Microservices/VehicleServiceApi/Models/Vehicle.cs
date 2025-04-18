namespace VehicleServiceApi.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public decimal DailyRate { get; set; }
        public bool IsAvailable { get; set; }
        public string Location { get; set; }
    }
}
