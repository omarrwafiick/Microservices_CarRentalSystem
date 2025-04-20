using BookingServiceApi.Dtos;
using BookingServiceApi.Models;

namespace BookingServiceApi.Extensions
{
    public static class BookingExtensions
    {
        public static GetBookingDto MapFromDomainToDto(this Booking domain)
        {
            return new GetBookingDto(
                domain.Id,
                domain.VehicleId,
                domain.UserId,
                domain.EndDate,
                domain.BookingStatusId,
                domain.TotalCost
                );
        }
        public static Booking MapFromDtoToDomain(this CreateBookingDto dto)
        {
            return new Booking
            { 
                VehicleId = dto.VehicleId,
                UserId = dto.UserId,
                EndDate = dto.EndDate,
                BookingStatusId = dto.BookingStatusId,
                TotalCost = dto.TotalCost
            };
        }
    }
}
