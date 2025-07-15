using Microsoft.AspNetCore.Mvc;  
using PaymentServiceApi.Interfaces;

namespace PaymentService.Controllers
{
    [Route("api/paymentrecords")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    { 
    }
}
