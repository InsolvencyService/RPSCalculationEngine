using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class NoticeWorkedNotPaidValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                NoticeWorkedNotPaidControllerTestsDataGenerator.GetBadRequest(),
                "'Unpaid Period From' Date cannot be after the 'Unpaid Period To'" };
            yield return new object[] {
                NoticeWorkedNotPaidControllerTestsDataGenerator.GetRequestWithNullInputSource(),
                "Input Source is not valid, correct values are 'rp1' or 'rp14a'"
            };

            yield return new object[] {
                NoticeWorkedNotPaidControllerTestsDataGenerator.GetRequestWithEmptyInputSource(),
                "Input Source is not valid, correct values are 'rp1' or 'rp14a'"
            };

            yield return new object[] {
                NoticeWorkedNotPaidControllerTestsDataGenerator.GetRequestWithInvalidInputSource(),
                "Input Source is not valid, correct values are 'rp1' or 'rp14a'"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}


