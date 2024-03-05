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
        private readonly Microsoft.Extensions.Hosting.IHostingEnvironment _hostingEnvironment;
        public AuthController(IJWTService jWTService, UserManager<ApplicationUser> userManager, Microsoft.Extensions.Hosting.IHostingEnvironment configuration)
        {
            _jWTService = jWTService;
            _userManager = userManager;
            _hostingEnvironment = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Unauthorized("Invalid username or password");
                }

                // Authentication successful, generate JWT
                var token = await _jWTService.GenerateToken(user);
                return Ok(new { Token = token, Reslt = "Successfully Authorized" });
            }
            else
            {
                if (request.Email != "admin@gmail.com" && request.Password != "Admin123*")
                {
                    return Unauthorized("Invalid username or password");
                }
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Email = request.Email;
                applicationUser.Id = "27d131eb-0da6-4d8e-a70d-8ba9d8536810";
                // Authentication successful, generate JWT
                var token = await _jWTService.GenerateToken(applicationUser);
                return Ok(new { Token = token, Reslt = "Successfully Authorized" });
            }

        }
    }

    //public class TokenGenerationRequest
    //{
    //    public string UserID { get; set; }
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //}
}
