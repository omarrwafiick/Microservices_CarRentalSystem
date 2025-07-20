using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Interfaces; 

namespace VehicleServiceApi.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController(ILocationService locationService, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetLocations([FromQuery] int skip = 0, [FromQuery] int take = 0)
        {  
            var result = await locationService.GetLocationsAsync(); 

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetLocationDto>>(result.Data.Take(take).Skip(skip)) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("{locationid}")]
        public async Task<IActionResult> GetLocation([FromRoute] string locationid)
        {
            if (Guid.TryParse(locationid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await locationService.GetLocationAsync(id);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<GetLocationDto>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("maintenance-centers/{locationid}")]
        public async Task<IActionResult> GetLocationMaintenanceCenters([FromRoute] string locationid)
        {
            if (Guid.TryParse(locationid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await locationService.GetLocationAsync(id, location => location.MaintenanceCenters);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetMaintenanceCenterDto>>(result.Data.MaintenanceCenters) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("available-vehicles/{locationid}")]
        public async Task<IActionResult> GetLocationAvailableVehicles([FromRoute] string locationid)
        {
            if (Guid.TryParse(locationid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await locationService.GetLocationAsync(id, location => location.LocationVehicles);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetVehicleDto>>(result.Data.LocationVehicles) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> AddNewLocation([FromBody] CreateLocationDto dto)
        {    
            var result = await locationService.AddLocationAsync(dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("{locationid}")]
        public async Task<IActionResult> UpdateLocation([FromRoute] string locationid, [FromBody] UpdateLocationDto dto)
        {
            if (Guid.TryParse(locationid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await locationService.UpdateLocationAsync(id, dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("activate/{locationid}")]
        public async Task<IActionResult> ActivateLocation([FromRoute] string locationid)
        {
            if (Guid.TryParse(locationid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await locationService.ChangeLocationStatusAsync(id, true);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpDelete("deactivate/{locationid}")]
        public async Task<IActionResult> DeactivateLocation([FromRoute] string locationid)
        {
            if (Guid.TryParse(locationid, out Guid id))
                return BadRequest(new { message = "Invalid id" });

            var result = await locationService.ChangeLocationStatusAsync(id, false);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }
    }
}
