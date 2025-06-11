using Common.Interfaces;

namespace BookingServiceApi.Models
{
    public class Booking : IBaseEntity
    {
        public Guid Id { get; set; } 
        public Guid VehicleId { get; set; } 
        public Guid RenterId { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public InteractionType InteractionType { get; set; } 
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow; 
    }
}
