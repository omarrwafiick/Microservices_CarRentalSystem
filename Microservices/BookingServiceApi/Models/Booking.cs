namespace BookingServiceApi.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalCost { get; set; }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
