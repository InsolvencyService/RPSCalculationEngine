using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ArrearsOfPayAggregateOutput
    {
        public ArrearsOfPayAggregateOutput()
        {
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.ConvertToJson();
        public string SelectedInputSource { get; set; }
        public ArrearsOfPayResponseDTO RP1ResultsList { get; set; }
        public ArrearsOfPayResponseDTO RP14aResultsList { get; set; }
    }
}
