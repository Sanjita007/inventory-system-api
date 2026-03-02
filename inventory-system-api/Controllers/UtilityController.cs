using inventory_system_api;
using inventory_system_api.Controllers;
using inventory_system_api.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace accswift_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UtilityController : BaseController
    {
        [HttpPost("ToXML")]
        public IActionResult ToXML(object json) {
            int id = 0;
            var res = 1 / id;
            return Ok(json.ToXml("SALESINVOICEDETAILS"));
        }

    }
}
