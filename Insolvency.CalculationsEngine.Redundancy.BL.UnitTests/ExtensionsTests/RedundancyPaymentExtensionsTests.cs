using System;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.APPA.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.RedundancyPaymentCalculation.Extensions;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ExtensionsTests
{
    public class RedundancyPaymentExtensionsTests
    {
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetAdjustedEmploymentStartDate_Returns_AdjustedEmployementStartDate_Given_TotalDaysLost_IsZero()
        {
            //Arrange
            var employmentStartDate = new DateTime(2016, 02, 01);
            var totalDaysLost = 0;

            //Act
            var result = await employmentStartDate.GetAdjustedEmploymentStartDate(totalDaysLost);

            //Assert
            var expectedDate = employmentStartDate; //As totalDaysLost is Zero
            result.Should().Be(expectedDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetAdjustedEmploymentStartDate_Returns_AdjustedEmployementStartDate_Given_TotalDaysLost_HasValue()
        {
            //Arrange
            var employmentStartDate = new DateTime(2012, 01, 02);
            var totalDaysLost = 7;

            //Act
            var result = await employmentStartDate.GetAdjustedEmploymentStartDate(totalDaysLost);

            //Assert
            var expectedDate = new DateTime(2012, 01, 09);
            result.Should().Be(expectedDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNoticeEntitlementWeeks_Returns_NoticeWeeks_Equal_12_When_YearsOFServiceIsMoreThan12()
        {
            //Arrange
            var empStartDate = new DateTime(2004, 10, 06);
            var relNoticeDate = new DateTime(2018, 01, 05);
            
            //Act
            var result = await empStartDate.GetNoticeEntitlementWeeks(relNoticeDate);

            //Assert
            //yearsOfService = 13
            result.Should().Be(12);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNoticeEntitlementWeeks_Returns_NoticeWeeks_When_YearsOFServiceIsLessThan12()
        {
            //Arrange
            var empStartDate = new DateTime(2007, 10, 06);
            var relNoticeDate = new DateTime(2018, 01, 05);

            //Act
            var result = await empStartDate.GetNoticeEntitlementWeeks(relNoticeDate);

            //Assert
            //yearsOfService = 10
            result.Should().Be(10);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetRelevantDismissalDate_Returns_DismissalDate()
        {
            //Arrange
            var dismissalDate = new DateTime(2018, 01, 05);
            var projectedNoticeDate = new DateTime(2018, 01, 04);

            //Act
            var result = await dismissalDate.GetRelevantDismissalDate(projectedNoticeDate);

            //Assert
            result.Should().Be(dismissalDate);
        }
        
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceOver41_Returns_Zero_WhenServiceEndedBefore41()
        {
            //Arrange
            var startDate = new DateTime(2004, 11, 01);
            var dismissalDate = new DateTime(2014, 03, 28);
            var dob = new DateTime(1974, 05, 29);
            //Act
            var result = await dob.GetYearsOfServiceOver41(startDate, dismissalDate);

            //Assert
            result.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceOver41_Returns_years_WhenServiceStartedBefore41()
        {
            //Arrange
            var startDate = new DateTime(1993, 01, 01);
            var dismissalDate = new DateTime(2017, 09, 19);
            var dob = new DateTime(1966, 07, 28);
            //Act
            var result = await dob.GetYearsOfServiceOver41(startDate, dismissalDate);

            //Assert
            result.Should().Be(10);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceOver41_Returns_Years_WhenServiceStartedAfter41()
        {
            //Arrange
            var startDate = new DateTime(2001, 01, 01);
            var dismissalDate = new DateTime(2017, 06, 23);
            var dob = new DateTime(1949, 04, 10);
            //Act
            var result = await dob.GetYearsOfServiceOver41(startDate, dismissalDate);

            //Assert
            result.Should().Be(16);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceFrom22To41_Returns_Zero_WhenServiceEndedBefore22()
        {
            //Arrange
            var startDate = new DateTime(2013, 10, 11);
            var dismissalDate = new DateTime(2016, 08, 05);
            var dob = new DateTime(1995, 03, 11);
            //Act
            var result = await dob.GetYearsOfServiceFrom22To41(startDate, dismissalDate);

            //Assert
            result.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceFrom22To41_Returns_Zero_WhenServiceStartedAfter41()
        {
            //Arrange
            var startDate = new DateTime(2001, 01, 01);
            var dismissalDate = new DateTime(2017, 03, 31);
            var dob = new DateTime(1949, 04, 10);
            //Act
            var result = await dob.GetYearsOfServiceFrom22To41(startDate, dismissalDate);

            //Assert
            result.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceFrom22To41_Returns_Year_WhenServiceStartedAfter22AndEndedBefore41()
        {
            //Arrange
            var startDate = new DateTime(2004, 11, 01);
            var dismissalDate = new DateTime(2014, 03, 28);
            var dob = new DateTime(1974, 05, 29);
            //Act
            var result = await dob.GetYearsOfServiceFrom22To41(startDate, dismissalDate);

            //Assert
            result.Should().Be(9);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceFrom22To41_Returns_Year_WhenServiceStartedBefore22AndEndedBefore41()
        {
            //Arrange
            var startDate = new DateTime(2000, 01, 10);
            var dismissalDate = new DateTime(2015, 01, 26);
            var dob = new DateTime(1981, 10, 20);
            //Act
            var result = await dob.GetYearsOfServiceFrom22To41(startDate, dismissalDate);

            //Assert
            result.Should().Be(11);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetYearsOfServiceFrom22To41_Returns_Year_WhenServiceStartedAfter22AndEndedAfter41()
        {
            //Arrange
            var startDate = new DateTime(1993, 01, 01);
            var dismissalDate = new DateTime(2017, 09, 19);
            var dob = new DateTime(1966, 07, 28);

            //Act
            var result = await dob.GetYearsOfServiceFrom22To41(startDate, dismissalDate);

            //Assert
            result.Should().Be(15);
        }
    }
}