using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ArrearsOfPayAggregateOutput
    {
        public ArrearsOfPayAggregateOutput()
        {
        }

        public string TraceInfo { get; set; } = string.Empty;
        public string SelectedInputSource { get; set; }
        public ArrearsOfPayResponseDTO RP1ResultsList { get; set; }
        public ArrearsOfPayResponseDTO RP14aResultsList { get; set; }
    }
}
