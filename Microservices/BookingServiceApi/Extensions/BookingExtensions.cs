using BookingServiceApi.Dtos;
using BookingServiceApi.Models;

namespace BookingServiceApi.Extensions
{
    public static class BookingExtensions
    {
        public static GetBookingDto MapFromDomainToDto(this Booking domain)
        { 
            var startDate = domain.StartDate is not null ? domain.StartDate : DateTime.UtcNow;
            var endDate = domain.EndDate is not null ? domain.EndDate : DateTime.UtcNow;
            return new GetBookingDto(
                domain.Id,
                domain.VehicleId,
                domain.RenterId,
                startDate,
                endDate,
                domain.InteractionType
                );
        }
        public static Booking MapFromDtoToDomain(this CreateBookingDto dto)
        {
            return new Booking
            { 
                VehicleId = dto.VehicleId,
                RenterId = dto.UserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                InteractionType = dto.InteractionType
            };
        }
    }
}
