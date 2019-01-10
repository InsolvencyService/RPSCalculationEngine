using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class ProtectiveAwardValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidInsolvencyDate(),
                "'Insolvency Date' is not provided or it is an invalid date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithFutureInsolvencyDate(),
                "'Insolvency Date' can not be in the future" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInsolvencyDateBeforeEmployeeStartDate(),
                "'Insolvency Date' can not be before the Employment Start Date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidEmploymentStartDate(),
                "'Employment Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithFutureEmploymentStartDate(),
                "'Employment Start Date' can not be in the future" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidDismissalDate(),
                "'Dismissal Date' is not provided or it is an invalid date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithFutureDismissalDate(),
                "'Dismissal Date' can not be in the future" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithDismissalDateBeforeEmployeeStartDate(),
                "'Dismissal Date' can not be before the Employment Start Date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidTribunalAwardDate(),
                "'Tribunal Award Date' is not provided or it is an invalid date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithFutureTribunalAwardDate(),
                "'Tribunal Award Date' can not be in the future" };
           yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidProtectiveAwardStartDate(),
                "'Protective Award Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithFutureProtectiveAwardStartDate(),
                "'Protective Award Start Date' can not be in the future" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithNullProtectiveAwardDays(),
                "'Protective Award Days' is not provided or it is not in the format of an integer" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithZeroProtectiveAwardDays(),
                "'Protective Award Days' is invalid; value must be 1 or greater" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithProtectiveAwardDaysGreaterThanNinetyDaysLimit(),
                "'Protective Award Days' is invalid; value must be 90 or less" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithNegativeProtectiveAwardDays(),
                "'Protective Award Days' is invalid; value must be 1 or greater" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithZeroWeeklyWage(),
                "'Weekly Wage' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithNegativeWeeklyWage(),
                "'Weekly Wage' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithNullShiftPattern(),
                "Shift pattern is not provided" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidShiftPattern(),
                "Invalid shift pattern correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidBenefitStartDate(),
                "'Benefit Start Date' is not provided or it is an invalid date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithFutureBenefitStartDate(),
                "'Benefit Start Date' can not be in the future" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidBenefitEndDate(),
                "'Benefit End Date' is not provided or it is an invalid date" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithFutureBenefitEndDate(),
                "'Benefit End Date' can not be in the future" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithBenefitStartDateBeforeDimissalDate(),
                "'Benefit Start Date' or 'Benefit End Date' can not be before Date of Dismissal" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithBenefitEndDateBeforeDismissalDate(),
                "'Benefit Start Date' or 'Benefit End Date' can not be before Date of Dismissal" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithZeroBenefitAmount(),
                "'Benefit Amount' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithNegativeBenefitAmount(),
                "'Benefit Amount' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithNullPayDay(),
                "'Pay day' is not provided or it is not in the format of an integer" };
            yield return new object[] {
                ProtectiveAwardControllerTestsDataGenerator.GetRequestWithInvalidPayDay(),
                "'Pay day' is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}



