using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData
{
    public class RefundOfNotionalTaxTestDataHelper
    {
        public class RefundOfNotionalTaxRequestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new RefundOfNotionalTaxCalculationRequestModel(11450m, 11500m, 500m, 400m, 100m),
                    new RefundOfNotionalTaxResponseDto(11450m, 11500m, 500m, 400m, 100m, 50m, 450m, 50m),
                    //when refund and netCNPPaid exceed cnpMeximumEntitlement
                    new RefundOfNotionalTaxCalculationRequestModel(450m, 11500m, 500m, 400m, 1500m),
                    new RefundOfNotionalTaxResponseDto(450m, 11500m, 500m, 400m, 1500m, 100m, 800m, 400m),
                    //when calculated MaxRefundAmount  results in a negative number
                    new RefundOfNotionalTaxCalculationRequestModel(11550m, 11500m, 10500, 10250m, 2050m),
                    new RefundOfNotionalTaxResponseDto(11550m, 11500m, 10500m, 10250m, 2050m, 0m, 10250m, 0m),

                };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
