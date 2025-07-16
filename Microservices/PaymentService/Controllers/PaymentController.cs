using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Dtos;
using PaymentServiceApi.Interfaces;
using Common.Helpers;

namespace PaymentService.Controllers
{
    //[AuthorizeRoles("RENTER")]
    [Route("api/paymentrecords")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            return Ok();
        }
        
        [HttpGet("{userid}")]
        public async Task<IActionResult> GetUserPayments([FromRoute] string userid)
        {
            return Ok();
        }   
         
        [HttpGet("summary")]
        public async Task<IActionResult> GetPaymentSummary()
        {
            return Ok(); 
        }

        [HttpGet("{bookingid}")]
        public async Task<IActionResult> GetBookingPayment([FromRoute] string bookingid)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPayment([FromBody] CreatePaymentDto dto)
        {
            return Ok();
        }

        [HttpPut("status/{paymentid}")]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] string paymentid, [FromBody] UpdatePaymentStatusDto dto)
        {
            return Ok();
        }
    }
}
