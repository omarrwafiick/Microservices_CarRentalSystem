using BookingServiceApi.Dtos;
using BookingServiceApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;
using Common.Helpers;

namespace BookingServiceApi.Controllers
{

    //[AuthorizeRoles("RENTER")]
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController(IBookingService bookingService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            return Ok();
        } 

        [HttpGet("{bookingid}")]
        public async Task<IActionResult> GetBookingById([FromRoute] string bookingid)
        {
            return Ok();
        }

        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetBookingsByUserId([FromRoute] string userid)
        {
            return Ok();
        }

        [HttpGet("vehicle/{vehicleid}")]
        public async Task<IActionResult> GetBookingsByVehicleId([FromRoute] string vehicleid)
        {
            return Ok();
        }

        [HttpGet("status/{bookingid}")]
        public async Task<IActionResult> GetBookingStatus([FromRoute] string bookingid)
        {
            return Ok();
        }   

        [HttpPost]
        public async Task<IActionResult> RegisterBooking([FromBody] CreateBookingDto dto)
        {
            return Ok();
        }

        [HttpPut("complete/{bookingid}")]
        public async Task<IActionResult> CompleteBooking([FromBody] string bookingid)
        {
            return Ok();
        }

        [HttpPut("cancel/{bookingid}")]
        public async Task<IActionResult> CancelBooking([FromBody] string bookingid)
        {
            return Ok();
        }
    }
}
