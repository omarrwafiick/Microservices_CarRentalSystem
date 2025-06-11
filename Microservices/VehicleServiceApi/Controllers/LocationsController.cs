using Microsoft.AspNetCore.Mvc;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Interfaces;

namespace VehicleServiceApi.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController(ILocationService locationService) : ControllerBase
    {

        [HttpGet("/location/{id}")]
        public async Task<IActionResult> GetLocation([FromRoute] Guid id)
        {
            var location = await locationService.GetLocationAsync(id);
            return location is not null ?
                Ok(new
                {
                    City = location.City,
                    District = location.District,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                }) :
                BadRequest("Failed to get location");
        }

        [HttpPost("/location")]
        public async Task<IActionResult> AddLocation([FromBody] LocationDto dto)
        {
            var createLocation = await locationService.CreateLocationAsync(dto);
            return createLocation ? Ok("Location was created successfully") : BadRequest("Failed to create Location");
        }

        [HttpDelete("/location/{id}")]
        public async Task<IActionResult> DeleteLocation([FromRoute] Guid id)
        {
            var deleteLocation = await locationService.DeleteLocationAsync(id);
            return deleteLocation ? Ok("Location was deleted successfully") : BadRequest("Failed to delete location");
        }
    }
}
