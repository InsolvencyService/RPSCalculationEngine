namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment
{
    public class ApportionmentCalculationRequestModel
    {
        public decimal GrossEntitlement { get; set; }
        public decimal TotalClaimedInFourMonth{ get; set; }
        public decimal GrossPaidInFourMonth { get; set; }
        public bool TupeStatus { get; set; }
    }
}
