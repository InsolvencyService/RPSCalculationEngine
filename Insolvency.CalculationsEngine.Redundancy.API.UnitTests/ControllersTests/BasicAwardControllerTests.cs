using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.BasicAward;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
//using Microsoft.Extensions.Logging.Internal;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.ControllersTests
{
    public class BasicAwardControllerTests : Controller
    {
        private readonly Mock<ILogger<BasicAwardController>> _mockLogger;
        private readonly IOptions<ConfigLookupRoot> _confOptions;
        private readonly Mock<IBasicAwardCalculationService> _service;

        public BasicAwardControllerTests()
        {
            _mockLogger = new Mock<ILogger<BasicAwardController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<String>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();

            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
            _service = new Mock<IBasicAwardCalculationService>();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var controller = new BasicAwardController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = controller.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api BasicAward");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds()
        {
            //Arrange
            var request = BasicAwardControllerTestsDataGenerator.GetValidRequest();
            var response = BasicAwardControllerTestsDataGenerator.GetValidResponse();

            _service.Setup(m => m.PerformBasicAwardCalculationAsync(request, _confOptions)).ReturnsAsync(response);

            var controller = new BasicAwardController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(BasicAwardValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAValidationError(BasicAwardCalculationRequestModel request, string expectedErrorMessage)
        {
            //Arrange
            var controller = new BasicAwardController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var badRequestObjectRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);


            _mockLogger.Verify(
               m => m.Log<It.IsAnyType>(
                   LogLevel.Error,
                   It.IsAny<EventId>(),
                   (It.IsAnyType)It.Is<object>(v =>
                           v.ToString().Contains(expectedErrorMessage)),
                   null,
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()));
        }
    }
}