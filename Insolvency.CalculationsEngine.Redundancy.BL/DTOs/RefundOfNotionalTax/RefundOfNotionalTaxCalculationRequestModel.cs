namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax
{
    public class RefundOfNotionalTaxCalculationRequestModel
    {
        public RefundOfNotionalTaxCalculationRequestModel() { }
        public RefundOfNotionalTaxCalculationRequestModel(decimal taxableEarnings, decimal taxAllowance, decimal maximumCNPEntitlement, decimal cnpPaid,
                                                          decimal cnpTaxDeducted)
        {
            TaxableEarnings = taxableEarnings;
            TaxAllowance = taxAllowance;
            MaximumCNPEntitlement = maximumCNPEntitlement;
            CnpPaid = cnpPaid;
            CnpTaxDeducted = cnpTaxDeducted;
        }

        public decimal? TaxableEarnings { get; set; }
        public decimal? TaxAllowance { get; set; }
        public decimal? MaximumCNPEntitlement { get; set; }
        public decimal? CnpPaid { get; set; }
        public decimal? CnpTaxDeducted { get; set; }
    }
}
