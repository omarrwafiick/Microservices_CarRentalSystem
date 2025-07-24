﻿

using BookingServiceApi.Enums;
using Common.Models;

namespace BookingServiceApi.Models
{
    public class Booking : BaseEntity
    {
        private Booking() { }

        public static Booking Create(
            int vehicleId,
            int renterId,
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
                VehicleId = vehicleId,
                RenterId = renterId,
                StartDate = startDate,
                EndDate = endDate,
                InteractionType = interactionType,
                TotalPrice = totalPrice,
                PickupLocation = pickupLocation,
                DropoffLocation = dropoffLocation,
                Notes = notes,
                RecordedAt = DateTime.UtcNow,
                IsCancelled = false
            };
        }

        public int VehicleId { get; private set; }
        public int RenterId { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public InteractionType InteractionType { get; private set; }

        public decimal TotalPrice { get; private set; }

        public string PickupLocation { get; private set; }
        public string DropoffLocation { get; private set; }

        public string Notes { get; private set; }

        public DateTime RecordedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public bool IsCancelled { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsViewed()
        {
            InteractionType = InteractionType.VIEWED;
            MarkAsUpdated();
        }

        public void MarkAsCompleted()
        {
            CompletedAt = DateTime.UtcNow;
            MarkAsUpdated();
        }

        public void Cancel()
        {
            CancelledAt = DateTime.UtcNow;
            MarkAsUpdated();
        }
    }

}
