using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiExample.Controllers;
using WebApiExample.Data.Entities;

namespace WebApiExample.Services.JWT
{
    public class JWTService : IJWTService
    {
        private const string TokenSecret = "RadinPeymentChannelUserManagementSecurely";
        private readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(8);

        private readonly UserManager<ApplicationUser> _userManager;
        public JWTService(UserManager<ApplicationUser> userManager)
        {
                _userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(TokenSecret);
            var claims = new List<Claim>()
            {
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //new Claim("userid", user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                //Issuer = "https://radin.tech/",
                //Audience = "https://usermangement.channel.com/",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        public async Task<ApplicationUser?> GetUserByID(string userId)
        {
            var rtn = await _userManager.FindByIdAsync(userId);
            if (rtn == null)
            {
                return null;
            }
            return rtn;
        }
    }
}
