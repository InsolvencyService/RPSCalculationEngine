using System;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
//using Microsoft.Extensions.Logging.Internal;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.ControllersTests
{
    public class RedundancyPaymentControllerTests : Controller
    {
        private readonly Mock<ILogger<RedundancyPaymentController>> _mockLogger;
        private readonly RedundancyPaymentTestsDataGenerator _redundancyPaymentControllerTestDataGenerator;
        private readonly IOptions<ConfigLookupRoot> _confOptions;
        private readonly Mock<IRedundanyPayCalculationsService> _mockService;

        public RedundancyPaymentControllerTests()
        {
            _mockLogger = new Mock<ILogger<RedundancyPaymentController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<String>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            _redundancyPaymentControllerTestDataGenerator = new RedundancyPaymentTestsDataGenerator();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();

            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
            _mockService = new Mock<IRedundanyPayCalculationsService>();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var redundancyPayController = new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = redundancyPayController.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api RedundancyPay");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Returns_RedundancyPaymentCalculationResponseDTO()
        {
            //Arrange
            var requestData = _redundancyPaymentControllerTestDataGenerator.GetValidRequestData();

            //Setup
            var responseData = _redundancyPaymentControllerTestDataGenerator.GetValidCalculationResults();
            _mockService.Setup(s => s.PerformRedundancyPayCalculationAsync(requestData, _confOptions))
                .ReturnsAsync(responseData);

            //Act
            var redundancyPayController =
                new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);

            //Assert
            var result =
                await redundancyPayController.PostAsync(requestData);
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            var responseDto = okObjectResult.Value.Should()
                .BeOfType<RedundancyPaymentResponseDto>().Subject;

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Calculation performed successfully for the request data provided")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                    );

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Returns_NotEligibleRedundancyPaymentCalculationResponse()
        {
            //Arrange
            var requestData = new RedundancyPaymentCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2015, 10, 11),
                DismissalDate = new DateTime(2016, 08, 05),
                DateNoticeGiven = new DateTime(2016, 08, 05),
                DateOfBirth = new DateTime(1995, 03, 11),
                WeeklyWage = 203.64m,
                EmployerPartPayment = 0,
                EmploymentBreaks = 0
            };

            //Act
            var redundancyPayController =
                new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);

            //Assert
            var result = await redundancyPayController.PostAsync(requestData);
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Years of service cannot be less than 2 years")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                    );
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsBadRequest_ForInvalidRequestData()
        {
            //Arrange
            var requestData = _redundancyPaymentControllerTestDataGenerator.GetInvalidRequestData();

            //Act
            var redundancyPayController =
                new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);
            //Assert
            var result = await redundancyPayController.PostAsync(requestData);
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Request model not valid")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                );
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsBadRequest_ForInvalidWeeklyWage()
        {
            //Arrange
            var requestData = _redundancyPaymentControllerTestDataGenerator.GetInvalidRequestData_WeeklyWage_Zero();

            //Act
            var redundancyPayController =
                new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);
            //Assert
            var result = await redundancyPayController.PostAsync(requestData);
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Weekly wage is invalid; value must be greater than zero")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                );

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsBadRequest_ForNullData()
        {
            //Arrange
            var requestData = _redundancyPaymentControllerTestDataGenerator.GetNullData();

            //Act
            var redundancyPayController =
                new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);
            //Assert
            var result = await redundancyPayController.PostAsync(requestData);
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Request Data is null and could not be parsed")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                );
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Fails_IfClaimReceptDateIsNotPresent()
        {
            //Arrange
            var requestData = new RedundancyPaymentCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2015, 10, 11),
                DismissalDate = new DateTime(2016, 08, 05),
                DateNoticeGiven = new DateTime(2016, 08, 05),
                DateOfBirth = new DateTime(1995, 03, 11),
                WeeklyWage = 203.64m,
                EmployerPartPayment = 0,
                EmploymentBreaks = 0
            };

            //Act
            var redundancyPayController =
                new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);

            //Assert
            var result = await redundancyPayController.PostAsync(requestData);
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Claim Receipt Date is not provided or it is an invalid date")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                    );

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Fails_IfClaimReceptDateIsAfterDismissalDatePlus6Months()
        {
            //Arrange
            var requestData = new RedundancyPaymentCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2015, 10, 11),
                DismissalDate = new DateTime(2016, 08, 05),
                DateNoticeGiven = new DateTime(2016, 08, 05),
                ClaimReceiptDate = new DateTime(2017, 2, 06),
                DateOfBirth = new DateTime(1995, 03, 11),
                WeeklyWage = 203.64m,
                EmployerPartPayment = 0,
                EmploymentBreaks = 0
            };

            //Act
            var redundancyPayController =
                new RedundancyPaymentController(_mockService.Object, _mockLogger.Object, _confOptions);

            //Assert
            var result = await redundancyPayController.PostAsync(requestData);
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Claim Receipt Date must be within 6 months of the dismissal date")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                     );
        }

    }
}