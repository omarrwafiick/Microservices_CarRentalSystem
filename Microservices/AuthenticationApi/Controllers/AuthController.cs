using AuthenticationApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IConfiguration configuration): ControllerBase
    {
        //todos
        //service layer with interface
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto) {
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto) {
            return Ok();
        }

        private string GenerateJwt(string email)
        {
            var key = Encoding.UTF8.GetBytes(configuration["AuthSection:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
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
