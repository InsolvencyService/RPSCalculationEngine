using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class NoticeWorkedNotPaidAggregateOutput
    {

        public NoticeWorkedNotPaidAggregateOutput()
        {
            rP1ResultsList = new List<NoticeWorkedNotPaidResponseDTO>();
            rP14aResultsList = new List<NoticeWorkedNotPaidResponseDTO>();
        }
        public List<NoticeWorkedNotPaidResponseDTO> rP1ResultsList { get; set; }
        public List<NoticeWorkedNotPaidResponseDTO> rP14aResultsList { get; set; }
    }
}
