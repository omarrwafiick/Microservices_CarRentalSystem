using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceApi.Dtos; 
using VehicleServiceApi.Interfaces; 
 
namespace VehicleServiceApi.Controllers
{
    [Route("api/vehicles")]
    [ApiController] 
    public class VehiclesController(IVehicleService vehicleService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetVechiles()
        {  
            var result = await vehicleService.GetVehiclesAsync();

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetVechilesByFilter(
            [FromQuery] string vechileType = "", 
            [FromQuery] string fuelType="", 
            [FromQuery] string transmissionType = ""
        )
        { 
            var result = await vehicleService.GetVehiclesByFilterAsync(fuelType, vechileType, transmissionType);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("{vehicleid}")]
        public async Task<IActionResult> GetVechileById([FromRoute] string vehicleid)
        {
            if (Guid.TryParse(vehicleid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await vehicleService.GetVehiclesByConditionAsync(
                vehicle => vehicle.Id == id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("recommendation")]
        public async Task<IActionResult> GetVechilesByRecommendation([FromRoute] string vehicleid, [FromBody] RecommendationDto dto)
        {
            var result = await vehicleService.RecommendRelevantVehiclesAsync(dto); 

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterVehicle([FromBody] CreateVehicleDto dto)
        {
            var result = await vehicleService.RegisterVehicleAsync(dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("status/{vehicleid}")]
        public async Task<IActionResult> UpdateVehicleStatus([FromRoute] string vehicleid, [FromBody] string status)
        {
            if (Guid.TryParse(vehicleid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await vehicleService.UpdateVehicleStatusAsync(id, status);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpDelete("deactivate/{vehicleid}")]
        public async Task<IActionResult> DeactivateVehicleStatus([FromRoute] string vehicleid)
        {
            if (Guid.TryParse(vehicleid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await vehicleService.ChangeVehicleStatusAsync(id, false);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("activate/{vehicleid}")]
        public async Task<IActionResult> ActivateVehicleStatus([FromRoute] string vehicleid)
        {
            if (Guid.TryParse(vehicleid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await vehicleService.ChangeVehicleStatusAsync(id, true);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }
    }
}
