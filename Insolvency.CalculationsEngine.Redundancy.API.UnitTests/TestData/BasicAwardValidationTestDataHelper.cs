using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class BasicAwardValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "Bad payload" };
            yield return new object[] {
                BasicAwardControllerTestsDataGenerator.GetRequestWithNegativeBasicAwardAmount(),
                "'Basic Award Amount' is invalid; value must not be negative" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}



