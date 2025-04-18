using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        //[HttpGet("{id}")]
        //public IActionResult GetUserById(Guid id) { } 

        [HttpGet]
        public IActionResult Get() {
            return Ok("working");
        } 

        //[HttpPut("{id}")]
        //public IActionResult UpdateUser(Guid id, [FromBody] UserDto userDto) { }
    }
}
