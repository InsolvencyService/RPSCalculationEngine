using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class HolidayPayAccruedValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidInsolvencyDate(),
                "Insolvency date is not provided or is not a valid date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithInsolvencyDateBeforeEmpStartDate(),
                "Insolvency date must be greater than the Employee Start date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidEmpStartDate(),
                "Employee start date is not provided or is not a valid date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidDismissalDate(),
                "Dismissal date is not provided or is not a valid date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithDismissalDateBeforeEmpStartDate(),
                "Dismissal date must be greater than the Employee Start date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithDismissalDateMoreThan1YearPriorToInsolvencyDate(),
                "Dismissal date must be no more than a year prior to the insolvency date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullContractedHolEntitlement(),
                "Contracted Holiday Entitlement is not provided" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeContractedHolEntitlement(),
                "Contracted Hoiday Entitlement must be greated than or equal to 0" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidHolidayYearStart(),
                "Holiday year start date is not provided or is not a valid date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStartAfterDismissalDate(),
                "Holiday year start date cannot be after DismissalDate date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStartAfterInsolvencyDate(),
                "Holiday year start date cannot be after Insolvency date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStart12MonthsBeforeDismissalDate(),
                "Holiday year start date must be no more than a year prior to the dismissal date/insolvency date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStart12MonthsBeforeInsolvencyDate(),
                "Holiday year start date must be no more than a year prior to the dismissal date/insolvency date" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullIsTaxable(),
                "IsTaxable value is not provided or is not valid ('true' or 'false')" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullPayDay(),
                "Pay day is not provided" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidPayDay(),
                "Pay day is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullShiftPattern(),
                "Shift pattern is not provided" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidShiftPattern(),
                "Invalid shift pattern correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullWeeklyWage(),
                "Weekly wage is not provided" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeWeeklyWage(),
                "Weekly wage is invalid; value must not be 0 or negative" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullDaysCFwd(),
                "Days carried forward is not provided" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeDaysCFwd(),
                "Days carried forward must be 0 or greater" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullDaysTaken(),
                "Days taken is not provided" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeDaysTaken(),
                "Days taken must be 0 or greater" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNullIpConfirmedDays(),
                "Ip Confirmed Days is not provided" };
            yield return new object[] {
                HolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeIpConfirmedDays(),
                "Ip Confirmed Days must be greater than or equal to 0" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}


