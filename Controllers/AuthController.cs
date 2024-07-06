using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Jwtsettings _jwtSettings;
        public AuthController(Jwtsettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }
        [HttpPost("token")]

        public IActionResult GenerateToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds =  new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
