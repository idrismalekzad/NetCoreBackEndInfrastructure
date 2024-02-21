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
        [Authorize]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok("Hello World");
        }   
    }
}
