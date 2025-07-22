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

        [HttpGet("{locationid:int}")]
        public async Task<IActionResult> GetLocation([FromRoute] int locationid)
        { 

            var result = await locationService.GetLocationAsync(locationid);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<GetLocationDto>(result.Data) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("maintenance-centers/{locationid:int}")]
        public async Task<IActionResult> GetLocationMaintenanceCenters([FromRoute] int locationid)
        {   
            var result = await locationService.GetLocationAsync(locationid, location => location.MaintenanceCenters);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message, data = mapper.Map<List<GetMaintenanceCenterDto>>(result.Data.MaintenanceCenters) }) :
                BadRequest(new { message = result.Message });
        }

        [HttpGet("available-vehicles/{locationid:int}")]
        public async Task<IActionResult> GetLocationAvailableVehicles([FromRoute] int locationid)
        { 
            var result = await locationService.GetLocationAsync(locationid, location => location.LocationVehicles);

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

        [HttpPut("{locationid:int}")]
        public async Task<IActionResult> UpdateLocation([FromRoute] int locationid, [FromBody] UpdateLocationDto dto)
        {  
            var result = await locationService.UpdateLocationAsync(locationid, dto);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPut("activate/{locationid:int}")]
        public async Task<IActionResult> ActivateLocation([FromRoute] int locationid)
        {  
            var result = await locationService.ChangeLocationStatusAsync(locationid, true);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }

        [HttpDelete("deactivate/{locationid:int}")]
        public async Task<IActionResult> DeactivateLocation([FromRoute] int locationid)
        {  
            var result = await locationService.ChangeLocationStatusAsync(locationid, false);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message });
        }
    }
}
