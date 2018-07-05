using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    public class TenanciesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = new Dictionary<string, string> {{"foo", "bar"}};

            return Ok(result);
        }
    }
}
