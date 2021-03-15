using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayTakenNotPaidAggregateOutput
    {
        public HolidayTakenNotPaidAggregateOutput()
        {
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();
        public string SelectedInputSource { get; set; }
        public HolidayTakenNotPaidResponseDTO RP1ResultsList { get; set; }
        public HolidayTakenNotPaidResponseDTO RP14aResultsList { get; set; }
    }
}
