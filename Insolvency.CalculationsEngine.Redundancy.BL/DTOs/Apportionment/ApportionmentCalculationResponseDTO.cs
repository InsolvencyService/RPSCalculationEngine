using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;
namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment
{
    public class ApportionmentCalculationResponseDTO
    {
        public string TraceInfo { get; set; } = TraceInfoSerializer.ConvertToJson();
        public decimal PrefClaim { get; set; }
        public decimal NonPrefClaim { get; set; }
        public decimal ApportionmentPercentage { get; set; }
        public bool TupeStatus { get; set; }
    }
}
