using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax
{
    public class RefundOfNotionalTaxResponseDto
    {
        public RefundOfNotionalTaxResponseDto()
        {
        }

        public RefundOfNotionalTaxResponseDto(decimal taxableEarning, decimal taxAllowance, decimal maximumCNPEntitlement, decimal cnpPaid, decimal cnpTaxDeducted,
            decimal? maximumRefundLimit, decimal? cnpPaidAfterRefund, decimal refundAmount)
        {
            TaxableEarning = taxableEarning;
            TaxAllowance = taxAllowance;
            MaximumCNPEntitlement = maximumCNPEntitlement;
            CnpPaid = cnpPaid;
            CnpTaxDeducted = cnpTaxDeducted;
            MaximumRefundLimit = maximumRefundLimit;
            CNPPaidAfterRefund = cnpPaidAfterRefund;
            RefundAmount = refundAmount;
        }
        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();
        public decimal? TaxableEarning { get; set; }
        public decimal? TaxAllowance { get; set; }
        public decimal? MaximumCNPEntitlement { get; set; }
        public decimal? CnpPaid { get; set; }
        public decimal? CnpTaxDeducted { get; set; }
        public decimal? MaximumRefundLimit { get; set; }
        public decimal? CNPPaidAfterRefund { get; set; }
        public decimal? RefundAmount { get; set; }
    }
}





