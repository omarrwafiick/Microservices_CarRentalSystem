using AuthenticationApi.Dtos;
using AuthenticationApi.Extensions;
using AuthenticationApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    //[AuthorizeRoles("RENTER", "OWNER")]
    [Route("api/users")]
    [ApiController] 
    public class UsersController(IUserService userService) : ControllerBase
    {
        //[AuthorizeRoles("ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetUsers() {
            var result = await userService.GetAllUsersAsync();

            return result.SuccessOrNot ?
               Ok(new { message = result.Message, data = result.Data.Select(x => x.MapFromDomainToDto()) }) :
               BadRequest(new { message = result.Message });  
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id) {
            var result = await userService.GetUserByIdAsync(id);

            return result.SuccessOrNot ?
              Ok(new { message = result.Message, data = result.Data.MapFromDomainToDto() }) :
              BadRequest(new { message = result.Message }); 
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto) {
            var result = await userService.UpdateUserAsync(dto);

            return result.SuccessOrNot ?
              Ok(new { message = result.Message }) :
              BadRequest(new { message = result.Message });
        }
    }
}
