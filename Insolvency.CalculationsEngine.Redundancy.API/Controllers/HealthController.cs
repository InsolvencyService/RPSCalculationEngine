using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HealthController : Controller
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Ping()
        {
            return new OkResult();
        }


        [HttpGet]
        [Route("~/api/[controller]")]
        [ProducesResponseType(typeof(HealthStatus), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> Get()
        {
            var status = await _healthCheckService.CheckHealthAsync();

            return new OkObjectResult(Enum.GetName(typeof(HealthStatus), status.Status));
        }
    }
}
