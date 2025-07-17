using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Dtos;
using PaymentServiceApi.Interfaces;
using Common.Helpers;
using AutoMapper;

namespace PaymentService.Controllers
{
    //[AuthorizeRoles("RENTER")]
    [Route("api/paymentrecords")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService, IMapper mapper) : ControllerBase
    {
        //[AuthorizeRoles("ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {  
            var result = await paymentService.GetPaymentRecordsAsync();

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetPaymentDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        //[AuthorizeRoles("ADMIN")]
        [HttpGet("summary")]
        public async Task<IActionResult> GetPaymentSummary()
        {  
            var result = await paymentService.GetPaymentSummary();

            return Ok(new { data = result });
        }

        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetUserPayments([FromRoute] string userid)
        {
            if (Guid.TryParse(userid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await paymentService.GetPaymentRecordsByConditionAsync(
                paymentRecord => paymentRecord.UserId == id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetPaymentDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("booking/{bookingid}")]
        public async Task<IActionResult> GetBookingPayment([FromRoute] string bookingid)
        {
            if (Guid.TryParse(bookingid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await paymentService.GetPaymentRecordsByConditionAsync(
                paymentRecord => paymentRecord.BookingId == id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetPaymentDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPayment([FromBody] CreatePaymentDto dto)
        { 
            var result = await paymentService.RegisterPaymentRecordsAsync(dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("status/{paymentid}")]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] string paymentid, [FromBody] UpdatePaymentStatusDto dto)
        {
            if (Guid.TryParse(paymentid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await paymentService.UpdatePaymentRecordsAsync(id, dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }
    }
}
