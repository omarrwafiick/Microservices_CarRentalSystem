using BookingServiceApi.Dtos;
using BookingServiceApi.Models;

namespace BookingServiceApi.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(Guid id);
        Task<bool> CreateBookingAsync(Booking domain);
        Task<bool> CancelBookingAsync(Guid userId, Guid vehicleId);
        Task<bool> CompleteBookingAsync(CompleteBookingDto dto);
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid id);
        Task<bool> RecordViewBookingsAsync(List<(Guid vehicleId, Guid userId)> viewedBookings);
        Task<bool> DislikeBookingAsync(Guid userId, Guid vehicleId);
    }
}
