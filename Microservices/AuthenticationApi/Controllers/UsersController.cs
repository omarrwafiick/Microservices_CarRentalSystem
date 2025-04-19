using AuthenticationApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers() {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id) {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDto dto) {
            return Ok();
        }
    }
}
