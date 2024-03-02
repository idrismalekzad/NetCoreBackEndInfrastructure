using BackEndInfrastructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiExample.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class TestApiController : BaseApiController
    {
        [HttpGet]
        [WebApiExample.Services.JWT.Middleware.Authorize]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok("Rerurn Data Successfully");
        }   
    }
}
