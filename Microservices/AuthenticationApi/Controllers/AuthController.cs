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
            return user is not null ? Ok(GenerateJwt(user)) : BadRequest("User email or password is incorrect");
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
    }
}
