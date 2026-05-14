using inventory_system_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace inventory_system_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize]/-+8
    public class BaseController : ControllerBase
    {
        private IActionResult CustomResponse(int statusode, string message, object result)
        {
            var response = new Response { StatusCode = statusode, Message = message, Data = result };

            return Ok(response);
        }

        protected IActionResult OkResponse()
        {
            return CustomResponse(200, "Success", null);
        } 
        protected IActionResult OkResponse(object result)
        {
            return CustomResponse(200, "Success", result);
        }

        protected IActionResult ErrorResponse(string errorMessage) => CustomResponse(500, errorMessage, null);
    }
}
