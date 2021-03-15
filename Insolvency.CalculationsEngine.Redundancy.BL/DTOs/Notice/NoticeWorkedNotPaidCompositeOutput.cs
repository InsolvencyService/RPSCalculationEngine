using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System.Collections.Generic;

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
        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();
        public string SelectedInputSource { get; set; }

        //public NoticeWorkedNotPaidAggregateOutput nwnpResults;
        public NoticeWorkedNotPaidResponseDTO rp1Results { get; set; }
        public NoticeWorkedNotPaidResponseDTO rp14aResults { get; set; }

    }
}
