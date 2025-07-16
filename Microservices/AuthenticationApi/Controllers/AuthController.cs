using AuthenticationApi.Dtos;  
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
            return register ? Ok("New account was created successfully") : BadRequest("Failed to create new account");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            var user = await userService.LoginAsync(loginDto);
            var token = tokenGenerator.GenerateToken(user); 
            return user is not null ? Ok(token) : BadRequest("User email or password is incorrect");
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var resetToken = await userService.ForgetPasswordAsync(email);
            return resetToken is not null ? Ok($"reset-token/{resetToken}") : BadRequest("Failed to verify user");
        }

        [HttpPost("resetpassword/{resetToken}")]
        public async Task<IActionResult> ResetPassword(
            [FromRoute] string resetToken, 
            [FromBody] LoginDto loginDto)
        {
            var result = await userService.ResetPasswordAsync(loginDto, resetToken);
            return result ? Ok("Password was reset successfully") : BadRequest("Failed to reset Password");
        } 
    }
}
