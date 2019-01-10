using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ArrearsOfPayResponseDTO
    {
        public ArrearsOfPayResponseDTO()
        {
            WeeklyResult = new List<ArrearsOfPayWeeklyResult>();
        }

        public ArrearsOfPayResponseDTO(string inputSource, decimal statutoryMax, bool dngApplied, bool runNWNP, List<ArrearsOfPayWeeklyResult> weeklyResult)
        {
            InputSource = inputSource;
            StatutoryMax = statutoryMax;
            WeeklyResult = weeklyResult;
            DngApplied = dngApplied;
            RunNWNP = runNWNP;
        }

        public string InputSource { get; set; }
        public decimal StatutoryMax { get; set; }
        public bool DngApplied { get; set; }
        public bool RunNWNP { get; set; }
        public List<ArrearsOfPayWeeklyResult> WeeklyResult { get; set; }
    }
}