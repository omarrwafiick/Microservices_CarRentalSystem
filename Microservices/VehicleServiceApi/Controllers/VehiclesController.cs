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
            var result = await vehicleService.GetVehiclesAsync(
                vehicle => vehicle.Model, vehicle => vehicle.VehicleImages, vehicle => vehicle.Location);

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
            var result = await vehicleService.GetVehiclesByFilterAsync(
                vehicle => vehicle.Model, vehicle => vehicle.VehicleImages,
                vehicle => vehicle.Location, fuelType, vechileType, transmissionType);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("{vehicleid:int}")]
        public async Task<IActionResult> GetVechileById([FromRoute] int vehicleid)
        {  
            var result = await vehicleService.GetVehiclesByConditionAsync(
                vehicle => vehicle.Model, vehicle => vehicle.VehicleImages,
                vehicle => vehicle.Location, vehicle => vehicle.Id == vehicleid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("recommendation")]
        public async Task<IActionResult> GetVechilesByRecommendation([FromBody] RecommendationDto dto)
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

        [HttpPut("status/{vehicleid:int}")]
        public async Task<IActionResult> UpdateVehicleStatus([FromRoute] int vehicleid, [FromBody] string status)
        {  
            var result = await vehicleService.UpdateVehicleStatusAsync(vehicleid, status);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpDelete("deactivate/{vehicleid:int}")]
        public async Task<IActionResult> DeactivateVehicleStatus([FromRoute] int vehicleid)
        {  
            var result = await vehicleService.ChangeVehicleStatusAsync(vehicleid, false);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("activate/{vehicleid:int}")]
        public async Task<IActionResult> ActivateVehicleStatus([FromRoute] int vehicleid)
        { 
            var result = await vehicleService.ChangeVehicleStatusAsync(vehicleid, true);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }
    }
}
