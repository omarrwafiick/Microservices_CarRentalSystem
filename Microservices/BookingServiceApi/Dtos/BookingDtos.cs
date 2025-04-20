
using System.ComponentModel.DataAnnotations;

namespace BookingServiceApi.Dtos
{
     public record GetBookingDto(Guid Id, Guid VehicleId, Guid UserId, DateTime EndDate, Guid BookingStatusId, decimal TotalCost);
     public record CreateBookingDto(
         [Required] Guid VehicleId, 
         [Required] Guid UserId, 
         [Required] DateTime EndDate, 
         [Required] Guid BookingStatusId,
         [Required] decimal TotalCost); 
}
