using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ProtectiveAwardResponseDTO
    {
        public ProtectiveAwardResponseDTO()
        {
            PayLines = new List<ProtectiveAwardPayLine>();
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();

        public bool IsTaxable { get; set; }

        public decimal StatutoryMax { get; set; }

        public List<ProtectiveAwardPayLine> PayLines { get; set; }
    }
}
