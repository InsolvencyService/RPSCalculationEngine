using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class NoticeWorkedNotPaidResponseDTO
    {
        public NoticeWorkedNotPaidResponseDTO()
        {
            WeeklyResult = new List<NoticeWorkedNotPaidWeeklyResult>();
        }

        public NoticeWorkedNotPaidResponseDTO(string inputSource, decimal statutoryMax, List<NoticeWorkedNotPaidWeeklyResult> weeklyResult)
        {
            InputSource = inputSource;
            StatutoryMax = statutoryMax;
            WeeklyResult = weeklyResult;
        }
        public string InputSource { get; set; }
        public decimal StatutoryMax { get; set; }
        public List<NoticeWorkedNotPaidWeeklyResult> WeeklyResult { get; set; }
    }
}
