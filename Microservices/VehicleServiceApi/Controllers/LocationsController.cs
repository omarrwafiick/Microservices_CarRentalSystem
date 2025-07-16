using Microsoft.AspNetCore.Mvc;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Interfaces;

namespace VehicleServiceApi.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController(ILocationService locationService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetLocations([FromQuery] int skip = 0, [FromQuery] int take = 0)
        {
            return Ok();
        }

        [HttpGet("{locationid}")]
        public async Task<IActionResult> GetLocation([FromRoute] string locationid)
        {
            return Ok();
        }

        [HttpGet("maintenance-centers/{locationid}")]
        public async Task<IActionResult> GetLocationMaintenanceCenters([FromRoute] string locationid)
        {
            return Ok();
        }

        [HttpGet("available-vehicles/{locationid}")]
        public async Task<IActionResult> GetLocationAvailableVehicles([FromRoute] string locationid)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewLocation([FromBody] CreateLocationDto dto)
        {
            return Ok();
        }

        [HttpPut("{locationid}")]
        public async Task<IActionResult> UpdateLocation([FromRoute] string locationid, [FromBody] UpdateLocationDto dto)
        {
            return Ok();
        }

        [HttpPut("activate/{locationid}")]
        public async Task<IActionResult> ActivateLocation([FromRoute] string locationid)
        {
            return Ok();
        }

        [HttpDelete("deactivate/{locationid}")]
        public async Task<IActionResult> DeactivateLocation([FromRoute] string locationid)
        {
            return Ok();
        }
    }
}
