using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class CompensatoryNoticePayValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //yield return new object[] { null, "Bad payload" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidInsolvencyEmploymentStartDate(),
                "'Insolvency Employment Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInsolvencyEmploymentStartDateBeforeDateOfBirth(),
                "'Insolvency Employment Start Date' cannot be before the Date of Birth" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidInsolvencyDate(),
                "'Insolvency Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidDismissalDate(),
                "'Dismissal Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithDismissalDateBeforeInsolvencyEmploymentStartDate(),
                "'Dismissal Date' must be at least 1 calendar month after the Insolvency Employment Start Date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithDismissalDateLessThat1MonthAfterInsolvencyEmploymentStartDate(),
                "'Dismissal Date' must be at least 1 calendar month after the Insolvency Employment Start Date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithDismissalDateBeforeDateNoticeGiven(),
                "'Dismissal Date' cannot be before the Date Notice Given" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidDateNoticeGiven(),
                "'Date Notice Given' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithDateNoticeGivenBeforeInsolvencyEmploymentStartDate(),
                "'Date Notice Given' cannot be before the Insolvency Employment Start Date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithZeroWeeklyWage(),
                "'Weekly Wage' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNegativeWeeklyWage(),
                "'Weekly Wage' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNullShiftPattern(),
                "Shift pattern is not provided" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidShiftPattern(),
                "Invalid shift pattern correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidDateOfBirth(),
                "'Date Of Birth' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidDeceasedDate(),
                "'Deceased Date' is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithDeceasedDateBeforeDateOfBirth(),
                "'Decreased Date' can not be before the Date of Birth" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidBenefitStartDate(),
                "'Benefit Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithBenefitEndDateBeforeBenefitStartDate(),
                "'Benefit End Date' cannot be before the Benefit Start Date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithZeroBenefitAmount(),
                "'Benefit Amount' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNegativeBenefitAmount(),
                "'Benefit Amount' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidNewEmploymentStartDate(),
                "'New Employment Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNewEmploymentEndDateBeforeNewEmploymentStartDate(),
                "'New Employment End Date' cannot be before the New Employment Start Date" };            
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNegativeNewEmploymentWage(),
                "'New Employment Wage' is invalid; value must not be negative or zero" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithZeroNewEmploymentWage(),
                "'New Employment Wage' is invalid; value must not be negative or zero" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNonZeroNewEmploymentWeeklyWageAndZeroNewEmploymentWage(),
                "'New Employment Weekly Wage' must be zero if New Employment Wage is zero" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNegativeNewEmploymentWeeklyWage(),
                "'New Employment Weekly Wage' is invalid; value must not be negative" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidWageIncreaseStartDate(),
                "'Wage Increase Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithWageIncreaseEndDateBeforeWageIncreaseStartDate(),
                "'Wage Increase End Date' cannot be before the Wage Increase Start Date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithZeroWageIncreaseAmount(),
                "'Wage Increase Amount' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNegativeWageIncreaseAmount(),
                "'Wage Increase Amount' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidNotionalBenefitOverrideStartDate(),
                "'Notional Benefit Override Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithInvalidNotionalBenefitOverrideEndDate(),
                "'Notional Benefit Override End Date' is not provided or it is an invalid date" };
            yield return new object[] {
                CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNotionalBenefitOverrideEndDateBeforeNotionalBenefitOverrideStartDate(),
                "'Notional Benefit Override End Date' cannot be before the Notional Benefit Override Start Date" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}


