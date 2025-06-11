using AuthenticationApi.Dtos;  
using AuthenticationApi.Interfaces;
using AuthenticationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Controllers
{ 
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IConfiguration configuration, IUserService userService) : ControllerBase
    { 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
            var register = await userService.RegisterAsync(registerDto);
            return register ? Ok("New account was created successfully") : BadRequest("Failed to create new account");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            var user = await userService.LoginAsync(loginDto);
            var token = GenerateJwt(user);
            Response.Cookies.Append("accessToken",token);
            return user is not null ? Ok(token) : BadRequest("User email or password is incorrect");
        }

        private string GenerateJwt(User user)
        {
            var key = Encoding.UTF8.GetBytes(configuration["AuthSection:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()!)
            };
            var token = new JwtSecurityToken(
                issuer: configuration["AuthSection:Issuer"],
                audience: configuration["AuthSection:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var result = await userService.ForgetPasswordAsync(email);
            return result ? Ok("User was verified") : BadRequest("Failed to verify user");
        }

        [HttpPost("resetpassword/{resetToken}")]
        public async Task<IActionResult> ResetPassword(
            [FromRoute] string resetToken, 
            [FromBody] LoginDto loginDto)
        {
            var result = await userService.ResetPasswordAsync(loginDto, resetToken);
            return result ? Ok("Password was reset successfully") : BadRequest("Failed to reset Password");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody] LoginDto loginDto)
        {
            Response.Cookies.Delete("accessToken");
            return Ok("User was logged out successfully");
        }
    }
}
