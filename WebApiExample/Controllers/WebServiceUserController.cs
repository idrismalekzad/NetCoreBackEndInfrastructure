using BackEndInfrastructure.DynamicLinqCore;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiExample.DB.Data.Domain;
using WebApiExample.Services;
using WebApiExample.Services.JWT.Middleware;

namespace WebApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Services.JWT.Middleware.Authorize]
    public class WebServiceUserController : ControllerBase
    {
        private readonly WebServiceUserService _webServiceUserService;
        public WebServiceUserController(WebServiceUserService webServiceUserService)
        {
                _webServiceUserService = webServiceUserService;
        }
        /// <summary>
        /// Get all the progrms registered in the system
        /// </summary>
        /// <param name="request">LinqDataRequest</param>
        /// <returns>LinqDataResult</returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<LinqDataResult<WebServiceUser>>> GetAll()
        {
            try
            {
                var request = Request.ToLinqDataHttpGetRequest();
                var res = await _webServiceUserService.ItemsAsync(request);
                return Ok(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
