 
using BookingServiceApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace BookingServiceApi.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController(IBookingService bookingService) : ControllerBase
    {
         
    }
}
