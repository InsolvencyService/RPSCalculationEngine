using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Holiday.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ExtensionsTests
{
    public class HolidayPayAccruedExtensionsTests
    {
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetTotalBusinessDaysInClaimPeriod_Returns_TotalBusinessDaysInClaimPeriod()
        {
            // Arrange
            var shiftPattern = new List<String>() { "1", "2", "3", "4" };
            var adjHolYearStart = new DateTime(2018, 02, 20);
            var holYearEndDate = new DateTime(2019, 02, 20);
            var expectedTotalBusinessDays = 209;

            // Act
            var result = await expectedTotalBusinessDays.GetTotalBusinessDaysInHolidayClaim(adjHolYearStart, holYearEndDate, shiftPattern);

            // Assert
            result.Should().Be(expectedTotalBusinessDays);
        }
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetTotalWorkingDaysInClaimPeriod_Returns_TotalBusinessDaysInClaimPeriod()
        {
            // Arrange
            var shiftPattern = new List<String>() {"1","2","3","4","5"};
            var holidayYearStart = new DateTime(2018, 1, 1);
            var holYearEndDate = new DateTime(2019, 1, 1);
            var dismissalDate = new DateTime(2018, 5, 18);
            var insolvencyDate = new DateTime(2018, 5, 31);
            var empStartDate = new DateTime(2017, 8, 22);

            var expectedTotalWorkingDays = 100;

            // Act
            var result = await expectedTotalWorkingDays.GetTotalWorkingDaysInHolidayClaim(
                shiftPattern, holidayYearStart,
            holYearEndDate, dismissalDate, insolvencyDate, empStartDate);

            // Assert
            result.Should().Be(expectedTotalWorkingDays);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetAdjustedHolidayEntitlement_Returns_HolidayEntitlementDaysInClaimPeriod()
        {
            // Arrange
            var statHolEntitlement = 28.00m;
            var contractHolEntitlement = 30.00m;

            var expectedTotalHolidayEntitlement = 30.00m;

            // Act
            var result = await statHolEntitlement.GetAdjustedHolidayEntitlement(contractHolEntitlement);

            // Assert
            result.Should().Be(expectedTotalHolidayEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetProRataAccruedHolidayEntitlement_Returns_ProRataHolidayEntitlement()
        {
            // Arrange
            var shiftPattern = new List<String>() { "1", "2", "3", "4", "5" };
            var adjHolidayEntitlement = 28.00m;
            var totalBusinessDaysInHolidayClaim = 261.00m;
            var totalWorkingDaysInHolidayClaim = 100.00m;
            var limitedDaysCFwd = 5.00m;
            var daysTaken = 3.00m;
            decimal? ipConfDaysDue = null;

            var expectedProRataAccruedHolidayEntitlement = 12.727969348659003831417624520m;

            // Act
            var result = await expectedProRataAccruedHolidayEntitlement.GetProRataAccruedDays(adjHolidayEntitlement, totalBusinessDaysInHolidayClaim,
                                                                totalWorkingDaysInHolidayClaim, limitedDaysCFwd, daysTaken, shiftPattern, ipConfDaysDue);

            // Assert
            result.Should().Be(expectedProRataAccruedHolidayEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetProRataAccruedHolidayEntitlement_Returns_ProRataHolidayEntitlement_WhenIpConfirmedDaysIsLower()
        {
            // Arrange
            var shiftPattern = new List<String>() { "1", "2", "3", "4", "5" };
            var adjHolidayEntitlement = 28.00m;
            var totalBusinessDaysInHolidayClaim = 261.00m;
            var totalWorkingDaysInHolidayClaim = 100.00m;
            var limitedDaysCFwd = 5.00m;
            var daysTaken = 3.00m;
            decimal? ipConfDaysDue = 3.5m;

            var expectedProRataAccruedHolidayEntitlement = 3.5m;

            // Act
            var result = await expectedProRataAccruedHolidayEntitlement.GetProRataAccruedDays(adjHolidayEntitlement, totalBusinessDaysInHolidayClaim,
                                                                totalWorkingDaysInHolidayClaim, limitedDaysCFwd, daysTaken, shiftPattern, ipConfDaysDue);

            // Assert
            result.Should().Be(expectedProRataAccruedHolidayEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetProRataAccruedHolidayEntitlement_Returns_ProRataHolidayEntitlement_WhenIpConfirmedDaysIsHigher()
        {
            // Arrange
            var shiftPattern = new List<String>() { "1", "2", "3", "4", "5" };
            var adjHolidayEntitlement = 28.00m;
            var totalBusinessDaysInHolidayClaim = 261.00m;
            var totalWorkingDaysInHolidayClaim = 100.00m;
            var limitedDaysCFwd = 5.00m;
            var daysTaken = 3.00m;
            decimal? ipConfDaysDue = 15m;

            var expectedProRataAccruedHolidayEntitlement = 12.727969348659003831417624520m;

            // Act
            var result = await expectedProRataAccruedHolidayEntitlement.GetProRataAccruedDays(adjHolidayEntitlement, totalBusinessDaysInHolidayClaim,
                                                                totalWorkingDaysInHolidayClaim, limitedDaysCFwd, daysTaken, shiftPattern, ipConfDaysDue);

            // Assert
            result.Should().Be(expectedProRataAccruedHolidayEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetProRataAccruedHolidayEntitlement_ReturnsMaxOf6TimesShiftPatternCount()
        {
            // Arrange
            var shiftPattern = new List<String>() { "1", "2", "3", "4", "5" };
            var adjHolidayEntitlement = 35m;
            var totalBusinessDaysInHolidayClaim = 261.00m;
            var totalWorkingDaysInHolidayClaim = 261.00m;
            var limitedDaysCFwd = 0m;
            var daysTaken = 0m;
            decimal? ipConfDaysDue = null;

            var expectedProRataAccruedHolidayEntitlement = 30m;

            // Act
            var result = await expectedProRataAccruedHolidayEntitlement.GetProRataAccruedDays(adjHolidayEntitlement, totalBusinessDaysInHolidayClaim,
                                                                totalWorkingDaysInHolidayClaim, limitedDaysCFwd, daysTaken, shiftPattern, ipConfDaysDue);

            // Assert
            result.Should().Be(expectedProRataAccruedHolidayEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetHolidayYearStart_Returns_AdjustedHolidayYearStart()
        {
            // Arrange
            var model = new HolidayPayAccruedCalculationRequestModel()
            {
                EmpStartDate = new DateTime(2017, 08, 22),
                HolidayYearStart = new DateTime(2018, 01, 01)
            };

            var expectedAdjEmpStartDate = new DateTime(2018, 01, 01);

            // Act
            var result = await model.GetHolidayYearStart();

            // Assert
            result.Should().Be(expectedAdjEmpStartDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetHolidayYearEnd_Returns_HolidayYearEnd()
        {
            // Arrange
            var model = new HolidayPayAccruedCalculationRequestModel()
            {
                EmpStartDate = new DateTime(2018, 08, 01),
                HolidayYearStart = new DateTime(2018, 03, 01)
            };

            var expectedHolidayYearEnd = new DateTime(2019, 07, 31);

            // Act
            var result = await model.GetHolidayYearEnd();

            // Assert
            result.Should().Be(expectedHolidayYearEnd);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetLimitedDaysCarriedForward_Returns_LimitedDaysCarriedForward()
        {
            // Arrange
            List<string> shiftPattern = new List<string>() { "1", "2", "3", "4", "5", "6" };
            var limitedDaysCFwd = 0m;
            var daysCFwd = 10.5m;

            var expectedLimitedDaysCFwd = 8.00m;

            // Act
            var result = await limitedDaysCFwd.GetLimitedDaysCFwd(shiftPattern, daysCFwd);

            // Assert
            result.Should().Be(expectedLimitedDaysCFwd);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetGrossEntitlement_Returns_GrossEntitlement()
        {
            // Arrange
            var grossEntitlement = 0m;
            var maxEntitlement = 508m;
            var employerEntitlement = 350.34m;

            var expectedGrossEntitlement = 350.34m;

            // Act
            var result = await grossEntitlement.GetGrossEntitlement(maxEntitlement, employerEntitlement);

            // Assert
            result.Should().Be(expectedGrossEntitlement);
        }
    }
}