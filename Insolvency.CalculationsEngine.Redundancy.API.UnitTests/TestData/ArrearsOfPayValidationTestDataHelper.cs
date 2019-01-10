using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class ArrearsOfPayValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithNullInputSource(),
                "'Input Source' is not valid, correct values are 'rp1' or 'rp14a'"
            };

            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithEmptyInputSource(),
                "'Input Source' is not valid, correct values are 'rp1' or 'rp14a'"
            };

            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidInputSource(),
                "'Input Source' is not valid, correct values are 'rp1' or 'rp14a'"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidInsolvencyDate(),
                "'Insolvency Date' is not provided or it is an invalid date"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidEmploymentStartDate(),
                "'Employment Start Date' is not provided or it is an invalid date"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidDismissalDate(),
                "'Dismissal Date' is not provided or it is an invalid date"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithEmploymentStartDateAfterDismissalDate(),
                "'Dismissal Date' can not be before the 'Employment Start Date'"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidUnpaidPeriodFrom(),
                "'Unpaid Period From' Date is not provided or it is an invalid date"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithUnpaidPeriodFromAfterDismissalDate(),
                "'Unpaid Period From' Date can not be after the 'Dismissal Date'"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidUnpaidPeriodTo(),
                "'Unpaid Period To' Date is not provided or it is an invalid date"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidDateNoticeGiven(),
                "'Date Notice Given' is not provided or it is an invalid date"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithUnpaidPeriodFromAfterInsolvencyDate(),
                "'Unpaid Period From' Date can not be after the Insolvency Date"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithDateNoticeGivenAfterDismissalDate(),
                "'Date Notice Given' can not be after the 'Dismissal Date'"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithDateNoticeGivenBeforeEmploymentStartDate(),
                "'Date Notice Given' can not be before 'Employment Start Date'"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithUnpaidPeriodFromAfterUnpaidPeriodTo(),
                "'Unpaid Period From' Date can not be after the 'Unpaid Period To'"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithUnpaidPeriodToMoreThan7MOnthsAfterUnpaidPeriodFrom(),
                "'Unpaid Period To' Date can not be moe than 7 months after the 'Unpaid Period From'"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithZeroWeeklyWage(),
                "'Weekly wage' is invalid; value must not be 0 or negative"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithNegativeWeeklyWage(),
                "'Weekly wage' is invalid; value must not be 0 or negative"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithNullShiftPattern(),
                "Invalid shift pattern  correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidShiftPattern(),
                "Invalid shift pattern  correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithNullPayDay(),
                "'Pay day' is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]"
            };
            yield return new object[] {
                ArrearsOfPayTestsDataGenerator.GetRequestWithInvalidPayDay(),
                "'Pay day' is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}



