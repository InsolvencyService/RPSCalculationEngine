using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class HolidayCalculationsServiceTests
    {
        private readonly HolidayCalculationService _service;
        private readonly Mock<IHolidayPayAccruedCalculationService> _hpaService;
        private readonly Mock<IHolidayTakenNotPaidCalculationService> _htnpService;

        private readonly IOptions<ConfigLookupRoot> _options;

        public HolidayCalculationsServiceTests()
        {
            _hpaService = new Mock<IHolidayPayAccruedCalculationService>();
            _htnpService = new Mock<IHolidayTakenNotPaidCalculationService>();
            _service = new HolidayCalculationService(_hpaService.Object, _htnpService.Object);

            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsHpaAndHtnpCalculationsWithRp14aSelected()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var hpaRequest = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2018, 10, 1),
                EmpStartDate = new DateTime(2016, 1, 1),
                DismissalDate = new DateTime(2018, 10, 1),
                ContractedHolEntitlement = 25,
                HolidayYearStart = new DateTime(2018, 1, 1),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Saturday,
                ShiftPattern = shiftPattern,
                WeeklyWage = 320m,
                DaysCFwd = 0m,
                DaysTaken = 8m,
            };

            var hpaResponse = new HolidayPayAccruedResponseDTO()
            {
                StatutoryMax = 508,
                HolidaysOwed = 28,
                BusinessDaysInClaim = 261,
                WorkingDaysInClaim = 196,
                ProRataAccruedDays = 13.0268m,
                WeeklyResults = new List<HolidayPayAccruedWeeklyResult>()
                {
                    new HolidayPayAccruedWeeklyResult(1, 508m, 320m, 320m, true, 64m, 18.96m, 237.04m, 320m, 0m),
                    new HolidayPayAccruedWeeklyResult(2, 508m, 320m, 320m, true, 64m, 18.96m, 237.04m, 320m, 0m),
                    new HolidayPayAccruedWeeklyResult(3, 292.23m, 193.72m, 193.72m, true, 38.74m, 3.81m, 151.17m, 193.72m, 0m),
                }
            };

            var htnpRequest = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 1), new DateTime(2018, 8, 12), 320, shiftPattern, 6, true),
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 2), new DateTime(2018, 8, 14), 320, shiftPattern, 6, true)
            };

            var htnpResponseRp1 = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp1,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 192, 192, true, 38.4m, 3.6m, 150m, 7, 3, 508, 192, 192, true),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, true),
                }
            };

            var htnpResponseRp14a = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp14a,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, false),
                    new HolidayTakenNotPaidWeeklyResult(3, new DateTime(2018, 8, 18), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false)
                }
            };

            _hpaService.Setup(x => x.PerformHolidayPayAccruedCalculationAsync(hpaRequest, _options)).ReturnsAsync(hpaResponse);
            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp1, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp1);
            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp14a, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp14a);

            var request = new HolidayCalculationRequestModel()
            {
                Hpa = hpaRequest,
                Htnp = htnpRequest
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.Should().NotBeNull();
            results.Htnp.Should().NotBeNull();
            results.Htnp.SelectedInputSource.Should().Be(InputSource.Rp1);

            _hpaService.Verify(m => m.PerformHolidayPayAccruedCalculationAsync(
                hpaRequest,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            _htnpService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
                InputSource.Rp1,
                14.9732m,
                16.9732m,
                hpaRequest.HolidayYearStart,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            _htnpService.Verify(m => m.PerformCalculationAsync(
                htnpRequest,
                InputSource.Rp14a,
                0m,
                0m,
                hpaRequest.HolidayYearStart,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsHpaAndHtnpCalculationsWithRp1Selected()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var hpaRequest = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2018, 10, 1),
                EmpStartDate = new DateTime(2016, 1, 1),
                DismissalDate = new DateTime(2018, 10, 1),
                ContractedHolEntitlement = 25,
                HolidayYearStart = new DateTime(2018, 1, 1),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Saturday,
                ShiftPattern = shiftPattern,
                WeeklyWage = 320m,
                DaysCFwd = 0m,
                DaysTaken = 8m,
            };

            var hpaResponse = new HolidayPayAccruedResponseDTO()
            {
                StatutoryMax = 508,
                HolidaysOwed = 28,
                BusinessDaysInClaim = 261,
                WorkingDaysInClaim = 196,
                ProRataAccruedDays = 13.0268m,
                WeeklyResults = new List<HolidayPayAccruedWeeklyResult>()
                {
                    new HolidayPayAccruedWeeklyResult(1, 508m, 320m, 320m, true, 64m, 18.96m, 237.04m, 320m, 0m),
                    new HolidayPayAccruedWeeklyResult(2, 508m, 320m, 320m, true, 64m, 18.96m, 237.04m, 320m, 0m),
                    new HolidayPayAccruedWeeklyResult(3, 292.23m, 193.72m, 193.72m, true, 38.74m, 3.81m, 151.17m, 193.72m, 0m),
                }
            };

            var htnpRequest = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 1), new DateTime(2018, 8, 12), 320, shiftPattern, 6, true),
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 2), new DateTime(2018, 8, 14), 320, shiftPattern, 6, true)
            };

            var htnpResponseRp1 = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp1,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, false),
                    new HolidayTakenNotPaidWeeklyResult(3, new DateTime(2018, 8, 18), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false)
                }
            };

            var htnpResponseRp14a = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp14a,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 192, 192, true, 38.4m, 3.6m, 150m, 7, 3, 508, 192, 192, true),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, true),
                }
            };

            _hpaService.Setup(x => x.PerformHolidayPayAccruedCalculationAsync(hpaRequest, _options)).ReturnsAsync(hpaResponse);
            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp1, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp1);
            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp14a, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp14a);

            var request = new HolidayCalculationRequestModel()
            {
                Hpa = hpaRequest,
                Htnp = htnpRequest
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.Should().NotBeNull();
            results.Htnp.Should().NotBeNull();
            results.Htnp.SelectedInputSource.Should().Be(InputSource.Rp14a);

            _hpaService.Verify(m => m.PerformHolidayPayAccruedCalculationAsync(
                hpaRequest,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            _htnpService.Verify(m => m.PerformCalculationAsync(
                htnpRequest,
                InputSource.Rp1,
                0m,
                0m,
                hpaRequest.HolidayYearStart,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            _htnpService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
                InputSource.Rp14a,
                14.9732m,
                16.9732m,
                hpaRequest.HolidayYearStart,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsHpaAndHtnpCalculationsWithNoHpaAndRp1Selected()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var htnpRequest = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 1), new DateTime(2018, 8, 12), 320, shiftPattern, 6, true),
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 2), new DateTime(2018, 8, 14), 320, shiftPattern, 6, true)
            };

            var htnpResponseRp1 = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp1,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 192, 192, true, 38.4m, 3.6m, 150m, 7, 3, 508, 192, 192, true),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, true),
                }
            };

            var htnpResponseRp14a = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp14a,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, false),
                    new HolidayTakenNotPaidWeeklyResult(3, new DateTime(2018, 8, 18), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false)
                }
            };

            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp1, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp1);
            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp14a, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp14a);

            var request = new HolidayCalculationRequestModel()
            {
                Hpa = null,
                Htnp = htnpRequest
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.Should().BeNull();
            results.Htnp.Should().NotBeNull();
            results.Htnp.SelectedInputSource.Should().Be(InputSource.Rp1);

            _htnpService.Verify(m => m.PerformCalculationAsync(
                htnpRequest,
                InputSource.Rp1,
                0,
                30m,
                null,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            _htnpService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
                InputSource.Rp14a,
                0m,
                0m,
                null,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsHpaAndHtnpCalculationsWithNoHpaAndRp14aSelected()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var htnpRequest = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 1), new DateTime(2018, 8, 12), 320, shiftPattern, 6, true),
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), new DateTime(2018, 8, 2), new DateTime(2018, 8, 14), 320, shiftPattern, 6, true)
            };

            var htnpResponseRp1 = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp1,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, false),
                    new HolidayTakenNotPaidWeeklyResult(3, new DateTime(2018, 8, 18), 508, 128, 128, true, 25.6m, 0m, 102.4m, 7, 2, 508, 128, 128, false)
                }
            };

            var htnpResponseRp14a = new HolidayTakenNotPaidResponseDTO()
            {
                InputSource = InputSource.Rp14a,
                WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 8, 4), 508, 192, 192, true, 38.4m, 3.6m, 150m, 7, 3, 508, 192, 192, true),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 8, 11), 508, 320, 320, true, 64m, 18.96m, 237.04m, 7, 5, 508, 320, 320, true),
                }
            };

            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp1, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp1);
            _htnpService.Setup(x => x.PerformCalculationAsync(htnpRequest, InputSource.Rp14a, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp14a);

            var request = new HolidayCalculationRequestModel()
            {
                Hpa = null,
                Htnp = htnpRequest
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.Should().BeNull();
            results.Htnp.Should().NotBeNull();
            results.Htnp.SelectedInputSource.Should().Be(InputSource.Rp14a);

            _htnpService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
                InputSource.Rp1,
                0m,
                0m,
                null,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            _htnpService.Verify(m => m.PerformCalculationAsync(
                htnpRequest,
                InputSource.Rp14a,
                0m,
                30m,
                null,
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsOnlyHpaCalculation()
        {
            // Arrange
            var hpaRequest = HolidayPayAccruedTestsDataGenerator.GetValidRequestData();
            var hpaResponse = HolidayPayAccruedTestsDataGenerator.GetValidResponseData();
            _hpaService.Setup(m => m.PerformHolidayPayAccruedCalculationAsync(hpaRequest, _options)).ReturnsAsync(hpaResponse);

            var request = new HolidayCalculationRequestModel()
            {
                Hpa = hpaRequest,
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.WeeklyResults.Count.Should().Be(hpaResponse.WeeklyResults.Count);
            results.Htnp.Should().BeNull();
            _htnpService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<decimal>(),
                It.IsAny<DateTime?>(),
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsOnlyHtnpCalculation()
        {
            // Arrange
            var htnpRequestRp1 = HolidayTakenNotPaidControllerTestsDataGenerator.GetValidRp1RequestData();
            var htnpRequestRp14a = HolidayTakenNotPaidControllerTestsDataGenerator.GetValidRp14aRequestData();
            var htnpResponseRp1 = HolidayTakenNotPaidControllerTestsDataGenerator.GetValidRp1ResponseData();
            var htnpResponseRp14a = HolidayTakenNotPaidControllerTestsDataGenerator.GetValidRp14aResponseData();
            var htnpRequest = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                htnpRequestRp1,
                htnpRequestRp14a
            };

            _htnpService.Setup(m => m.PerformCalculationAsync(htnpRequest, InputSource.Rp1, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp1);
            _htnpService.Setup(m => m.PerformCalculationAsync(htnpRequest, InputSource.Rp14a, It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<DateTime?>(), _options)).ReturnsAsync(htnpResponseRp14a);

            var request = new HolidayCalculationRequestModel()
            {
                Hpa = null,
                Htnp = htnpRequest
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.Should().BeNull();
            _hpaService.Verify(m => m.PerformHolidayPayAccruedCalculationAsync(
                It.IsAny<HolidayPayAccruedCalculationRequestModel>(),
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
            _htnpService.Verify(m => m.PerformCalculationAsync(
               It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
               InputSource.Rp1,
               It.IsAny<decimal>(),
               It.IsAny<decimal>(),
               It.IsAny<DateTime?>(),
               It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
            _htnpService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
                InputSource.Rp14a,
                It.IsAny<decimal>(),
                It.IsAny<decimal>(),
                It.IsAny<DateTime?>(),
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsNoCalculationsWhenNoHtnpIsEmpty()
        {
            // Arrange
            var request = new HolidayCalculationRequestModel()
            {
                Hpa = null,
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.Should().BeNull();
            results.Htnp.Should().BeNull();

            _hpaService.Verify(m => m.PerformHolidayPayAccruedCalculationAsync(
                It.IsAny<HolidayPayAccruedCalculationRequestModel>(),
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
            _htnpService.Verify(m => m.PerformCalculationAsync(
              It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
              It.IsAny<string>(),
              It.IsAny<decimal>(),
              It.IsAny<decimal>(),
              It.IsAny<DateTime?>(),
              It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayCalculationAsync_PerformsNoCalculationsWhenNoHtnpIsNull()
        {
            // Arrange
            var request = new HolidayCalculationRequestModel()
            {
                Hpa = null,
                Htnp = null
            };

            // Act
            var results = await _service.PerformHolidayCalculationAsync(request, _options);

            // Assert
            results.Hpa.Should().BeNull();
            results.Htnp.Should().BeNull();

            _hpaService.Verify(m => m.PerformHolidayPayAccruedCalculationAsync(
                It.IsAny<HolidayPayAccruedCalculationRequestModel>(),
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
            _htnpService.Verify(m => m.PerformCalculationAsync(
              It.IsAny<List<HolidayTakenNotPaidCalculationRequestModel>>(),
              It.IsAny<string>(),
              It.IsAny<decimal>(),
              It.IsAny<decimal>(),
              It.IsAny<DateTime?>(),
              It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
        }
    }
}



