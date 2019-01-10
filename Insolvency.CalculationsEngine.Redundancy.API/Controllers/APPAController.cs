using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Errors;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Insolvency.CalculationsEngine.Redundancy.Common.Exceptions;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class APPAController : Controller
    {
        private readonly IAPPACalculationService _appaService;
        private readonly ILogger<APPAController> _logger;
        private readonly IOptions<ConfigLookupRoot> _options;

        public APPAController(IAPPACalculationService appaService,
            ILogger<APPAController> logger,
            IOptions<ConfigLookupRoot> options)
        {
            _appaService = appaService;
            _logger = logger;
            _options = options;
        }

        [HttpGet]
        [Route("/pinggetappa")]
        public IActionResult Get()
        {
            return new ContentResult()
            {
                Content = "PingGet response from RPS Api APPA",
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        // POST /pa
        [HttpPost]
        [Route("/appa")]
        [ProducesResponseType(typeof(APPACalculationResponseDTO), 200)]
        [ProducesResponseType(typeof(ModelErrorCollection), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> PostAsync([FromBody] APPACalculationRequestModel data)
        {
            try
            {
                if (data == null)
                {
                    ModelState.AddModelError("",
                        "Bad payload provided; unable to parse the request , please review request data and try again");
                    _logger.LogError(400, $"Bad payload " +
                                          $"{ModelState}\n Request Data is null and could not be parsed");
                    return BadRequest(ModelState);
                }

                var validator = new APPACalculationRequestValidator();
                if (!ModelState.IsValid || !validator.Validate(data).IsValid)
                {
                    _logger.LogError((int)HttpStatusCode.BadRequest, $"Request model not valid " +
                                                                      $"{ModelState}\n Request Data {JsonConvert.SerializeObject(data)} \n Errors : " +
                                                                      $"{validator.Validate(data).Errors.GetErrorsAsString()} ");
                    return BadRequest(ModelState);
                }

                var result = await _appaService.PerformCalculationAsync(data, _options);
                _logger.LogInformation((int)HttpStatusCode.OK,
                    $"Calculation performed successfully for the request data provided \n Request Data: {JsonConvert.SerializeObject(data)}");
                return Ok(result);
            }
            catch (MissingConfigurationException exp)
            {
                _logger.LogError((int)HttpStatusCode.BadRequest,
                    $"Calculation was not performed for the request data provided \n Request Data: {JsonConvert.SerializeObject(data)} Bad payload provided; {exp.Message}, please review request data and try again");
                return BadRequest($"Bad payload provided; {exp.Message}, please review request data and try again");
            }
        }
    }
}


