using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class NoticeWorkedNotPaidCompositeOutput
    {

        public NoticeWorkedNotPaidCompositeOutput()
        {
           // nwnpResults = new NoticeWorkedNotPaidAggregateOutput();
            rp1Results = new NoticeWorkedNotPaidResponseDTO();
            rp14aResults = new NoticeWorkedNotPaidResponseDTO();
        }
        public string TraceInfo { get; set; } = TraceInfoSerializer.ConvertToJson();
        public string SelectedInputSource { get; set; }

        //public NoticeWorkedNotPaidAggregateOutput nwnpResults;
        public NoticeWorkedNotPaidResponseDTO rp1Results { get; set; }
        public NoticeWorkedNotPaidResponseDTO rp14aResults { get; set; }

    }
}
