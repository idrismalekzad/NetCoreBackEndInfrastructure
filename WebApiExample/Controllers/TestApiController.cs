using BackEndInfrastructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiExample.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TestApiController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Hello World");
        }   
    }
}
