using BookingServiceApi.Dtos;
using BookingServiceApi.Extensions;
using BookingServiceApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace BookingServiceApi.Controllers
{
    [Route("api/bookings")]
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

        [HttpPost("cancel/{userId}/{vehicleId}")]
        public async Task<IActionResult> CancelBooking([FromRoute] Guid userId, [FromRoute] Guid vehicleId) {
            var cancelBooking = await bookingService.CancelBookingAsync(userId, vehicleId);
            //cancel
            return cancelBooking ? Ok("Booking was canceled successfully") : BadRequest("Failed to cancel booking");
        }
     
        [HttpPost("dislike/{userId}/{vehicleId}")]
        public async Task<IActionResult> DislikeBooking([FromRoute] Guid userId, [FromRoute] Guid vehicleId)
        {
            var cancelBooking = await bookingService.CancelBookingAsync(userId, vehicleId);
            //cancel
            return cancelBooking ? Ok("Booking was canceled successfully") : BadRequest("Failed to cancel booking");
        }
        
        [HttpPut("view/range")]
        public async Task<IActionResult> ViewBookings([FromBody] List<(Guid vehicleId, Guid userId)> viewedBookings)
        {
            var cancelBooking = await bookingService.RecordViewBookingsAsync(viewedBookings);
            
            return cancelBooking ? Ok("Bookings was recorded viewed successfully") : BadRequest("Failed to record view bookings");
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteBooking([FromBody] CompleteBookingDto dto) {
            var completeBooking = await bookingService.CompleteBookingAsync(dto);

            return completeBooking ? Ok("Booking was completed as booked successfully") : BadRequest("Failed to complete booking");
        }
 
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUser([FromRoute] Guid userId) {
            var bookings = await bookingService.GetBookingsByUserAsync(userId);
            return bookings.Any() ? Ok(bookings.Select(x => x.MapFromDomainToDto())) : NotFound("No booking was found to this user");
        }
    }
}
