using System;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.APPA.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Itenso.TimePeriod;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ExtensionsTests
{
    public class ArrearsOfPayExtensionsTests
    {
        public ArrearsOfPayExtensionsTests()
        {
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetAdjustedWeeklyWage_Returns_AdjustedWeeklyWage_When_ArrearsOfPayClaimAmount_Is_NotZero()
        {
            //Arrange
            //N.B weekly wage in request data is 150 NOT zero
            var requestData = ArrearsOfPayTestsDataGenerator.GetValidRequestData();
            //Act
            var result = await requestData.WeeklyWage.GetAdjustedWeeklyWageAsync
            (requestData.ShiftPattern, requestData.UnpaidPeriodFrom, requestData.UnpaidPeriodTo,
                requestData.ApClaimAmount);

            //Assert
            result.Should().Be(68.181818181818181818181818182M);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetAdjustedWeeklyWage_Returns_AdjustedWeeklyWage_When_ArrearsOfPayClaimAmountIsZero()
        {
            //Arrange
            var requestData = ArrearsOfPayTestsDataGenerator.GetValidRequestData();
            var apClaimAmount = 0.0m;
            var adjustedPeriodFrom =
                await requestData.UnpaidPeriodFrom.GetAdjustedPeriodFromAsync(requestData.InsolvencyDate);
            var adjustedPeriodTo =
                await requestData.UnpaidPeriodFrom.GetAdjustedPeriodToAsync(requestData.InsolvencyDate,
                    requestData.DismissalDate);

            //Act
            var result = await requestData.WeeklyWage.GetAdjustedWeeklyWageAsync(requestData.ShiftPattern,
                adjustedPeriodFrom, adjustedPeriodTo, apClaimAmount);

            //Assert
            result.Should().Be(requestData.WeeklyWage);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetShiftDaysInClaimPeriodAsync_Returns_CountOfShiftDays_FromGivenPeriod()
        {
            //Arrange
            //Shift Pattern in request data => {"2", "3", "4", "5", "6"}
            //claim period => 19/12/2016 to 03/01/2017
            var requestData = ArrearsOfPayTestsDataGenerator.GetValidRequestData();
            //check count of first day in shift pattern in this instance 2 (Tuesday) in the claim period
            //result should be [3] Tuesdays = >20/12/2016, 27/12/2017, 03/01/2017
            var shiftDay = Convert.ToInt32(requestData.ShiftPattern[0]);
            var adjustedClaimPeriodFrom =
                await requestData.UnpaidPeriodFrom.GetAdjustedPeriodFromAsync(requestData.InsolvencyDate);
            var adjustedClaimPeriodTo =
                await requestData.UnpaidPeriodTo.GetAdjustedPeriodToAsync(requestData.InsolvencyDate,
                    requestData.DismissalDate);

            //Act
            var result = await adjustedClaimPeriodFrom.GetShiftDaysInClaimPeriodAsync(adjustedClaimPeriodTo, shiftDay);

            //Assert
            result.Should().Be(3);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetPreferentialClaimAsync_Returns_PreferentialClaim_UsingTotalAPPaid_FromGivenInputs()
        {
            //Arrange
            var totalAPPaid = 500m;
            var preferentialClaim = 800m;
            var apClaimAmount = 500m;

            //Act
            var result = await totalAPPaid.GetPreferentialClaimAsync(apClaimAmount, preferentialClaim);

            //Assert
            result.Should().Be(500);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetPreferentialClaimAsync_Returns_PreferentialClaim_UsingPreferentialLimit_FromGivenInputs()
        {
            //Arrange
            var totalAPPaid = 3502.55m;
            var preferentialClaim = 800m;
            var apClaimAmount = 5000m;

            //Act
            var result = await totalAPPaid.GetPreferentialClaimAsync(apClaimAmount, preferentialClaim);

            //Assert
            result.Should().Be(560.408m);
        }
    }
}