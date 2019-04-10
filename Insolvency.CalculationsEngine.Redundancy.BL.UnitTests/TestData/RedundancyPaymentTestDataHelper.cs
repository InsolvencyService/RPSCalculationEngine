using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData
{
    public class RedundancyPaymentTestDataHelper
    {
        public class RedundancyPaymentRequestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    //service under22
                    new RedundancyPaymentCalculationRequestModel(
                        new DateTime(2013,10,11), new DateTime(2016,08,05), new DateTime(2016,08,05),
                        new DateTime(1995,03,11), 203.64m, 0, 0),
                    new RedundancyPaymentResponseDto(new DateTime(2013,10,11), 2 ,new DateTime(2016,08,19), 2, 0, 0, 1, 203.64m, 0, 203.64m, 479),
                };

                yield return new object[]
                {
                    //service from 22 to 41
                   new RedundancyPaymentCalculationRequestModel(
                        new DateTime(2013,02,01), new DateTime(2016,02,29), new DateTime(2016,02,29),
                        new DateTime(1976,05,12), 186.86m, 0, 0),
                    new RedundancyPaymentResponseDto(new DateTime(2013,02,01), 3 ,new DateTime(2016,03,21), 0, 3, 0,3, 560.58m, 0, 560.58m, 475),
                };

                yield return new object[]
                {
                   //service over 41
                   new RedundancyPaymentCalculationRequestModel(
                        new DateTime(1993,09,01), new DateTime(2016,07,13), new DateTime(2016,05,18),
                        new DateTime(1948,07,03), 124.06m, 0, 0),
                    new RedundancyPaymentResponseDto(new DateTime(1993,09,01), 12,new DateTime(2016,08,10), 0, 0, 20, 30, 3721.8m, 0, 3721.8m, 479),
                };

                yield return new object[]
                {
                    //service in differnet age bands
                   new RedundancyPaymentCalculationRequestModel(
                        new DateTime(1993,01,01), new DateTime(2017,09,19), new DateTime(2017,09,19),
                        new DateTime(1966,07,28), 920.54m, 0, 0),
                    new RedundancyPaymentResponseDto(new DateTime(1993,01,01), 12,new DateTime(2017,12,12), 0, 10, 10, 25, 12225m, 0, 12225m, 489),
                };

                yield return new object[]
                {
                    //dismissal date = 21st birthday
                   new RedundancyPaymentCalculationRequestModel(
                    new DateTime(2011,04,14), new DateTime(2016,04,14), new DateTime(2016,02,13),
                    new DateTime(1995,04,14), 257m, 102.35m, 0),
                   new RedundancyPaymentResponseDto(new DateTime(2011,04,14), 4,new DateTime(2016,04,14), 5, 0, 0, 2.5m, 642.5m, 102.35m, 540.15m, 475)
                };

                yield return new object[]
                {
                    //part payment greater than entitlement
                   new RedundancyPaymentCalculationRequestModel(
                    new DateTime(1997,10,1), new DateTime(2018,10,1), new DateTime(2018,10,1),
                    new DateTime(1956,9,1), 300m, 10000m, 0),
                   new RedundancyPaymentResponseDto(new DateTime(1997,10,1), 12, new DateTime(2018,12,24), 0, 0, 20, 30m, 9000m, 10000m, 0m, 508)
                };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}