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

        [HttpGet("user/{userid:int}")]
        public async Task<IActionResult> GetUserPayments([FromRoute] int userid)
        {  
            var result = await paymentService.GetPaymentRecordsByConditionAsync(
                paymentRecord => paymentRecord.UserId == userid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetPaymentDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("booking/{bookingid:int}")]
        public async Task<IActionResult> GetBookingPayment([FromRoute] int bookingid)
        {  
            var result = await paymentService.GetPaymentRecordsByConditionAsync(
                paymentRecord => paymentRecord.BookingId == bookingid);

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

        [HttpPut("status/{paymentid:int}")]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] int paymentid, [FromBody] UpdatePaymentStatusDto dto)
        {  
            var result = await paymentService.UpdatePaymentRecordsAsync(paymentid, dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }
    }
}
