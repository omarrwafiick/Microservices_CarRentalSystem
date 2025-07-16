

using BookingServiceApi.Enums;
using Common.Models;

namespace BookingServiceApi.Models
{
    public class Booking : BaseEntity
    {
        public Guid VehicleId { get; private set; }
        public Guid RenterId { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public InteractionType InteractionType { get; private set; }

        public decimal TotalPrice { get; private set; }

        public string PickupLocation { get; private set; }
        public string DropoffLocation { get; private set; }

        public string Notes { get; private set; }

        public DateTime RecordedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private Booking() { }

        public static Booking Create(
            Guid vehicleId,
            Guid renterId,
            DateTime startDate,
            DateTime endDate,
            InteractionType interactionType,
            decimal totalPrice,
            string pickupLocation,
            string dropoffLocation,
            string notes)
        {   
            return new Booking
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicleId,
                RenterId = renterId,
                StartDate = startDate,
                EndDate = endDate,
                InteractionType = interactionType,
                TotalPrice = totalPrice,
                PickupLocation = pickupLocation,
                DropoffLocation = dropoffLocation,
                Notes = notes,
                RecordedAt = DateTime.UtcNow
            };
        }

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsCompleted()
        {
            CompletedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            CancelledAt = DateTime.UtcNow;
        }
    }

}
