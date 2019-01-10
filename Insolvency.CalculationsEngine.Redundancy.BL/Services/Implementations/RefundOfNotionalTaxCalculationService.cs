using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class RefundOfNotionalTaxCalculationService : IRefundOfNotionalTaxCalculationService
    {
        public async Task<RefundOfNotionalTaxResponseDto> PerformRefundOfNotionalTaxCalculationAsync(
            RefundOfNotionalTaxCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            decimal? refundAmount = 0.0m;
            var refundDue = data.TaxableEarnings < data.TaxAllowance ? true : false;
            if (refundDue)
            {
                refundAmount = data.CnpTaxDeducted;
            }
            var diffTaxAllowanceTaxableEarnings = data.TaxAllowance - data.TaxableEarnings;
            var diffMaxCNPEntitlementCnpPaid = data.MaximumCNPEntitlement - data.CnpPaid;
            var maximumRefundLimit = diffTaxAllowanceTaxableEarnings < diffMaxCNPEntitlementCnpPaid ? diffTaxAllowanceTaxableEarnings : diffMaxCNPEntitlementCnpPaid;
            if (maximumRefundLimit < 0)
            {
                maximumRefundLimit = 0;
            }
            //check refund does not generate tax liability
            if (refundAmount > maximumRefundLimit)
            {
                refundAmount = maximumRefundLimit;
            }
            //check refund and netCNPPaid does not exceed cnpMeximumEntitlement
            if ((refundAmount + data.CnpTaxDeducted) > (data.MaximumCNPEntitlement))
            {
                refundAmount = data.MaximumCNPEntitlement - refundAmount;
            }

            var calculationResult = new RefundOfNotionalTaxResponseDto();
            calculationResult.TaxableEarning = Math.Round(data.TaxableEarnings.Value, 2);
            calculationResult.TaxAllowance = Math.Round(data.TaxAllowance.Value, 2);
            calculationResult.MaximumCNPEntitlement = Math.Round(data.MaximumCNPEntitlement.Value, 2); 
            calculationResult.CnpPaid = Math.Round(data.CnpPaid.Value, 2);
            calculationResult.CnpTaxDeducted = Math.Round(data.CnpTaxDeducted.Value, 2);
            calculationResult.MaximumRefundLimit = Math.Round(maximumRefundLimit.Value, 2);
            calculationResult.CNPPaidAfterRefund = Math.Round(refundAmount.Value + data.CnpPaid.Value, 2);
            calculationResult.RefundAmount = Math.Round(refundAmount.Value, 2);
            return await Task.FromResult(calculationResult);
        }
    }
}