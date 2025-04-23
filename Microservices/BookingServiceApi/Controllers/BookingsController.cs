using BookingServiceApi.Dtos;
using BookingServiceApi.Extensions;
using BookingServiceApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace BookingServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController(IBookingService bookingService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllBookings() { 
            var bookings = await bookingService.GetAllBookingsAsync();
            return bookings.Any() ? Ok(bookings.Select(x=>x.MapFromDomainToDto())) : NotFound("No booking was found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById([FromRoute] Guid id) {
            var booking = await bookingService.GetBookingByIdAsync(id);
            return booking is not null ? Ok(booking.MapFromDomainToDto()) : NotFound("No booking was found");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingDto) {
            var newBooking = await bookingService.CreateBookingAsync(bookingDto.MapFromDtoToDomain());
            return newBooking ? Ok("New booking was created successfully") : BadRequest("Failed to create a new booking");
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking([FromRoute] Guid id) {
            var cancelBooking = await bookingService.CancelBookingAsync(id);
            return cancelBooking ? Ok("Booking was canceled successfully") : BadRequest("Failed to cancel booking");
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteBooking([FromRoute] Guid id) {
            var completeBooking = await bookingService.CompleteBookingAsync(id);
            return completeBooking ? Ok("Booking was completed successfully") : BadRequest("Failed to complete booking");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUser([FromRoute] Guid userId) {
            var bookings = await bookingService.GetBookingsByUserAsync(userId);
            return bookings.Any() ? Ok(bookings.Select(x => x.MapFromDomainToDto())) : NotFound("No booking was found to this user");
        }
    }
}
