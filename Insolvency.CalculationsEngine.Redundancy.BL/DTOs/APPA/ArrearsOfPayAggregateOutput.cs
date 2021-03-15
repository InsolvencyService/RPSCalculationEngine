using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ArrearsOfPayAggregateOutput
    {
        public ArrearsOfPayAggregateOutput()
        {
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();
        public string SelectedInputSource { get; set; }
        public ArrearsOfPayResponseDTO RP1ResultsList { get; set; }
        public ArrearsOfPayResponseDTO RP14aResultsList { get; set; }
    }
}
