using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string TokenSecret = "RadinPeymentChannelUserManagementSecurely";
        private readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(8);

        [HttpPost]
        public async Task<ActionResult<string>> GenerateToken([FromBody] TokenGenerationRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSecret));

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, request.Email),
                new(JwtRegisteredClaimNames.Email, request.Email),
                new("userid", request.UserID.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                Issuer = "https://radin.tech/",
                Audience = "https://usermangement.channel.com/",
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return Ok(jwt);
        }
    }

    public class TokenGenerationRequest
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
