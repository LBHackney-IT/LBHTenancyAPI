using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.HealthChecks;

namespace LBHTenancyAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    public class HealthcheckController : Controller
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthcheckController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }
        
        [HttpGet]
        [Route("/healthcheck")]
        public IActionResult Healthcheck()
        {
            return Ok(new Dictionary<string, object> {{"success", true}});
        }

        [HttpGet]
        [Route("/healthcheck/detailed")]
        public async Task<IActionResult> DetailedHealthCheck()
        {
            var healthCheckResult = await _healthCheckService.CheckHealthAsync(Request.GetCancellationToken()).ConfigureAwait(false);
            return Ok(healthCheckResult);
        }
    }
}
