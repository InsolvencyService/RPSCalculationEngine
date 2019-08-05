using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.Common.UnitTests.ExtensionsTests
{
    public class ExtensionsTests
    {
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task
            GetAdjustedPeriodToAsync_ReturnsAdjustedPeriodToSameAsDismissalDate_When_UnpaidPeriodToGreaterThanDismissal()
        {
            //Arrange
            var insolvencyDate = new DateTime(2017, 07, 30);
            var unpaidPeriodTo = new DateTime(2017, 06, 30);
            var dismissalDateTime = new DateTime(2017, 05, 31);

            //Act
            var result = await unpaidPeriodTo.GetAdjustedPeriodToAsync(insolvencyDate, dismissalDateTime);

            //Assert
            result.Should().Be(dismissalDateTime);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task
            GetAdjustedPeriodToAsync_ReturnsAdjustedPeriodToSameAsInsolvencyDate_When_UnpaidPeriodToGreaterThanInsolvencyDate()
        {
            //Arrange
            var insolvencyDate = new DateTime(2017, 03, 22);
            var unpaidPeriodTo = new DateTime(2017, 05, 31);
            var dismissalDateTime = new DateTime(2017, 05, 31);

            //Act
            var result = await unpaidPeriodTo.GetAdjustedPeriodToAsync(insolvencyDate, dismissalDateTime);

            //Assert
            result.Should().Be(insolvencyDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetEnumValueAsync_ReturnsDayOfWeekEnumValue()
        {
            //Sunday = 0,Monday = 1,Tuesday = 2,Wednesday = 3,Thursday = 4,Friday = 5,Saturday = 6

            //Arrange
            int? dayValue = 3;
            const DayOfWeek expectedResult = DayOfWeek.Wednesday;

            //Act
            var result = await dayValue.GetEnumValueAsync();

            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetFourMonthDateAsync_ReturnsCorrectFourMonthDate()
        {
            //Arrange
            var insolvencyDate = new DateTime(2018, 05, 01);
            var expectedFourMonthDate = new DateTime(2018, 01, 01);

            //Act
            var result = await insolvencyDate.GetFourMonthDateAsync();

            //Assert
            result.Should().Be(expectedFourMonthDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetServiceYearsAsync_Returns_WholeYearsInService()
        {
            //Arrange
            var empStartDate = new DateTime(2015, 10, 06);
            var dismissalDate = new DateTime(2018, 08, 31);
            var expectedResultYearsInService = 2;

            //Act
            var result = await empStartDate.GetServiceYearsAsync(dismissalDate);

            //Asset
            result.Should().Be(expectedResultYearsInService);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetServiceYearsAsync_Returns_ZeroYearsService()
        {
            //Arrange
            var firstDate = new DateTime(2015, 10, 06);
            var secondDate = new DateTime(2015, 10, 05);
            var expectedResultYearsInService = 0;

            //Act
            var result = await firstDate.GetServiceYearsAsync(secondDate);

            //Asset
            result.Should().Be(expectedResultYearsInService);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetWeekDatesAsync_Returns_WeekDates_In_Reverse_StaringFromGivenPayDate()
        {
            //Weekend date 06/05/2018  (-6)days => 30/04/2018
            //week dates =>30/04/2018....01,02,03,04,05.....06/05/2018

            //Arrange
            var weekEndDate = new DateTime(2018, 05, 06);
            var weekStartDate = weekEndDate.AddDays(-6);

            //Act
            var result = await weekEndDate.GetWeekDatesAsync();

            //Assert
            result.Count.Should().Be(7);
            result[0].Date.Should().Be(weekStartDate.Date);
            result[6].Date.Should().Be(weekEndDate.Date);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetRelevantNoticeDate_Returns_NoticeDate()
        {
            //Arrange
            var dismissalDate = new DateTime(2018, 01, 07);
            var noticeDate = new DateTime(2018, 01, 05);

            //Act
            var result = await noticeDate.GetRelevantNoticeDate(dismissalDate);

            //Assert
            Assert.Equal(result, noticeDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetRelevantNoticeDate_Returns_DismissalDate()
        {
            //Arrange
            var dismissalDate = new DateTime(2017, 08, 31);
            var noticeDate = new DateTime(2018, 01, 05);

            //Act
            var result = await noticeDate.GetRelevantNoticeDate(dismissalDate);

            //Assert
            Assert.Equal(result, dismissalDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetRelevantNoticeDate_When_BothDatesAreSame()
        {
            //Arrange
            var dismissalDate = new DateTime(2018, 06, 08);
            var noticeDate = new DateTime(2018, 06, 08);

            //Act
            var result = await noticeDate.GetRelevantNoticeDate(dismissalDate);

            //Assert
            Assert.Equal(result, dismissalDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetProjectedNoticeDate_Equals_RelNoticeDate_When_NoticeWeeks_Zero()
        {
            //Arrange
            var relNoticeDate = new DateTime(2018, 01, 05);
            var noticeWeeks = 0;

            //Act
            var result = await relNoticeDate.GetProjectedNoticeDate(noticeWeeks);

            //Assert
            Assert.Equal(result, relNoticeDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetProjectedNoticeDate_When_NoticeWeeks_NotZero()
        {
            //Arrange
            var relNoticeDate = new DateTime(2018, 01, 05);
            var noticeWeeks = 10;

            //Act
            var result = await relNoticeDate.GetProjectedNoticeDate(noticeWeeks);

            //Assert
            var expectedDate = new DateTime(2018, 03, 16);
            Assert.Equal(result, expectedDate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetTaxDeducted_Returns_Value_WhenGrossEntitlementIsValidDecimal()
        {
            //Arrange
            var grossEntitlement = 500m;
            var taxRate = 0.20m;
            var isTaxable = true;

            //Act
            var result = await grossEntitlement.GetTaxDeducted(taxRate, isTaxable);

            //Assert
            var expectedTaxDeducated = 100m;
            result.Should().Be(expectedTaxDeducated);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetTaxDeducted_Returns_Zero_WhenIsTaxableIsFalse()
        {
            //Arrange
            var grossEntitlement = 500m;
            var taxRate = 0.20m;
            var isTaxable = false;

            //Act
            var result = await grossEntitlement.GetTaxDeducted(taxRate, isTaxable);

            //Assert
            var expectedTaxDeducated = 0m;
            result.Should().Be(expectedTaxDeducated);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetTaxDeducted_Returns_Zero_WhenGrossEntitlementIsZero()
        {
            //Arrange
            var grossEntitlement = 0m;
            var taxRate = 0.20m;
            var isTaxable = true;

            //Act
            var result = await grossEntitlement.GetTaxDeducted(taxRate, isTaxable);

            //Assert
            var expectedTaxDeducated = 0m;
            result.Should().Be(expectedTaxDeducated);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNIDeducted_Returns_Value_WhenGrossEntitlementIsGreaterThenUpperThreshold()
        {
            //Arrange
            var grossEntitlement = 1000m;
            var niThreshold = 162m;
            var niUpperThreshold = 892m;
            var niRate = 0.12m;
            var niUpperRate = 0.02m;
            var isTaxable = true;

            //Act
            var result = await grossEntitlement.GetNIDeducted(niThreshold, niUpperThreshold, niRate, niUpperRate, isTaxable);

            //Assert
            var expectedNiDeducated = 89.76m;
            result.Should().Be(expectedNiDeducated);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNIDeducted_Returns_Value_WhenGrossEntitlementIsBetweenUpperAndLowerThreshold()
        {
            //Arrange
            var grossEntitlement = 500m;
            var niThreshold = 162m;
            var niUpperThreshold = 892m;
            var niRate = 0.12m;
            var niUpperRate = 0.02m;
            var isTaxable = true;

            //Act
            var result = await grossEntitlement.GetNIDeducted(niThreshold, niUpperThreshold, niRate, niUpperRate, isTaxable);

            //Assert
            var expectedNiDeducated = 40.56m;
            result.Should().Be(expectedNiDeducated);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNIDeducted_Returns_Value_WhenGrossEntitlementIsLessThenThreshold()
        {
            //Arrange
            var grossEntitlement = 100m;
            var niThreshold = 162m;
            var niUpperThreshold = 892m;
            var niRate = 0.12m;
            var niUpperRate = 0.02m;
            var isTaxable = true;

            //Act
            var result = await grossEntitlement.GetNIDeducted(niThreshold, niUpperThreshold, niRate, niUpperRate, isTaxable);

            //Assert
            result.Should().Be(0);
        }


        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNIDeducted_Returns_Zero_WhenIsTaxableIsFalse()
        {
            //Arrange
            var grossEntitlement = 100m;
            var niThreshold = 162m;
            var niUpperThreshold = 892m;
            var niRate = 0.12m;
            var niUpperRate = 0.02m;
            var isTaxable = false;

            //Act
            var result = await grossEntitlement.GetNIDeducted(niThreshold, niUpperThreshold, niRate, niUpperRate, isTaxable);

            //Assert
            result.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNetLiability_Returns_Value_WhenValidInputsProvided()
        {
            //Arrange
            var grossEntitlement = 479m;
            var taxDeducted = 95.8m;
            var niDeducted = 38.04m;

            //Act
            var result = await grossEntitlement.GetNetLiability(taxDeducted, niDeducted);

            //Assert
            var expectedNetLiability = 345.16m;
            result.Should().Be(expectedNetLiability);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetStatutoryHolidayEntitlement_Returns_StatutoryHolidayEntitlementForFiveDayWeek()
        {
            // Arrange
            var shiftPatten = new List<string>() { "1", "2", "3", "4", "5" } ;
            var statHolidayEntitlement = 0m;

            var expectedStatHolidayEntilement = 28m;

            // Act
            var result = await statHolidayEntitlement.GetStatutoryHolidayEntitlement(shiftPatten);

            // Assert
            result.Should().Be(expectedStatHolidayEntilement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetStatutoryHolidayEntitlement_Returns_StatutoryHolidayEntitlementForThreeDayWeek()
        {
            // Arrange
            var shiftPatten = new List<string>() { "1", "2", "3" };
            var statHolidayEntitlement = 0m;

            var expectedStatHolidayEntilement = 16.80m;

            // Act
            var result = await statHolidayEntitlement.GetStatutoryHolidayEntitlement(shiftPatten);

            // Assert
            result.Should().Be(expectedStatHolidayEntilement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetBusinessDaysInRange_ReturnsNumBusinessDays()
        {
            // Arrange
            var shiftPatten = new List<string>() { "1", "2", "3" };
            var startDate = new DateTime(2018, 08, 26);
            var endDate = new DateTime(2018, 08, 30);

            // Act
            var result = await startDate.GetNumBusinessDaysInRange(endDate, shiftPatten);

            // Assert
            result.Should().Be(3);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetShiftDayNames_ReturnsDaysName()
        {
            // Arrange
            var shiftPatten = new List<string>() { "1", "2", "3" };

            // Act
            var result = await shiftPatten.GetShiftDayNames();

            // Assert
            result.Should().Be("MondayTuesdayWednesday");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetDaysInRange_ReturnsDays()
        {
            // Arrange
            var startDate = new DateTime(2018, 08, 12);
            var endDate = new DateTime(2018, 08, 30);

            // Act
            var result = await startDate.GetDaysInRange(endDate, DayOfWeek.Monday);

            // Assert
            result.Count.Should().Be(3);
            result[0].Should().Be(new DateTime(2018, 08, 13));
            result[1].Should().Be(new DateTime(2018, 08, 20));
            result[2].Should().Be(new DateTime(2018, 08, 27));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetDailyAmount_ReturnsDailyAmount()
        {
            // Arrange
            var startDate = new DateTime(2018, 08, 12);
            var endDate = new DateTime(2018, 08, 21);
            decimal amount = 100m;

            // Act
            var result = await amount.GetDailyAmount(startDate, endDate);

            // Assert
            result.Should().Be(10m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetShiftDayNames_WhenShiftPatternIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            var shiftPattern = new List<string>();

            // Act
            var result = await shiftPattern.GetShiftDayNames();

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetShiftDayNames_ReturnsConcatenatedDayNames()
        {
            // Arrange
            var shiftPattern = new List<string>()
            {  "1", "2", "3" };

            // Act
            var result = await shiftPattern.GetShiftDayNames();

            // Assert
            result.Should().Be("MondayTuesdayWednesday");
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ContainsDayWeek_ReturnsTrue_IfDayIsContained()
        {
            // Arrange
            var shiftPattern = new List<string>()
            {  "1", "2", "3" };

            // Act
            var result = await shiftPattern.ContainsDayWeek(DayOfWeek.Monday);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ContainsDayWeek_ReturnsFalse_IfDayIsNotContained()
        {
            // Arrange
            var shiftPattern = new List<string>()
            {  "1", "2", "3" };

            // Act
            var result = await shiftPattern.ContainsDayWeek(DayOfWeek.Thursday);

            // Assert
            result.Should().BeFalse();
        }

       
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetTaxYear_ReturnsStartYear_WhenBefore6thApril()
        {
            // Arrange
            var date = new DateTime(2018, 4, 5);

            // Act
            var result = await date.GetTaxYear();

            // Assert
            result.Should().Be(2017);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetTaxYear_ReturnsStartYear_WhenAfter6thApril()
        {
            // Arrange
            var date = new DateTime(2018, 4, 6);

            // Act
            var result = await date.GetTaxYear();

            // Assert
            result.Should().Be(2018);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetEarnings_ReturnsEartningsForWholeWeeks()
        {
            // Arrange
            var start = new DateTime(2018, 9, 3);
            var end = new DateTime(2018, 9, 23);
            var weeklyWage = 140m;

            // Act
            var result = await weeklyWage.GetEarnings(start, end);

            // Assert
            result.Should().Be(420m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetEarnings_ReturnsEartningsForWholePartialWeeks()
        {
            // Arrange
            var start = new DateTime(2018, 9, 3);
            var end = new DateTime(2018, 9, 26);
            var weeklyWage = 140m;

            // Act
            var result = await weeklyWage.GetEarnings(start, end);

            // Assert
            result.Should().Be(480m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetEarnings_ReturnsEartningsForWholePartialWeeksExceedingWeeklyWage()
        {
            // Arrange
            var start = new DateTime(2018, 9, 3);
            var end = new DateTime(2018, 9, 29);
            var weeklyWage = 140m;

            // Act
            var result = await weeklyWage.GetEarnings(start, end);

            // Assert
            result.Should().Be(540m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task IsEmploymentDay_Returns_TrueIfDayInShiftPattern()
        {
            // Arrange
            var date = new DateTime(2018, 10, 17);
            var shiftPattern = new List<string>()
            {  "1", "2", "3" };

            // Act
            var result = await date.IsEmploymentDay(shiftPattern);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task IsEmploymentDay_Returns_FalseIfDayNotInShiftPattern()
        {
            // Arrange
            var date = new DateTime(2018, 10, 18);
            var shiftPattern = new List<string>()
            {  "1", "2", "3" };

            // Act
            var result = await date.IsEmploymentDay(shiftPattern);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task AddEmploymentDays_AddsOnlyEmploymentDays()
        {
            // Arrange
            var date = new DateTime(2018, 10, 10);
            var shiftPattern = new List<string>()
            {  "1", "2", "3", "4", "5" };

            // Act
            var result = await date.AddEmploymentDays(3, shiftPattern);

            // Assert
            result.Should().Be(new DateTime(2018, 10, 15));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetPayDay_ReturnsNextPayDay_WhenPayDayInFuture()
        {
            // Arrange
            var date = new DateTime(2018, 10, 17);
            var payDay = DayOfWeek.Saturday;

            // Act
            var result = await date.GetPayDay(payDay);

            // Assert
            result.Should().Be(new DateTime(2018, 10, 20));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetPayDay_ReturnsNextPayDay_WhenPayDayIsDate()
        {
            // Arrange
            var date = new DateTime(2018, 10, 20);
            var payDay = DayOfWeek.Saturday;

            // Act
            var result = await date.GetPayDay(payDay);

            // Assert
            result.Should().Be(new DateTime(2018, 10, 20));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNumDaysInIntersectionOfTwoRanges_Returns0WhenRange2IsBeforeRange1()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 9, 1);
            var endDate2 = new DateTime(2018, 9, 10);

            // Act
            var result = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNumDaysInIntersectionOfTwoRanges_ReturnsValueWhenRange2OverlapsRange1Start()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 9, 25);
            var endDate2 = new DateTime(2018, 10, 3);

            // Act
            var result = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2);

            // Assert
            result.Should().Be(3);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNumDaysInIntersectionOfTwoRanges_ReturnsValueWhenRange2InsideRange1()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 2);
            var endDate2 = new DateTime(2018, 10, 5);

            // Act
            var result = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2);

            // Assert
            result.Should().Be(4);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNumDaysInIntersectionOfTwoRanges_ReturnsValueWhenRange2OverlapsRange1End()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 5);
            var endDate2 = new DateTime(2018, 10, 10);

            // Act
            var result = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2);

            // Assert
            result.Should().Be(3);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNumDaysInIntersectionOfTwoRanges_Returns0WhenRange2AfterRange1()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 9);
            var endDate2 = new DateTime(2018, 10, 15);

            // Act
            var result = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNumDaysInIntersectionOfTwoRanges_Returns2WhenDateNoticeGivenLessThanEndDate()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 5);
            var endDate2 = new DateTime(2018, 10, 10);
            var dateNoticeGiven = new DateTime(2018, 10, 6);

            // Act
            var result = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2, dateNoticeGiven);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetNumDaysInIntersectionOfTwoRanges_Returns3WhenDateNoticeGivenLessThanStartDate()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 5);
            var endDate2 = new DateTime(2018, 10, 10);
            var dateNoticeGiven = new DateTime(2018, 10, 4);

            // Act
            var result = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2, dateNoticeGiven);

            // Assert
            result.Should().Be(3);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task DoRangesIntersect_ReturnsFalseWhenRange2BeforeRange1()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 9, 1);
            var endDate2 = new DateTime(2018, 9, 10);

            // Act
            var result = await startDate1.DoRangesIntersect(endDate1, startDate2, endDate2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task DoRangesIntersect_ReturnsTrueWhenRange2OverlapsRange1Start()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 9, 25);
            var endDate2 = new DateTime(2018, 10, 3);

            // Act
            var result = await startDate1.DoRangesIntersect(endDate1, startDate2, endDate2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task DoRangesIntersect_ReturnsTrueWhenRange2InsideRange1()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 2);
            var endDate2 = new DateTime(2018, 10, 5);

            // Act
            var result = await startDate1.DoRangesIntersect(endDate1, startDate2, endDate2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task DoRangesIntersect_ReturnsTrueWhenRange1InsideRange2()
        {
            // Arrange
            var startDate2 = new DateTime(2018, 10, 1);
            var endDate2 = new DateTime(2018, 10, 7);
            var startDate1 = new DateTime(2018, 10, 2);
            var endDate1 = new DateTime(2018, 10, 5);

            // Act
            var result = await startDate1.DoRangesIntersect(endDate1, startDate2, endDate2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task DoRangesIntersect_ReturnsTrueWhenRange2OverlapsRange1End()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 5);
            var endDate2 = new DateTime(2018, 10, 10);

            // Act
            var result = await startDate1.DoRangesIntersect(endDate1, startDate2, endDate2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task DoRangesIntersect_ReturnsFalseRange2AfterRange1()
        {
            // Arrange
            var startDate1 = new DateTime(2018, 10, 1);
            var endDate1 = new DateTime(2018, 10, 7);
            var startDate2 = new DateTime(2018, 10, 9);
            var endDate2 = new DateTime(2018, 10, 15);

            // Act
            var result = await startDate1.DoRangesIntersect(endDate1, startDate2, endDate2);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetAge_ReturnsAgeIfHadBirthday()
        {
            // Arrange
            var dob = new DateTime(1993, 3, 27);
            var now = new DateTime(2018, 3, 28);

            // Act
            var result = await dob.GetAge(now);

            // Assert
            result.Should().Be(25);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetAge_ReturnsAgeIfNotHadBirthday()
        {
            // Arrange
            var dob = new DateTime(1993, 3, 27);
            var now = new DateTime(2018, 3, 26);

            // Act
            var result = await dob.GetAge(now);

            // Assert
            result.Should().Be(24);
        }
    }
}
