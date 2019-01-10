using System;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public class RefundOfNotionalTaxTestsDataGenerator
    {
        public RefundOfNotionalTaxCalculationRequestModel GetValidRequestData()
        {
            return new RefundOfNotionalTaxCalculationRequestModel
            {
                TaxableEarnings = 11450.0m,
                TaxAllowance = 11500.0m,
                MaximumCNPEntitlement = 500.0m,
                CnpPaid = 400.0m,
                CnpTaxDeducted = 100.0m,
            };
        }

        public RefundOfNotionalTaxCalculationRequestModel GetNullPayload()
        {
            return null;
        }

        public RefundOfNotionalTaxResponseDto GetValidResponseData()
        {
            return new RefundOfNotionalTaxResponseDto
            {
                TaxableEarning = 11450.0m,
                TaxAllowance = 11500.0m,
                MaximumCNPEntitlement = 500.0m,
                CnpPaid = 400.0m,
                CnpTaxDeducted = 100.0m,
                MaximumRefundLimit = 50.0m,
                CNPPaidAfterRefund = 450.0m,
                RefundAmount = 50.0m
            };
        }
    }
}
