using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    public class HealthcheckController : Controller
    {
        [Route("/healthcheck")]
        [HttpGet]
        public IActionResult Healthcheck()
        {
            return Ok(new Dictionary<string, object> {{"success", true}});
        }
    }
}
