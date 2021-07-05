using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.Controllers;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
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
    public class APPAControllerTests 
    {
        private readonly Mock<ILogger<APPAController>> _mockLogger;
        private readonly IOptions<ConfigLookupRoot> _confOptions;
        private readonly Mock<IAPPACalculationService> _service;

        public APPAControllerTests()
        {
            _mockLogger = new Mock<ILogger<APPAController>>();
            _mockLogger.Setup(x => x.Log(It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<String>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();

            _confOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
            _service = new Mock<IAPPACalculationService>();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void PingGet_ReturnsSuccess()
        {
            //Arrange
            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = controller.Get();

            //Assert
            var contentResult = result.Should().BeOfType<ContentResult>().Subject;
            contentResult.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
            contentResult.Content.Should().Be("PingGet response from RPS Api APPA");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds()
        {
            //Arrange
            var request = APPAControllerTestsDataGenerator.GetValidRequestData();
            var response = APPAControllerTestsDataGenerator.GetValidResponseData();

            _service.Setup(m => m.PerformCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }


        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(APPAValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAValidationError(APPACalculationRequestModel request, string expectedErrorMessage)
        {
            //Arrange
            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

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
        [ClassData(typeof(ArrearsOfPayValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAAPValidationError(ArrearsOfPayCalculationRequestModel apRequest, string expectedErrorMessage)
        {
            //Arrange
            var request = new APPACalculationRequestModel()
            {
                Ap = new List<ArrearsOfPayCalculationRequestModel>()
                {
                    apRequest
                }
            };

            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

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
        [ClassData(typeof(ProtectiveAwardValidationTestDataHelper))]
        public async Task PostAsync_FailsWithBadRequest_WhenThereIsAPAValidationError(ProtectiveAwardCalculationRequestModel paRequest, string expectedErrorMessage)
        {
            //Arrange
            var request = new APPACalculationRequestModel()
            {
                Pa = paRequest
            };

            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

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

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_SucceedsWithOverlappingPeriodsInDifferentInputSources()
        {
            //Arrange
            var request = new APPACalculationRequestModel()
            {
                Ap = new List<ArrearsOfPayCalculationRequestModel>()
                    {
                        new ArrearsOfPayCalculationRequestModel
                        {
                            InputSource = InputSource.Rp1,
                            InsolvencyDate = new DateTime(2017, 03, 22),
                            EmploymentStartDate = new DateTime(2016, 12, 19),
                            DismissalDate = new DateTime(2017, 01, 03),
                            DateNoticeGiven = new DateTime(2016, 12, 19),
                            UnpaidPeriodFrom = new DateTime(2016, 12, 19),
                            UnpaidPeriodTo = new DateTime(2017, 01, 03),
                            ApClaimAmount = 150,
                            IsTaxable = true,
                            PayDay = (int)DayOfWeek.Saturday,
                            ShiftPattern = new List<string> { "2", "3", "4", "5", "6" },
                            WeeklyWage = 243.25m
                        },
                        new ArrearsOfPayCalculationRequestModel
                        {
                            InputSource = InputSource.Rp14a,
                            InsolvencyDate = new DateTime(2017, 03, 22),
                            EmploymentStartDate = new DateTime(2016, 12, 19),
                            DismissalDate = new DateTime(2017, 01, 03),
                            DateNoticeGiven = new DateTime(2016, 12, 19),
                            UnpaidPeriodFrom = new DateTime(2017, 1, 3),
                            UnpaidPeriodTo = new DateTime(2017, 01, 4),
                            ApClaimAmount = 150,
                            IsTaxable = true,
                            PayDay = (int)DayOfWeek.Saturday,
                            ShiftPattern = new List<string> { "2", "3", "4", "5", "6" },
                            WeeklyWage = 243.25m
                        }
                    },
                Pa = null
            };
            var response = APPAControllerTestsDataGenerator.GetValidResponseData();

            _service.Setup(m => m.PerformCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

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
            var request = new APPACalculationRequestModel
            {
                Ap = new List<ArrearsOfPayCalculationRequestModel>(),
                Pa = new ProtectiveAwardCalculationRequestModel()
                {
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 4, 6),
                    DismissalDate = new DateTime(2018, 10, 20),
                    TribunalAwardDate = new DateTime(2018, 10, 21),
                    ProtectiveAwardStartDate = new DateTime(2018, 10, 22),
                    ProtectiveAwardDays = 90,
                    PayDay = 6,
                    WeeklyWage = 400M,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" }
                }
            };
            var response = new APPACalculationResponseDTO();

            _service.Setup(m => m.PerformCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

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
            var request = new APPACalculationRequestModel
            {
                Rp14aNotRequired = true,
                Ap = new List<ArrearsOfPayCalculationRequestModel>()
                {
                    new ArrearsOfPayCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp1,
                        InsolvencyDate = new DateTime(2018, 10, 20),
                        EmploymentStartDate = new DateTime(2016, 04, 06),
                        DismissalDate = new DateTime(2018, 10, 20),
                        DateNoticeGiven = new DateTime(2018, 10, 6),
                        UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                        UnpaidPeriodTo = new DateTime(2018, 10, 9),
                        ApClaimAmount = 700M,
                        IsTaxable = true,
                        PayDay = 6,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 400m
                    },
                    new ArrearsOfPayCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp1,
                        InsolvencyDate = new DateTime(2018, 10, 20),
                        EmploymentStartDate = new DateTime(2016, 04, 06),
                        DismissalDate = new DateTime(2018, 10, 20),
                        DateNoticeGiven = new DateTime(2018, 10, 14),
                        UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                        UnpaidPeriodTo = new DateTime(2018, 10, 18),
                        ApClaimAmount = 600M,
                        IsTaxable = true,
                        PayDay = 6,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 400m
                    }
                },
                Pa = new ProtectiveAwardCalculationRequestModel()
                {
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 4, 6),
                    DismissalDate = new DateTime(2018, 10, 20),
                    TribunalAwardDate = new DateTime(2018, 10, 21),
                    ProtectiveAwardStartDate = new DateTime(2018, 10, 22),
                    ProtectiveAwardDays = 90,
                    PayDay = 6,
                    WeeklyWage = 400M,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" }
                }
            };
            var response = new APPACalculationResponseDTO();

            _service.Setup(m => m.PerformCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PostAsync_Succeeds_WithRP14aDataAndRp1NotRequiredOverride()
        {
            //Arrange
            var request = new APPACalculationRequestModel
            {
                Rp1NotRequired = true,
                Rp14aNotRequired = false,
                Ap = new List<ArrearsOfPayCalculationRequestModel>()
                {
                    new ArrearsOfPayCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp14a,
                        InsolvencyDate = new DateTime(2018, 10, 20),
                        EmploymentStartDate = new DateTime(2016, 04, 06),
                        DismissalDate = new DateTime(2018, 10, 20),
                        DateNoticeGiven = new DateTime(2018, 10, 6),
                        UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                        UnpaidPeriodTo = new DateTime(2018, 10, 9),
                        ApClaimAmount = 700M,
                        IsTaxable = true,
                        PayDay = 6,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 400m
                    },
                    new ArrearsOfPayCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp14a,
                        InsolvencyDate = new DateTime(2018, 10, 20),
                        EmploymentStartDate = new DateTime(2016, 04, 06),
                        DismissalDate = new DateTime(2018, 10, 20),
                        DateNoticeGiven = new DateTime(2018, 10, 14),
                        UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                        UnpaidPeriodTo = new DateTime(2018, 10, 18),
                        ApClaimAmount = 600M,
                        IsTaxable = true,
                        PayDay = 6,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 400m
                    }
                },
                Pa = new ProtectiveAwardCalculationRequestModel()
                {
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 4, 6),
                    DismissalDate = new DateTime(2018, 10, 20),
                    TribunalAwardDate = new DateTime(2018, 10, 21),
                    ProtectiveAwardStartDate = new DateTime(2018, 10, 22),
                    ProtectiveAwardDays = 90,
                    PayDay = 6,
                    WeeklyWage = 400M,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" }
                }
            };
            var response = new APPACalculationResponseDTO();

            _service.Setup(m => m.PerformCalculationAsync(request, _confOptions)).ReturnsAsync(response);
            var controller = new APPAController(_service.Object, _mockLogger.Object, _confOptions);

            //Act
            var result = await controller.PostAsync(request);

            //Assert
            var okObjectRequest = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectRequest.StatusCode.Should().Be((int)System.Net.HttpStatusCode.OK);
        }
    }
}