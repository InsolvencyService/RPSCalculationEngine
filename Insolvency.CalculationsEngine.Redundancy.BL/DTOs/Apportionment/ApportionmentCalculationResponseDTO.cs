using System;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment
{
    public class ApportionmentCalculationResponseDTO
    {
        public decimal PrefClaim { get; set; }
        public decimal NonPrefClaim { get; set; }
        public decimal ApportionmentPercentage { get; set; }
        public bool TupeStatus { get; set; }
    }
}
