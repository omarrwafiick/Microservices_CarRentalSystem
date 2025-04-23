using Microsoft.AspNetCore.Mvc; 
using PaymentServiceApi.Dtos;
using PaymentServiceApi.Extensions;
using PaymentServiceApi.Interfaces;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        [HttpGet] 
        public async Task<IActionResult> GetAllPayments() {
            var payments = await paymentService.GetAllPaymentsAsync();
            return payments.Any() ? Ok(payments.Select(x => x.MapFromDomainToDto())) : NotFound("No payment was found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById([FromRoute] Guid id) {
            var payment = await paymentService.GetPaymentByIdAsync(id);
            return payment is not null ? Ok(payment.MapFromDomainToDto()) : NotFound("No payment was found");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto) {
            var createPayments = await paymentService.CreatePaymentAsync(dto.MapFromDtoToDomain());
            return createPayments ? Ok("Payment was created successfully") : BadRequest("Failed to create new payment");
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetPaymentsByUser([FromRoute] Guid id) {
            var payments = await paymentService.GetPaymentsByUserAsync(id);
            return payments.Any() ? Ok(payments.Select(x => x.MapFromDomainToDto())) : NotFound("No payment was found for this user");
        }

        [HttpPost("refund/{id}")]
        public async Task<IActionResult> RefundPayment([FromRoute] Guid id) {
            var refundPayments = await paymentService.RefundPaymentAsync(id);
            return refundPayments ? Ok("Payment was refunded successfully") : BadRequest("Failed to refund payment");
        }
    }
}
