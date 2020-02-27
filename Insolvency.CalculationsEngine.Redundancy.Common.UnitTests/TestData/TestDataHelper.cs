using System;
using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.Common.UnitTests.TestData
{
    public class TestDataHelper
    {
        public class StatutoryMaxUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {new DateTime(2015, 10, 23), 475};
                yield return new object[] {new DateTime(2016, 04, 12), 479};
                yield return new object[] {new DateTime(2014, 06, 19), 464};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class BenefitsWaitingDaysUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {new DateTime(2015, 10, 23), 7};
                yield return new object[] {new DateTime(2016, 04, 12), 7};
                yield return new object[] {new DateTime(2014, 06, 19), 7};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class TaxRateUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 0.2m };
                yield return new object[] { new DateTime(2016, 04, 12), 0.2m };
                yield return new object[] { new DateTime(2000, 06, 19), 0.2m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NIRateUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 0.12m };
                yield return new object[] { new DateTime(2016, 04, 12), 0.12m };
                yield return new object[] { new DateTime(2000, 06, 19), 0.12m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NIUpperRateUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 0.02m };
                yield return new object[] { new DateTime(2016, 04, 12), 0.02m };
                yield return new object[] { new DateTime(2000, 06, 19), 0.02m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NIThresholdUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 166m };
                yield return new object[] { new DateTime(2016, 04, 12), 166m };
                yield return new object[] { new DateTime(2000, 06, 19), 166m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NIUpperThresholdUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 962m };
                yield return new object[] { new DateTime(2016, 04, 12), 962m };
                yield return new object[] { new DateTime(2000, 06, 19), 962m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class PreferentialLimitUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 800m };
                yield return new object[] { new DateTime(2016, 04, 12), 800m };
                yield return new object[] { new DateTime(2000, 06, 19), 800m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NotionalBenefitsMonthlyRateUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 24, 251.77m };
                yield return new object[] { new DateTime(2016, 04, 12), 24, 251.77m };
                yield return new object[] { new DateTime(2000, 06, 19), 24, 251.77m };
                yield return new object[] { new DateTime(2015, 10, 23), 25, 317.82m };
                yield return new object[] { new DateTime(2016, 04, 12), 25, 317.82m };
                yield return new object[] { new DateTime(2000, 06, 19), 25, 317.82m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class NotionalBenefitsWeeklyRateUnitTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new DateTime(2015, 10, 23), 24, 57.90m };
                yield return new object[] { new DateTime(2016, 04, 12), 24, 57.90m };
                yield return new object[] { new DateTime(2000, 06, 19), 24, 57.90m };
                yield return new object[] { new DateTime(2015, 10, 23), 25, 73.10m };
                yield return new object[] { new DateTime(2016, 04, 12), 25, 73.10m };
                yield return new object[] { new DateTime(2000, 06, 19), 25, 73.10m };
                yield return new object[] { new DateTime(2020, 03, 31), 24, 57.90m };
                yield return new object[] { new DateTime(2020, 03, 31), 25, 73.10m };
                yield return new object[] { new DateTime(2020, 04, 01), 24, 58.90m };
                yield return new object[] { new DateTime(2020, 04, 01), 25, 74.35m };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}