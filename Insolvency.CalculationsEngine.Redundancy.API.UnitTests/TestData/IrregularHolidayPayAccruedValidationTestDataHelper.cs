using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class IrregularHolidayPayAccruedValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidInsolvencyDate(),
                "Insolvency date is not provided or is not a valid date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithInsolvencyDateBeforeEmpStartDate(),
                "Insolvency date must be greater than the Employee Start date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidEmpStartDate(),
                "Employee start date is not provided or is not a valid date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidDismissalDate(),
                "Dismissal date is not provided or is not a valid date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithDismissalDateBeforeEmpStartDate(),
                "Dismissal date must be greater than the Employee Start date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithDismissalDateMoreThan1YearPriorToInsolvencyDate(),
                "Dismissal date must be no more than a year prior to the insolvency date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStartBefore1stApril2024(),
                "Irregular Hours worker who’s Holiday Year Start Date Core can not be before 1st April 2024." };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullContractedHolEntitlement(),
                "Contracted Holiday Entitlement is not provided" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeContractedHolEntitlement(),
                "Contracted Hoiday Entitlement must be greated than or equal to 0" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidHolidayYearStart(),
                "Holiday year start date is not provided or is not a valid date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStartAfterDismissalDate(),
                "Holiday year start date cannot be after DismissalDate date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStartAfterInsolvencyDate(),
                "Holiday year start date cannot be after Insolvency date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStart12MonthsBeforeDismissalDate(),
                "Holiday year start date must be no more than a year prior to the dismissal date/insolvency date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithHolidayYearStart12MonthsBeforeInsolvencyDate(),
                "Holiday year start date must be no more than a year prior to the dismissal date/insolvency date" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullIsTaxable(),
                "IsTaxable value is not provided or is not valid ('true' or 'false')" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullPayDay(),
                "Pay day is not provided" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidPayDay(),
                "Pay day is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullShiftPattern(),
                "Shift pattern is not provided" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithInvalidShiftPattern(),
                "Invalid shift pattern correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullWeeklyWage(),
                "Weekly wage is not provided" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeWeeklyWage(),
                "Weekly wage is invalid; value must not be 0 or negative" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullDaysCFwd(),
                "Days carried forward is not provided" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeDaysCFwd(),
                "Days carried forward must be 0 or greater" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullDaysTaken(),
                "Days taken is not provided" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeDaysTaken(),
                "Days taken must be 0 or greater" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithBlankHolidaysCarriedOverCoreSource(),
                "Holidays Carried Over Core Source is not provided" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNegativeHolidayAccuredCoreDays(),
                "Holiday Accrued Days Core must be greater than or equal to 0" };
            yield return new object[] {
                IrregularHolidayPayAccruedTestsDataGenerator.GetRequestWithNullHolidayAccuredCoreDays(),
                "Holiday Accrued Days Core is not provided" };


        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
