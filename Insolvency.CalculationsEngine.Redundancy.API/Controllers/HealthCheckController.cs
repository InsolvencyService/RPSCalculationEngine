using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Insolvency.CalculationsEngine.Redundancy.API.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/v1/HealthCheck")]
    public class HealthCheckController : Controller
    {
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(ILogger<HealthCheckController> logger)
        {
            _logger = logger;
        }

        // GET: api/HealthCheck
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Insolvency.CalculationsEngine.Redundancy.API Health checked");

            return new OkObjectResult("Insolvency.CalculationsEngine.Redundancy.API is alive");
        }
    }
}