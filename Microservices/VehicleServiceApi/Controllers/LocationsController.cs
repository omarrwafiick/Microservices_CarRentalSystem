using Microsoft.AspNetCore.Mvc; 
using VehicleServiceApi.Interfaces;

namespace VehicleServiceApi.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController(ILocationService locationService) : ControllerBase
    {
         
    }
}
