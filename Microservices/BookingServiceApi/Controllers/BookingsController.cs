using BookingServiceApi.Dtos;
using BookingServiceApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;
using Common.Helpers;
using AutoMapper; 
using Common.Dtos;
using BookingServiceApi.Models;

namespace BookingServiceApi.Controllers
{
    //[AuthorizeRoles("RENTER")]
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController(IBookingService bookingService, IMapper mapper) : ControllerBase
    {
        //[AuthorizeRoles("ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        { 
            var result = await bookingService.GetBookingsAsync();

            return result.SuccessOrNot ?
                Ok( new { message = result.Message, data = mapper.Map<GetBookingDto>(result.Data) } ): 
                BadRequest(new { message = result.Message });
        }

        [HttpGet("current/{bookingid:int}")]
        public async Task<IActionResult> GetCurrentBooking([FromRoute] int bookingid)
        {
            var result = await bookingService.GetCurrentBookingLocationsAsync(bookingid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<GetPickUpDto>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("{bookingid:int}")]
        public async Task<IActionResult> GetBookingById([FromRoute] int bookingid)
        {   
            var result = await bookingService.GetBookingsByConditionAsync(b => b.Id == bookingid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetBookingDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("user/{userid:int}")]
        public async Task<IActionResult> GetBookingsByUserId([FromRoute] int userid)
        {   
            var result = await bookingService.GetBookingsByConditionAsync(b => b.RenterId == userid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetBookingDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("vehicle/{vehicleid:int}")]
        public async Task<IActionResult> GetBookingsByVehicleId([FromRoute] int vehicleid)
        {  
            var result = await bookingService.GetBookingsByConditionAsync(b => b.VehicleId == vehicleid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetBookingDto>>(result.Data) }) :
                BadRequest(new { message = result });
        }

        [HttpGet("status/{bookingid:int}")]
        public async Task<IActionResult> GetBookingStatus([FromRoute] int bookingid)
        { 
            var result = await bookingService.GetBookingsByConditionAsync(b => b.Id == bookingid);

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

        [HttpPut("complete/{bookingid:int}")]
        public async Task<IActionResult> CompleteBooking([FromBody] int bookingid)
        { 
            var result = await UpdateBookingStatusCommon(bookingid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("cancel/{bookingid:int}")]
        public async Task<IActionResult> CancelBooking([FromBody] int bookingid)
        {
            var result = await UpdateBookingStatusCommon(bookingid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        } 

        private async Task<ServiceResult<bool>> UpdateBookingStatusCommon(int bookingid)
        {  
            var result = await bookingService.UpdateBookingStatusAsync(bookingid);

            return result;
        }
    }
}
