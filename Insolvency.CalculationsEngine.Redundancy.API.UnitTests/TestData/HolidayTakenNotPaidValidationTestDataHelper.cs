using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class HolidayTakenNotPaidValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithNullInputSource(),
                "'Input Source' is not valid, correct values are 'rp1' or 'rp14a'"
            };

            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithEmptyInputSource(),
                "'Input Source' is not valid, correct values are 'rp1' or 'rp14a'"
            };

            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithInvalidInputSource(),
                "'Input Source' is not valid, correct values are 'rp1' or 'rp14a'"
            };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithInvalidInsolvencyDate(),
                "'Insolvency Date' is not provided or it is an invalid date" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithInvalidDismissalDate(),
                "'Dismissal Date' is not provided or it is an invalid date" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithInvalidUnpaidPeriodFrom(),
                "'Unpaid Period From' Date is not provided or it is invalid" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithInvalidUnpaidPeriodTo(),
                "'Unpaid Period To' Date is not provided or it is invalid" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithUnpaidFromDateAfterDismissalDate(),
                "'Unpaid Period From' Date can not be after the 'Dismissal Date'" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithUnpaidFromDateAfterInsolvencyDate(),
                "'Unpaid Period From' Date can not be after the 'Insolvency Date'" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithUnpaidToDateAfterDismissalDate(),
                "'Unpaid Period To' Date can not be after the 'Dismissal Date'" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithUnpaidToDateAfterInsolvencyDate(),
                "'Unpaid Period To' Date can not be after the 'Insolvency Date'" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithUnpaidToDateBeforeUnpaidFromDate(),
                "'Unpaid Period From' Date cannot be after the 'Unpaid Period To'" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithZeroWeeklyWage(),
                "'Weekly Wage' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithNegativeWeeklyWage(),
                "'Weekly Wage' is invalid; value must not be 0 or negative" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithNullShiftPattern(),
                "'Shift Pattern' is not provided" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithInvalidShiftPattern(),
                "Invalid 'shift pattern' correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithNullPayDay(),
                "'Pay Day' is not provided" };
            yield return new object[] {
                HolidayTakenNotPaidControllerTestsDataGenerator.GetRequestWithInvalidPayDay(),
                "'Pay Day' is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}


