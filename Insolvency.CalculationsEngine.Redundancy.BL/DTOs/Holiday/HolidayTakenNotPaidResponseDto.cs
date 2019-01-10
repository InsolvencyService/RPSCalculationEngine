using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayTakenNotPaidResponseDTO
    {
        public HolidayTakenNotPaidResponseDTO()
        {
            WeeklyResult = new List<HolidayTakenNotPaidWeeklyResult>();
        }

        public HolidayTakenNotPaidResponseDTO(string inputSource, decimal statutoryMax, List<HolidayTakenNotPaidWeeklyResult> weeklyResult)
        {
            InputSource = inputSource;
            StatutoryMax = statutoryMax;
            WeeklyResult = weeklyResult;
        }

        public string InputSource { get; set; }
        public decimal StatutoryMax { get; set; }

        public List<HolidayTakenNotPaidWeeklyResult> WeeklyResult { get; set; }
    }
}
