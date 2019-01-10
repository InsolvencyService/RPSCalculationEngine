using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.ControllersTests
{
    public class ProjectedNoticeDateControllerTests : Controller
    {
        private readonly Mock<ILogger<ProjectedNoticeDateController>> _mockLogger;
        private readonly IOptions<ConfigLookupRoot> _confOptions;
        private readonly Mock<IProjectedNoticeDateCalculationService> _service;

        public ProjectedNoticeDateControllerTests()
        {
            _mockLogger = new Mock<ILogger<ProjectedNoticeDateController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();

            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
            _service = new Mock<IProjectedNoticeDateCalculationService>();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var projectedNoticeDateController = new ProjectedNoticeDateController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = projectedNoticeDateController.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api ProjectedNoticeDate");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds()
        {
            //Arrange
            var requestData = ProjectedNoticeDateTestsDataGenerator.GetValidRequestData();
            var response = ProjectedNoticeDateTestsDataGenerator.GetValidResponseData();

            _service.Setup(m => m.PerformProjectedNoticeDateCalculationAsync(requestData, _confOptions)).ReturnsAsync(response);
            var projectedNoticeDateController = new ProjectedNoticeDateController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await projectedNoticeDateController.PostAsync(requestData);

            //Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            var responseDto = okObjectResult.Value.Should().BeOfType<ProjectedNoticeDateResponseDTO>().Subject;
            responseDto.ProjectedNoticeDate.Should().Be(new DateTime(2018, 01, 08));

            _mockLogger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<object>(v =>
                    v.ToString().Contains("Calculation performed successfully for the request data provided")),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsBadRequest_WhenRequestData_IsNull()
        {
            //Arrange
            var projectedNoticeDateController = new ProjectedNoticeDateController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await projectedNoticeDateController.PostAsync(null);

            //Assert
            var badRequestObjectRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);
            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<object>(v =>
                    v.ToString().Contains("Bad payload")),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsBadRequest_ForInvalidRequestData_MissingEmploymentStartDate()
        {
            //Arrange
            var requestData = ProjectedNoticeDateTestsDataGenerator.GetInvalidRequestData_InvalidEmployeeStartDate();
            var projectedNoticeDateController = new ProjectedNoticeDateController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await projectedNoticeDateController.PostAsync(requestData);

            //Assert
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);
            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<object>(v => v.ToString().Contains("'Employment Start Date' is not provided or it is an invalid date")),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsBadRequest_ForInvalidRequestData_DismissalDateBeforeEmploymentStartDate()
        {
            //Arrange
            var requestData = ProjectedNoticeDateTestsDataGenerator.GetInvalidRequestData_DismissalDataBeforeEmployeeStartDate();
            var projectedNoticeDateController = new ProjectedNoticeDateController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await projectedNoticeDateController.PostAsync(requestData);

            //Assert
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var statusCode = badRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);
            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<object>(v => v.ToString().Contains("'Dismissal Date' can not be before the Employment Start Date")),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
        }
    }
}