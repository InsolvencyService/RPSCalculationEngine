using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using System;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class HolidayPayAccruedCalculationsServiceTests
    {
        private readonly HolidayPayAccruedCalculationService _holidayPayAccruedCalculationService;
        private readonly IOptions<ConfigLookupRoot> _options;
        public HolidayPayAccruedCalculationsServiceTests()
        {
            _holidayPayAccruedCalculationService = new HolidayPayAccruedCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_CalculatedHolidayPayAccruedResponse()
        {
            // Arrange
            var inputData = await Task.FromResult(HolidayPayAccruedTestsDataGenerator.GetValidRequestData());

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(479.00m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(260.00m);
            outputData.Result.WorkingDaysInClaim.Should().Be(56.00m);
            outputData.Result.ProRataAccruedDays.Should().Be(8.0308m);

            outputData.Result.WeeklyResults.Count.Should().Be(2);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(479.00m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(48.65m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(6.39m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(188.21m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);

            outputData.Result.WeeklyResults[1].WeekNumber.Should().Be(2);
            outputData.Result.WeeklyResults[1].MaximumEntitlement.Should().Be(275.82m);
            outputData.Result.WeeklyResults[1].EmployerEntitlement.Should().Be(147.45m);
            outputData.Result.WeeklyResults[1].GrossEntitlement.Should().Be(147.45m);
            outputData.Result.WeeklyResults[1].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[1].TaxDeducted.Should().Be(29.49m);
            outputData.Result.WeeklyResults[1].NiDeducted.Should().Be(0.00m);
            outputData.Result.WeeklyResults[1].NetEntitlement.Should().Be(117.96m);
            outputData.Result.WeeklyResults[1].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[1].GrossEntitlement);
            outputData.Result.WeeklyResults[1].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_CalculatedHolidayPayAccruedResponseWithIpConfirmedDaysOverride()
        {
            // Arrange
            var inputData = await Task.FromResult(HolidayPayAccruedTestsDataGenerator.GetRequestDataWithIpConfirmedDays());

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(479.00m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(260.00m);
            outputData.Result.WorkingDaysInClaim.Should().Be(56.00m);
            outputData.Result.ProRataAccruedDays.Should().Be(5m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(479.00m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(48.65m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(6.39m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(188.21m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_WhenHolidayStartDate12MonthsBeforeInsolvencyDate()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 1, 14),
                EmpStartDate = new DateTime(2016, 1, 1),
                DismissalDate = new DateTime(2018, 11, 30),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2018, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Saturday,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 575.34m,
                DaysCFwd = 8m,
                DaysTaken = 10m,
                IpConfirmedDays = 41
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(508m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(261.00m);
            outputData.Result.WorkingDaysInClaim.Should().Be(230m);
            outputData.Result.ProRataAccruedDays.Should().Be(22.6743m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_PartialDay0Point99()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 10, 22),
                EmpStartDate = new DateTime(2010, 10, 22),
                DismissalDate = new DateTime(2019, 10, 22),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2019, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Friday,
                ShiftPattern = new List<string> { "1", "3", "4" },
                WeeklyWage = 800m,
                DaysCFwd = 0m,
                DaysTaken = 0m,
                IpConfirmedDays = 0.99m
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(525m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(156m);
            outputData.Result.WorkingDaysInClaim.Should().Be(126m);
            outputData.Result.ProRataAccruedDays.Should().Be(0.99m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(224.25m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(264m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(224.25m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(44.85m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(4.11m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(175.29m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_PartialDay1()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 10, 22),
                EmpStartDate = new DateTime(2010, 10, 22),
                DismissalDate = new DateTime(2019, 10, 22),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2019, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Friday,
                ShiftPattern = new List<string> { "1", "3", "4" },
                WeeklyWage = 800m,
                DaysCFwd = 0m,
                DaysTaken = 0m,
                IpConfirmedDays = 1m
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(525m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(156m);
            outputData.Result.WorkingDaysInClaim.Should().Be(126m);
            outputData.Result.ProRataAccruedDays.Should().Be(1m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(225m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(266.67m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(225m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(45m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(4.20m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(175.80m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_PartialDay1Point1()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 10, 22),
                EmpStartDate = new DateTime(2010, 10, 22),
                DismissalDate = new DateTime(2019, 10, 22),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2019, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Friday,
                ShiftPattern = new List<string> { "1", "3", "4" },
                WeeklyWage = 800m,
                DaysCFwd = 0m,
                DaysTaken = 0m,
                IpConfirmedDays = 1.1m
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(525m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(156m);
            outputData.Result.WorkingDaysInClaim.Should().Be(126m);
            outputData.Result.ProRataAccruedDays.Should().Be(1.1m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(307.5m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(293.33m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(293.33m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(58.67m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(12.40m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(222.26m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_PartialDay1Point9()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 10, 22),
                EmpStartDate = new DateTime(2010, 10, 22),
                DismissalDate = new DateTime(2019, 10, 22),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2019, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Friday,
                ShiftPattern = new List<string> { "1", "3", "4" },
                WeeklyWage = 800m,
                DaysCFwd = 0m,
                DaysTaken = 0m,
                IpConfirmedDays = 1.9m
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(525m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(156m);
            outputData.Result.WorkingDaysInClaim.Should().Be(126m);
            outputData.Result.ProRataAccruedDays.Should().Be(1.9m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(367.5m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(506.67m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(367.5m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(73.5m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(21.30m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(272.70m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_PartialDay2()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 10, 22),
                EmpStartDate = new DateTime(2010, 10, 22),
                DismissalDate = new DateTime(2019, 10, 22),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2019, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Friday,
                ShiftPattern = new List<string> { "1", "3", "4" },
                WeeklyWage = 800m,
                DaysCFwd = 0m,
                DaysTaken = 0m,
                IpConfirmedDays = 2m
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(525m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(156m);
            outputData.Result.WorkingDaysInClaim.Should().Be(126m);
            outputData.Result.ProRataAccruedDays.Should().Be(2m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(375m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(533.33m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(375m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(75m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(22.20m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(277.80m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_PartialDay2Point99()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 10, 22),
                EmpStartDate = new DateTime(2010, 10, 22),
                DismissalDate = new DateTime(2019, 10, 22),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2019, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Friday,
                ShiftPattern = new List<string> { "1", "3", "4" },
                WeeklyWage = 800m,
                DaysCFwd = 0m,
                DaysTaken = 0m,
                IpConfirmedDays = 2.99m
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(525m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(156m);
            outputData.Result.WorkingDaysInClaim.Should().Be(126m);
            outputData.Result.ProRataAccruedDays.Should().Be(2.99m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(449.25m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(797.33m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(449.25m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(89.85m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(31.11m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(328.29m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_PartialDay3()
        {
            // Arrange
            var inputData = new HolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2019, 10, 22),
                EmpStartDate = new DateTime(2010, 10, 22),
                DismissalDate = new DateTime(2019, 10, 22),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2019, 01, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Friday,
                ShiftPattern = new List<string> { "1", "3", "4" },
                WeeklyWage = 800m,
                DaysCFwd = 0m,
                DaysTaken = 0m,
                IpConfirmedDays = 3m
            };

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(525m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(156m);
            outputData.Result.WorkingDaysInClaim.Should().Be(126m);
            outputData.Result.ProRataAccruedDays.Should().Be(3m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(525m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(800m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(525m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(105m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(40.20m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(379.80m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }


        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedForIrregularHoursWorkersCalculationAsync_Return_CalculatedHolidayPayAccruedResponse()
        {
            // Arrange
            var inputData = await Task.FromResult(IrregularHolidayPayAccruedTestsDataGenerator.GetValidRequestForIrregularHourWorkerData());

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedForIrregularHoursWorkersCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(571.00m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(261.00m);
            outputData.Result.WorkingDaysInClaim.Should().Be(65.00m);
            outputData.Result.ProRataAccruedDays.Should().Be(4.9732m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(487.24m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(241.95m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(241.95m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(48.39m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(6.23m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(187.33m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedForIrregularHoursWorkersCalculationWithSource_OverrideAsync_Return_CalculatedHolidayPayAccruedResponse()
        {
            // Arrange
            var inputData = await Task.FromResult(IrregularHolidayPayAccruedTestsDataGenerator.GetValidRequestForIrregularHourWorkerDataWithSource_Override());

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedForIrregularHoursWorkersCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(571.00m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(261.00m);
            outputData.Result.WorkingDaysInClaim.Should().Be(65.00m);
            outputData.Result.ProRataAccruedDays.Should().Be(10.9732m);

            outputData.Result.WeeklyResults.Count.Should().Be(3);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(571m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(48.65m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(6.39m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(188.21m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);

            outputData.Result.WeeklyResults[1].WeekNumber.Should().Be(2);
            outputData.Result.WeeklyResults[1].MaximumEntitlement.Should().Be(571m);
            outputData.Result.WeeklyResults[1].EmployerEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[1].GrossEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[1].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[1].TaxDeducted.Should().Be(48.65m);
            outputData.Result.WeeklyResults[1].NiDeducted.Should().Be(6.39m);
            outputData.Result.WeeklyResults[1].NetEntitlement.Should().Be(188.21m);
            outputData.Result.WeeklyResults[1].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[1].NonPreferentialClaim.Should().Be(0m);
        }
    }
}

