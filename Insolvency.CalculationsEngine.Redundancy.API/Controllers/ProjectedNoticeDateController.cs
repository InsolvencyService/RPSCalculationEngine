using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Errors;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class ProjectedNoticeDateController : Controller
    {
        private readonly IProjectedNoticeDateCalculationService _pndCalculationService;
        private readonly ILogger<ProjectedNoticeDateController> _logger;
        private readonly IOptions<ConfigLookupRoot> _options;

        public ProjectedNoticeDateController(IProjectedNoticeDateCalculationService pndCalculationService,
            ILogger<ProjectedNoticeDateController> logger,
            IOptions<ConfigLookupRoot> options)
        {
            _pndCalculationService = pndCalculationService;
            _logger = logger;
            _options = options;
        }

        [HttpGet]
        [Route("/pinggetpnd")]
        public IActionResult Get()
        {
            return new ContentResult()
            {
                Content = "PingGet response from RPS Api ProjectedNoticeDate",
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        // POST /pnd
        [HttpPost]
        [Route("/pnd")]
        [ProducesResponseType(typeof(ProjectedNoticeDateResponseDTO), 200)]
        [ProducesResponseType(typeof(ModelErrorCollection), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> PostAsync([FromBody] ProjectedNoticeDateCalculationRequestModel data)
        {
            if (data == null)
            {
                ModelState.AddModelError("",
                    "Bad payload provided; unable to parse the request , please review request data and try again");
                _logger.LogError(400, $"Bad payload " +
                                      $"{ModelState}\n Request Data is null and could not be parsed");
                return BadRequest(ModelState);
            }

            var validator = new ProjectedNoticeDateCalculationRequestValidator();
            if (!ModelState.IsValid || !validator.Validate(data).IsValid)
            {
                _logger.LogError((int)HttpStatusCode.BadRequest, $"Request model not valid " +
                                      $"{ModelState}\n Request Data {JsonConvert.SerializeObject(data)} \n Errors : " +
                                      $"{validator.Validate(data).Errors.GetErrorsAsString()} ");
                return BadRequest(ModelState);
            }

            var result = await _pndCalculationService.PerformProjectedNoticeDateCalculationAsync(data, _options);
            _logger.LogInformation((int)HttpStatusCode.OK,
                    $"Calculation performed successfully for the request data provided \n Request Data: {JsonConvert.SerializeObject(data)}");
            return Ok(result);
        }
    }
}