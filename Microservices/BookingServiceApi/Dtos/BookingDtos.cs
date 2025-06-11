
using BookingServiceApi.Models;
using System.ComponentModel.DataAnnotations;

namespace BookingServiceApi.Dtos
{
     public record GetBookingDto(Guid Id, Guid VehicleId, Guid UserId, DateTime? StartDate, DateTime? EndDate, InteractionType InteractionType);
     public record CreateBookingDto(
         [Required] Guid VehicleId, 
         [Required] Guid UserId,
         [Required] DateTime StartDate,
         [Required] DateTime EndDate, 
         [Required] InteractionType InteractionType);

    public record CompleteBookingDto(
         [Required] Guid VehicleId,
         [Required] Guid UserId,
         [Required] DateTime StartDate,
         [Required] DateTime EndDate);
}
