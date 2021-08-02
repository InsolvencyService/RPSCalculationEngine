using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
//using Microsoft.Extensions.Logging.Internal;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.ControllersTests
{
    public class RefundOfNotionalTaxControllerTests : Controller
    {
        private readonly Mock<ILogger<RefundOfNotionalTaxController>> _mockLogger;
        private readonly RefundOfNotionalTaxTestsDataGenerator _refundOfNotionalTaxControllerTestDataGenerator;
        private readonly IOptions<ConfigLookupRoot> _confOptions;
        private readonly Mock<IRefundOfNotionalTaxCalculationService> _mockService;

        public RefundOfNotionalTaxControllerTests()
        {
            _mockLogger = new Mock<ILogger<RefundOfNotionalTaxController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<String>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            _refundOfNotionalTaxControllerTestDataGenerator = new RefundOfNotionalTaxTestsDataGenerator();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
            _mockService = new Mock<IRefundOfNotionalTaxCalculationService>();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var refundOfNotionalTaxController =new RefundOfNotionalTaxController(_mockService.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = refundOfNotionalTaxController.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api RefundOfNotionalTax");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsOkObjectResult_ForValidRequestData()
        {
            // Arrange
            var requestData = _refundOfNotionalTaxControllerTestDataGenerator.GetValidRequestData();

            // Setup
            var responseData = _refundOfNotionalTaxControllerTestDataGenerator.GetValidResponseData();
            _mockService.Setup(s => s.PerformRefundOfNotionalTaxCalculationAsync(requestData, _confOptions))
                .ReturnsAsync(responseData);

            // Act
            var refundOfNotionalTaxController =
                new RefundOfNotionalTaxController(_mockService.Object, _mockLogger.Object, _confOptions);

            // Assert
            var result = await refundOfNotionalTaxController.PostAsync(requestData);
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var statusCode = okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockLogger.Verify(
            m => m.Log<It.IsAnyType>(
                LogLevel.Information,
                It.IsAny<EventId>(),
                (It.IsAnyType)It.Is<object>(v =>
                        v.ToString().Contains("Calculation performed successfully for the request data provided")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsBadRequest_ForNullRequestData()
        {
            // Arrange
            var requestData = _refundOfNotionalTaxControllerTestDataGenerator.GetNullPayload();

            // Act
            var refundOfNotionalTaxController =
                new RefundOfNotionalTaxController(_mockService.Object, _mockLogger.Object,
                    _confOptions);

            // Assert
            var result =
                await refundOfNotionalTaxController.PostAsync(requestData);
            var badRequestObjectRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(
                m => m.Log<It.IsAnyType>(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    (It.IsAnyType)It.Is<object>(v =>
                            v.ToString().Contains("Bad payload")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>())
                );

        }
    }
}