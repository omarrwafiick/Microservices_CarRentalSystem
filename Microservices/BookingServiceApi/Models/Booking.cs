using Common.Interfaces;

namespace BookingServiceApi.Models
{
    public class Booking : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public Guid BookingStatusId { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalCost { get; set; }
    }
     
}
