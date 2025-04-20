using BookingServiceApi.Dtos;
using BookingServiceApi.Models;

namespace BookingServiceApi.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(Guid id);
        Task<bool> CreateBookingAsync(Booking domain);
        Task<bool> CancelBookingAsync(Guid id);
        Task<bool> CompleteBookingAsync(Guid id);
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid id);
    }
}
