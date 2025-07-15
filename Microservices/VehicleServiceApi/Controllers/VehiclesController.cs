using Microsoft.AspNetCore.Mvc; 
using VehicleServiceApi.Interfaces;
 
namespace VehicleServiceApi.Controllers
{
    [Route("api/vehicles")]
    [ApiController] 
    public class VehiclesController(IVehicleService vehicleService) : ControllerBase
    { 

    }
}
