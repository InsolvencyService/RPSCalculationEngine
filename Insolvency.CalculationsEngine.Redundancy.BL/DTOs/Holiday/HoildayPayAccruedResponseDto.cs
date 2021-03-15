using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayPayAccruedResponseDTO
    {
        public HolidayPayAccruedResponseDTO()
        {
            WeeklyResults = new List<HolidayPayAccruedWeeklyResult>();
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();
        public decimal StatutoryMax { get; set; }
        public decimal HolidaysOwed { get; set; }
        public decimal BusinessDaysInClaim { get; set; }
        public decimal WorkingDaysInClaim { get; set; }
        public decimal ProRataAccruedDays { get; set; }
        public List<HolidayPayAccruedWeeklyResult> WeeklyResults { get; set; }
    }
}