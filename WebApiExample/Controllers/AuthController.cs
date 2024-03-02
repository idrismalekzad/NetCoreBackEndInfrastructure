using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiExample.Data.Entities;
using WebApiExample.Services.JWT;

namespace WebApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTService _jWTService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(IJWTService jWTService, UserManager<ApplicationUser> userManager)
        {
            _jWTService = jWTService;
            _userManager = userManager; 
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return BadRequest("Invalid username or password");
            }

            // Authentication successful, generate JWT
            var token = await _jWTService.GenerateToken(user);
            return Ok(new { Token = token , Reslt = "Successfully Authorized"});
        }
    }

    //public class TokenGenerationRequest
    //{
    //    public string UserID { get; set; }
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //}
}
