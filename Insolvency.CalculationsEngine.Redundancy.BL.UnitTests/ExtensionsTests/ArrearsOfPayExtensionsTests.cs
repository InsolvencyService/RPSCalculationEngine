using System;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.APPA.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
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