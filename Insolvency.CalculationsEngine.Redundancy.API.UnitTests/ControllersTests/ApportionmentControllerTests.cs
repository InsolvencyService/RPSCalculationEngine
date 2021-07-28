using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment;
//using Microsoft.Extensions.Logging.Internal;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.ControllersTests
{
    public class ApportionmentControllerTests : Controller
    {
        private readonly Mock<ILogger<ApportionmentController>> _mockLogger;
        private readonly IOptions<ConfigLookupRoot> _confOptions;
        private readonly Mock<IApportionmentCalculationService> _service;

        public ApportionmentControllerTests()
        {
            _mockLogger = new Mock<ILogger<ApportionmentController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<String>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();

            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
            _service = new Mock<IApportionmentCalculationService>();
        }
        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var controller = new ApportionmentController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = controller.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api Apportionment");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds()
        {
            //Arrange
            var request = ApportionmentControllerTestsDataGenerator.GetValidRequestWithDataExampleOne();
            var response = ApportionmentControllerTestsDataGenerator.GetValidResponseForExampleOne();

            _service.Setup(m => m.PerformApportionmentCalculationAsync(request, _confOptions)).ReturnsAsync(response);

            var controller = new ApportionmentController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(ApportionmentValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAValidationError(ApportionmentCalculationRequestModel request, string expectedErrorMessage)
        {
            //Arrange
            var controller = new ApportionmentController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var badRequestObjectRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            //_mockLogger.Verify(x => x.Log(
            //    LogLevel.Error,
            //    It.IsAny<EventId>(),
            //    It.Is<object>(v => v.ToString().Contains(expectedErrorMessage)),
            //    null,
            //    It.IsAny<Func<object, Exception, string>>()
            //));

            _mockLogger.Verify(
               m => m.Log<It.IsAnyType>(
                   LogLevel.Error,
                   It.IsAny<EventId>(),
                   (It.IsAnyType)It.Is<object>(v =>
                           v.ToString().Contains(expectedErrorMessage)),
                   null,
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()),
               Times.Once);
        }

    }
}
