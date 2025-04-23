using AuthenticationApi.Dtos;
using AuthenticationApi.Extensions;
using AuthenticationApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers() {
            var users = await userService.GetAllUsersAsync();
            return users.Any() ? Ok(users.Select(x=>x.MapFromDomainToDto())) : NotFound("No user was found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id) {
            var user = await userService.GetUserByIdAsync(id);
            return user is not null ? Ok(user.MapFromDomainToDto()) : NotFound("No user was found");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto) {
            var user = await userService.UpdateUserAsync(dto);
            return user ? Ok("User was updated successfully") : BadRequest("User couldn't be updated");
        }
    }
}
