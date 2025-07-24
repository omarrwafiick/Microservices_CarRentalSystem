using AuthenticationApi.Dtos;
using AuthenticationApi.Extensions;
using AuthenticationApi.Interfaces;  
using Microsoft.AspNetCore.Mvc;  

namespace AuthenticationApi.Controllers
{ 
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IUserService userService, ITokenGenerator tokenGenerator) : ControllerBase
    { 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
            var register = await userService.RegisterAsync(registerDto);

            return register.SuccessOrNot ? 
                Ok(new { message = register.Message, id = register.Data }) : 
                BadRequest(new { message = register.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            var result = await userService.LoginAsync(loginDto);
             
            return result.SuccessOrNot ?
                Ok(new { 
                    message = result.Message, 
                    data = result.Data.MapFromDomainToDto(),
                    token = tokenGenerator.GenerateToken(result.Data)
                }) :
                BadRequest(new { message = result.Message });
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var result = await userService.ForgetPasswordAsync(email);

            return result.SuccessOrNot ?
            Ok(new { message = result.Message, path = result.Data }) :
            BadRequest(new { message = result.Message }); 
        }

        [HttpPost("resetpassword/{resetToken}")]
        public async Task<IActionResult> ResetPassword(
            [FromRoute] string resetToken, 
            [FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await userService.ResetPasswordAsync(resetPasswordDto, resetToken);

            return result.SuccessOrNot ?
                Ok(new { message = result.Message }) :
                BadRequest(new { message = result.Message }); 
        } 
    }
}
