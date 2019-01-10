using System.Collections;
using System.Collections.Generic;
namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public class ApportionmentValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { ApportionmentControllerTestsDataGenerator.GetBadPayload(), "Bad payload" };
            yield return new object[] {
                ApportionmentControllerTestsDataGenerator.GetRequestWithNegativeGrossPaidInFourMonth(),
                "'Gross Paid In Four Months Amount' is invalid" };
            yield return new object[] {
                ApportionmentControllerTestsDataGenerator.GetRequestWithNegativeGrossEntitlement(),
                "'Gross Entitlement Amount' is invalid" };
            yield return new object[] {
                ApportionmentControllerTestsDataGenerator.GetRequestWithNegativeTotalClaimed(),
                "'Total Claimed Amount In Four Month' is invalid" };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
