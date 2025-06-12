using Microsoft.AspNetCore.Mvc;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Extensions;
using VehicleServiceApi.Interfaces;
 
namespace VehicleServiceApi.Controllers
{
    [Route("api/vehicles")]
    [ApiController] 
    public class VehiclesController(IVehicleService vehicleService) : ControllerBase
    {
        [HttpGet] 
        public async Task<IActionResult> GetAllVehicles() {
            var vehicles = await vehicleService.GetAllVehiclesAsync();
            return vehicles.Any() ? Ok(vehicles.Select(x=>x.MapFromDomainToDto())) : NotFound("No vehicle was found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById([FromRoute] Guid id) {
            var vehicle = await vehicleService.GetVehicleByIdAsync(id);
            return vehicle is not null ? Ok(vehicle.MapFromDomainToDto()) : NotFound("No vehicle was found");
        }

        [HttpGet("recommendation")]
        public async Task<IActionResult> RecommendRelevantVehicles([FromQuery] RecommendationDto data)
        {
            var referrerHeader = HttpContext.Request.Headers["Referrer"].ToString();
            var authTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var vehicles = await vehicleService.RecommendRelevantVehiclesAsync(data, authTokenHeader, referrerHeader);
            return vehicles.Any() ? Ok(vehicles.Select(x => x.MapFromDomainToDto())) : NotFound("No vehicle was found");
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableVehicles() {
            var vehicles = await vehicleService.GetAvailableVehiclesAsync();
            return vehicles.Any() ? Ok(vehicles.Select(x => x.MapFromDomainToDto())) : NotFound("No vehicle is available at the moment");
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleDto dto) {
            var newVehicle = await vehicleService.CreateVehicleAsync(dto.CreateMapFromDtoToDomain());
            return newVehicle ? Ok("New vehicle was created successfully") : BadRequest("Failed to create new vehicle");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle([FromRoute] Guid id, [FromBody] UpdateVehicleDto dto) {
            var vehicle = await vehicleService.GetVehicleByIdAsync(id);
            if (vehicle is null) return NotFound("No vehicle was found");
            var updateVehicle = await vehicleService.UpdateVehicleAsync(dto.UpdateMapFromDtoToDomain(vehicle));
            return updateVehicle ? Ok("Vehicle was updated successfully") : BadRequest("Failed to update vehicle");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id) {
            var deleteVehicle = await vehicleService.DeleteVehicleAsync(id);
            return deleteVehicle ? Ok("Vehicle was deleted successfully") : BadRequest("Failed to delete vehicle");
        }

    }
}
