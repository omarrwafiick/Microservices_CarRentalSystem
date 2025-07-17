

using BookingServiceApi.Dtos;
using BookingServiceApi.Models;
using Common.Dtos;
using System.Linq.Expressions;

namespace BookingServiceApi.Interfaces
{
    public interface IBookingService
    {
        Task<ServiceResult<List<Booking>>> GetBookingsAsync();
        Task<ServiceResult<List<Booking>>> GetBookingsByConditionAsync(Expression<Func<Booking, bool>> condition); 
        Task<ServiceResult<bool>> RegisterBookingAsync(CreateBookingDto dto);
        Task<ServiceResult<bool>> UpdateBookingStatusAsync(Guid id);
        Task ConsumeBookingsUpdateFromVehicleService();
    }
}
