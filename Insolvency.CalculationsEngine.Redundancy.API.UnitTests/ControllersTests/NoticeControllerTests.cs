using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.ControllersTests
{
    public class NoticeControllerTests : Controller
    {
        private readonly Mock<INoticeCalculationService> _service;
        private readonly Mock<ILogger<NoticeController>> _mockLogger;
        private readonly IOptions<ConfigLookupRoot> _confOptions;

        public NoticeControllerTests()
        {
            _service = new Mock<INoticeCalculationService>();

            _mockLogger = new Mock<ILogger<NoticeController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();

            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = controller.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api Notice Pay");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds()
        {
            //Arrange
            var request = NoticeControllerTestsDataGenerator.GetValidRequestData();
            var response = NoticeControllerTestsDataGenerator.GetValidResponseData();

            _service.Setup(m => m.PerformNoticePayCompositeCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }



        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(NoticeValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAValidationError(NoticePayCompositeCalculationRequestModel request, string expectedErrorMessage)
        {
            //Arrange
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var badRequestObjectRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<object>(v => v.ToString().Contains(expectedErrorMessage)),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(NoticeWorkedNotPaidValidationTestDataHelper))]
        public async Task PostAsync_ReturnsBadRequest_When_RequestData_HaveInvalidNWNPData(NoticeWorkedNotPaidCalculationRequestModel request, string expectedErrorMessage)
        {
            //Arrange
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);
            var data = new NoticePayCompositeCalculationRequestModel()
            {
                Cnp = null,
                Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>() { request }
            };

            //Act
            var result = await controller.PostAsync(data);

            //Assert
            var badRequestObjectRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);

            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<object>(v =>
                    v.ToString().Contains(expectedErrorMessage)),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(CompensatoryNoticePayValidationTestDataHelper))]
        public async Task PostAsync_ReturnsBadRequest_When_RequestData_HaveInvalidCNPData(CompensatoryNoticePayCalculationRequestModel request, string expectedErrorMessage)
        {
            //Arrange
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);
            var data = new NoticePayCompositeCalculationRequestModel()
            {
                Cnp = request,
                Nwnp = null 
            };

            //Act

            var result = await controller.PostAsync(data);

            //Assert
            var badRequestObjectRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.BadRequest);
            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<object>(v =>
                    v.ToString().Contains(expectedErrorMessage)),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
            
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsOkResult_When_RequestData_HasEitherCNPandNWNPData()
        {
            //Arrange
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);
            var data = new NoticePayCompositeCalculationRequestModel() { Cnp = CompensatoryNoticePayControllerTestsDataGenerator.GetValidRequest(), Nwnp = null };
            var response = new NoticePayCompositeCalculationResponseDTO();
            _service.Setup(s => s.PerformNoticePayCompositeCalculationAsync(data, _confOptions))
                .ReturnsAsync(It.IsAny< NoticePayCompositeCalculationResponseDTO>());

            //Act

            var result = await controller.PostAsync(data);

            //Assert
            var okRequestObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            _mockLogger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<object>(v =>
                    v.ToString().Contains("Calculation performed successfully for the request data provided")),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
            okRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_ReturnsOkResult_When_RequestData_HasBothCNPandNWNPData()
        {
            //Arrange
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);
            var data = new NoticePayCompositeCalculationRequestModel()
                        { Cnp = CompensatoryNoticePayControllerTestsDataGenerator.GetValidRequest(),
                        Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>()
                        {
                            NoticeWorkedNotPaidControllerTestsDataGenerator.GetValidRP1Request(),
                            NoticeWorkedNotPaidControllerTestsDataGenerator.GetValidRP14aRequest()
                        }
            };
            var response = new NoticePayCompositeCalculationResponseDTO();
            _service.Setup(s => s.PerformNoticePayCompositeCalculationAsync(data, _confOptions))
                .ReturnsAsync(It.IsAny<NoticePayCompositeCalculationResponseDTO>());

            //Act

            var result = await controller.PostAsync(data);

            //Assert
            var okRequestObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            _mockLogger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<object>(v =>
                    v.ToString().Contains("Calculation performed successfully for the request data provided")),
                null,
                It.IsAny<Func<object, Exception, string>>()
            ));
            okRequestObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_SucceedsWithOverlappingPeriodsInDifferentInputSources()
        {
            //Arrange
            var request = new NoticePayCompositeCalculationRequestModel()
            {
                Cnp = null,
                Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>()
                    {
                        new NoticeWorkedNotPaidCalculationRequestModel()
                        {
                             InputSource = InputSource.Rp1,
                             EmploymentStartDate = new DateTime(2015, 8, 2),
                             InsolvencyDate = new DateTime(2018, 7, 20),
                             DateNoticeGiven = new DateTime(2018, 7, 20),
                             DismissalDate = new DateTime(2018, 7, 20),
                             UnpaidPeriodFrom = new DateTime(2018, 7, 1),
                             UnpaidPeriodTo = new DateTime(2018, 7, 8),
                             WeeklyWage = 320,
                             ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                             PayDay = 6,
                             IsTaxable = true
                         },
                        new NoticeWorkedNotPaidCalculationRequestModel()
                        {
                             InputSource = InputSource.Rp14a,
                             EmploymentStartDate = new DateTime(2015, 8, 2),
                             InsolvencyDate = new DateTime(2018, 7, 20),
                             DateNoticeGiven = new DateTime(2018, 7, 20),
                             DismissalDate = new DateTime(2018, 7, 20),
                             UnpaidPeriodFrom = new DateTime(2018, 7, 8),
                             UnpaidPeriodTo = new DateTime(2018, 7, 10),
                             WeeklyWage = 320,
                             ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                             PayDay = 6,
                             IsTaxable = true
                         }
                    }
            };

            var response = NoticeControllerTestsDataGenerator.GetValidResponseData();

            _service.Setup(m => m.PerformNoticePayCompositeCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds_WithNoAPData()
        {
            //Arrange
            var request = new NoticePayCompositeCalculationRequestModel
            {
                Cnp = new CompensatoryNoticePayCalculationRequestModel
                {
                    InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                    InsolvencyDate = new DateTime(2018, 6, 1),
                    DismissalDate = new DateTime(2018, 06, 05),
                    DateNoticeGiven = new DateTime(2018, 06, 01),
                    WeeklyWage = 330.25m,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    IsTaxable = true,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    DeceasedDate = null
                },
                Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>(),
            };
            var response = new NoticePayCompositeCalculationResponseDTO();

            _service.Setup(m => m.PerformNoticePayCompositeCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new NoticeController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }
    }
}
