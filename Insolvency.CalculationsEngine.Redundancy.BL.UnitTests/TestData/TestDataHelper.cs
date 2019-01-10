using System;
using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData
{
    public class TestDataHelper
    {
        public class ProjectedNoticeDateUnitTestData : IEnumerable<object[]>
        {
            //Arrange 
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    {new DateTime(2018, 07, 26), new DateTime(2018, 08, 31), 2, new DateTime(2018, 08, 09)};
                yield return new object[]
                    {new DateTime(2018, 09, 01), new DateTime(2018, 08, 31), 2, new DateTime(2018, 09, 14)};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NotionalBenefitsDaysUnitTestData : IEnumerable<object[]>
        {
            //Arrange 
            public IEnumerator<object[]> GetEnumerator()
            {
                /*
                Example:
                benefitsStartDate = 01 / 08 / 2017
                benefitsEndDate = 06 / 08 / 2017
                dateNoticeGiven = 23 / 07 / 2017
                projectedNoticeDate = 09 / 08 / 2017
                benfitWaitingDays = 7 
                  */
                yield return new object[]
                {
                    new DateTime(2017, 08, 01), new DateTime(2017, 08, 06), new DateTime(2017, 07, 23),
                    new DateTime(2017, 08, 09), 7, 4
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class DisallowedBenefitsWeeksUnitTestData : IEnumerable<object[]>
        {
            //Arrange 
            public IEnumerator<object[]> GetEnumerator()
            {
                /*
                Example:
                   projectedNoticeDate = 09/08/2017
                   benefitsEndDate = 21/08/2017
                   disallowedBenefitsWeeks = 1.71 i.e.(12 days / 7)
                  */
                yield return new object[]
                {
                    new DateTime(2017, 08, 21), new DateTime(2017, 08, 09), 1.71
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class EstimatedDailyBenefitsClaimedUnitTestData : IEnumerable<object[]>
        {
            //Arrange 
            public IEnumerator<object[]> GetEnumerator()
            {
                /*
                Example:
                   benefitsStartDate = 08/08/2017
                   benefitsEndDate = 21/08/2017
                   benefitsClaimed =100.00
                   projectedNoticeDate = 15/08/2017
                   estimatedBenefitsClaimed = 8.9744
                  */
                yield return new object[]
                {
                    new DateTime(2017, 08, 08), new DateTime(2017, 08, 21), 100.00, new DateTime(2017, 08, 15),
                    Math.Round(8.9744, 2)
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NewEmploymentOverlapUnitTestData : IEnumerable<object[]>
        {
            //Arrange 
            public IEnumerator<object[]> GetEnumerator()
            {
                /*
                Example:
                   empStartDate
                   dismissalDate
                  */
                yield return new object[]
                {
                    new DateTime(2017, 08, 08), new DateTime(2017, 08, 21)
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}