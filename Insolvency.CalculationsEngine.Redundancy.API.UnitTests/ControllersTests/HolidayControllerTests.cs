using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
//using Microsoft.Extensions.Logging.Internal;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.ControllersTests
{
    public class HolidayControllerTests 
    {
        private readonly Mock<ILogger<HolidayController>> _mockLogger;
        private readonly IOptions<ConfigLookupRoot> _confOptions;
        private readonly Mock<IHolidayCalculationService> _service;

        public HolidayControllerTests()
        {
            _mockLogger = new Mock<ILogger<HolidayController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<String>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();

            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
            _service = new Mock<IHolidayCalculationService>();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = controller.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api Holiday");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds()
        {
            //Arrange
            var request = HolidayControllerTestsDataGenerator.GetValidRequestData();
            var response = HolidayControllerTestsDataGenerator.GetValidResponseData();

            _service.Setup(m => m.PerformHolidayCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }


        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(HolidayValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAValidationError(HolidayCalculationRequestModel request, string expectedErrorMessage)
        {
            //Arrange
            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

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
                 (It.IsAnyType)It.Is<object>(v => v.ToString().Contains(expectedErrorMessage)),
                  null,
                  It.IsAny<Func<It.IsAnyType, Exception, string>>()));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(HolidayPayAccruedValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAHpaValidationError(HolidayPayAccruedCalculationRequestModel hpaRequest, string expectedErrorMessage)
        {
            //Arrange
            var request = new HolidayCalculationRequestModel()
            {
                Hpa = hpaRequest
            };

            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

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
                  (It.IsAnyType)It.Is<object>(v => v.ToString().Contains(expectedErrorMessage)),
                   null,
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(HolidayTakenNotPaidValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAHtnpValidationError(HolidayTakenNotPaidCalculationRequestModel htnpRequest, string expectedErrorMessage)
        {
            //Arrange
            var request = new HolidayCalculationRequestModel()
            {
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                {
                    htnpRequest
                }
            };

            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

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
                 (It.IsAnyType)It.Is<object>(v => v.ToString().Contains(expectedErrorMessage)),
                  null,
                  It.IsAny<Func<It.IsAnyType, Exception, string>>()));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_SucceedsWithOverlappingPeriodsInDifferentInputSources()
        {
            //Arrange
            var request = new HolidayCalculationRequestModel()
            {
                Hpa = null,
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                    {
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 10), new DateTime(2018, 9, 18), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 18), new DateTime(2018, 9, 20), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                    }
            };
            var response = HolidayControllerTestsDataGenerator.GetValidResponseData();

            _service.Setup(m => m.PerformHolidayCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds_WithNoHTNPData()
        {
            //Arrange
            var request = new HolidayCalculationRequestModel
            {
                Hpa = new HolidayPayAccruedCalculationRequestModel
                {
                    InsolvencyDate = new DateTime(2017, 03, 22),
                    EmpStartDate = new DateTime(2016, 12, 19),
                    DismissalDate = new DateTime(2017, 03, 20),
                    ContractedHolEntitlement = 25,
                    HolidayYearStart = new DateTime(2017, 01, 01),
                    IsTaxable = true,
                    PayDay = (int)DayOfWeek.Saturday,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 243.25m,
                    DaysCFwd = 5.5m,
                    DaysTaken = 3.5m,
                    IpConfirmedDays = 25
                },
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
            };
            var response = new HolidayCalculationResponseDTO();

            _service.Setup(m => m.PerformHolidayCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds_WithNoRP14aDataAndOverride()
        {
            //Arrange
            var request = new HolidayCalculationRequestModel
            {
                Rp14aNotRequired = true,
                Hpa = new HolidayPayAccruedCalculationRequestModel
                {
                    InsolvencyDate = new DateTime(2017, 03, 22),
                    EmpStartDate = new DateTime(2016, 12, 19),
                    DismissalDate = new DateTime(2017, 03, 20),
                    ContractedHolEntitlement = 25,
                    HolidayYearStart = new DateTime(2017, 01, 01),
                    IsTaxable = true,
                    PayDay = (int)DayOfWeek.Saturday,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 243.25m,
                    DaysCFwd = 5.5m,
                    DaysTaken = 3.5m,
                    IpConfirmedDays = 25
                },
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                {
                    new HolidayTakenNotPaidCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp1,
                        InsolvencyDate = new DateTime(2018, 01, 10),
                        DismissalDate = new DateTime(2018, 01, 03),
                        UnpaidPeriodFrom = new DateTime(2017, 12, 12),
                        UnpaidPeriodTo = new DateTime(2017, 12, 29),
                        WeeklyWage = 306.85m,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        PayDay = 6,
                        IsTaxable = true
                    },
                }
            };
            var response = new HolidayCalculationResponseDTO();

            _service.Setup(m => m.PerformHolidayCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds_WithRP14aDataAndRP1NotRequiredOverride()
        {
            //Arrange
            var request = new HolidayCalculationRequestModel
            {
                Rp1NotRequired =true,
                Rp14aNotRequired = true,
                Hpa = new HolidayPayAccruedCalculationRequestModel
                {
                    InsolvencyDate = new DateTime(2017, 03, 22),
                    EmpStartDate = new DateTime(2016, 12, 19),
                    DismissalDate = new DateTime(2017, 03, 20),
                    ContractedHolEntitlement = 25,
                    HolidayYearStart = new DateTime(2017, 01, 01),
                    IsTaxable = true,
                    PayDay = (int)DayOfWeek.Saturday,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 243.25m,
                    DaysCFwd = 5.5m,
                    DaysTaken = 3.5m,
                    IpConfirmedDays = 25
                },
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                {
                    new HolidayTakenNotPaidCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp14a,
                        InsolvencyDate = new DateTime(2018, 01, 10),
                        DismissalDate = new DateTime(2018, 01, 03),
                        UnpaidPeriodFrom = new DateTime(2017, 12, 12),
                        UnpaidPeriodTo = new DateTime(2017, 12, 29),
                        WeeklyWage = 306.85m,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        PayDay = 6,
                        IsTaxable = true
                    }
                }
            };
            var response = new HolidayCalculationResponseDTO();

            _service.Setup(m => m.PerformHolidayCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new HolidayController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }
    }
}