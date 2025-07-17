using BookingServiceApi.Dtos;
using BookingServiceApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;
using Common.Helpers;
using AutoMapper; 
using Common.Dtos;

namespace BookingServiceApi.Controllers
{
    //[AuthorizeRoles("RENTER")]
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController(IBookingService bookingService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        { 
            var result = await bookingService.GetBookingsAsync();

            return result.SuccessOrNot ?
                Ok( new { message = result.Message, data = mapper.Map<GetBookingDto>(result.Data) } ): 
                BadRequest(new { message = result.Message });
        } 

        [HttpGet("{bookingid}")]
        public async Task<IActionResult> GetBookingById([FromRoute] string bookingid)
        {
           if(Guid.TryParse(bookingid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await bookingService.GetBookingsByConditionAsync(b => b.Id == id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetBookingDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetBookingsByUserId([FromRoute] string userid)
        { 
            if (Guid.TryParse(userid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await bookingService.GetBookingsByConditionAsync(b => b.RenterId == id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetBookingDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("vehicle/{vehicleid}")]
        public async Task<IActionResult> GetBookingsByVehicleId([FromRoute] string vehicleid)
        {
            if (Guid.TryParse(vehicleid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await bookingService.GetBookingsByConditionAsync(b => b.VehicleId == id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetBookingDto>>(result.Data) }) :
                BadRequest(new { message = result });
        }

        [HttpGet("status/{bookingid}")]
        public async Task<IActionResult> GetBookingStatus([FromRoute] string bookingid)
        {
            if (Guid.TryParse(bookingid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await bookingService.GetBookingsByConditionAsync(b => b.Id == id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = new { isCancelled = result.Data[0].IsCancelled } }) :
                BadRequest(new { message = result });
        }   

        [HttpPost]
        public async Task<IActionResult> RegisterBooking([FromBody] CreateBookingDto dto)
        { 
            var result = await bookingService.RegisterBookingAsync(dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("complete/{bookingid}")]
        public async Task<IActionResult> CompleteBooking([FromBody] string bookingid)
        { 
            var result = await UpdateBookingStatusCommon(bookingid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("cancel/{bookingid}")]
        public async Task<IActionResult> CancelBooking([FromBody] string bookingid)
        {
            var result = await UpdateBookingStatusCommon(bookingid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        } 

        private async Task<ServiceResult<bool>> UpdateBookingStatusCommon(string bookingid)
        { 
            if (Guid.TryParse(bookingid, out Guid id))
                return ServiceResult<bool>.Failure("Invalid id");

            var result = await bookingService.UpdateBookingStatusAsync(id);

            return result;
        }
    }
}
