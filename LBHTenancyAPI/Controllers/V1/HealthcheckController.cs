using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.HealthChecks;

namespace LBHTenancyAPI.Controllers.V1
{
    /// <summary>
    /// Health Check controller to assist with checking API is alive and potentially connect to dependencies
    /// </summary>
    [ApiVersion("1")]
    [Produces("application/json")]
    public class HealthcheckController : Controller
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthcheckController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Simple Health Check
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/healthcheck")]
        public IActionResult Healthcheck()
        {
            return Ok(new Dictionary<string, object> {{"success", true}});
        }

        /// <summary>
        /// Sophisticated Health Check - Can check things like can connect to dependencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/healthcheck/status")]
        public async Task<IActionResult> StatusHealthCheck()
        {
            var healthCheckResult = await _healthCheckService.CheckHealthAsync(Request.GetCancellationToken()).ConfigureAwait(false);
            return Ok(healthCheckResult);
        }
    }
}
