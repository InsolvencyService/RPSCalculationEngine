using System.Linq;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators;
using Insolvency.CalculationsEngine.Redundancy.Common.Exceptions;
using Newtonsoft.Json;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class ApportionmentController : Controller
    {
        private readonly IApportionmentCalculationService _apportionmentCalculationService;
        private readonly ILogger<ApportionmentController> _logger;
        private readonly IOptions<ConfigLookupRoot> _options;
        public ApportionmentController(IApportionmentCalculationService apportionmentCalculationService, ILogger<ApportionmentController> logger, IOptions<ConfigLookupRoot> options)
        {
            _apportionmentCalculationService = apportionmentCalculationService;
            _logger = logger;
            _options = options;
        }

        [HttpGet]
        [Route("/pinggetApportionment")]
        public IActionResult Get()
        {
            return new ContentResult()
            {
                Content = "PingGet response from RPS Api Apportionment",
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        // POST /apportionment
        [HttpPost]
        [Route("/apportionment")]
        [ProducesResponseType(typeof(ApportionmentCalculationResponseDTO), 200)]
        [ProducesResponseType(typeof(ModelErrorCollection), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> PostAsync([FromBody] ApportionmentCalculationRequestModel data)
        {
            try
            {
                if (data == null)
                {
                    ModelState.AddModelError("",
                        "Bad payload provided; unable to parse the request , please review request data and try again");
                    _logger.LogError((int)HttpStatusCode.BadRequest, $"Bad payload " +
                                          $"{ModelState}\n Request Data is null and could not be parsed");
                    return BadRequest(ModelState);
                }
                var validator = new ApportionmentCalculationRequestValidator();
                if (!ModelState.IsValid || !validator.Validate(data).IsValid)
                {
                    _logger.LogError((int)HttpStatusCode.BadRequest, $"Request model not valid " +
                                                                     $"{ModelState}\n Request Data {JsonConvert.SerializeObject(data)} \n Errors : " +
                                                                     $"{validator.Validate(data).Errors.GetErrorsAsString()} ");
                    return BadRequest(ModelState);
                }
                var result = await _apportionmentCalculationService.PerformApportionmentCalculationAsync(data, _options);
                return Ok(result);
            }
            catch (MissingConfigurationException exp)
            {
                _logger.LogError((int)HttpStatusCode.BadRequest,
                    $"Apportionment Calculation was not performed for the request data provided \n Request Data: {JsonConvert.SerializeObject(data)} Bad payload provided; {exp.Message}, please review request data and try again");
                return BadRequest($"Bad payload provided; {exp.Message}, please review request data and try again");
            }
        }
    }
}
