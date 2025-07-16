using Microsoft.AspNetCore.Mvc;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Interfaces;
 
namespace VehicleServiceApi.Controllers
{
    [Route("api/vehicles")]
    [ApiController] 
    public class VehiclesController(IVehicleService vehicleService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVechiles()
        {
            return Ok();
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetVechilesByFilter(
            [FromQuery] string? vechileType, [FromQuery] string? fuelType, [FromQuery] string? transmissionType
        )
        {
            return Ok();
        }

        [HttpGet("{vehicleid}")]
        public async Task<IActionResult> GetVechileById([FromRoute] string vehicleid)
        {
            return Ok();
        }

        [HttpGet("recommendation")]
        public async Task<IActionResult> GetVechilesByRecommendation([FromRoute] string vehicleid)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterVehicle([FromBody] CreateVehicleDto dto)
        {
            return Ok();
        }

        [HttpPut("{vehicleid}")]
        public async Task<IActionResult> UpdateVehicle([FromRoute] string vehicleid, [FromBody] UpdateVehicleStatusDto dto)
        {
            return Ok();
        }

        [HttpPut("status/{vehicleid}")]
        public async Task<IActionResult> UpdateVehicleStatus([FromRoute] string vehicleid, [FromBody] UpdateVehicleDto dto)
        {
            return Ok();
        }
    }
}
