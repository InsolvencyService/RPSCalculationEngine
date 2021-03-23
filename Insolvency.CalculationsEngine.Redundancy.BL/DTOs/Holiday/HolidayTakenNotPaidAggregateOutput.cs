using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayTakenNotPaidAggregateOutput
    {
        public HolidayTakenNotPaidAggregateOutput()
        {
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.ConvertToJson();
        public string SelectedInputSource { get; set; }
        public HolidayTakenNotPaidResponseDTO RP1ResultsList { get; set; }
        public HolidayTakenNotPaidResponseDTO RP14aResultsList { get; set; }
    }
}
