using System;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayPayAccruedCalculationRequestModel
    {
        public HolidayPayAccruedCalculationRequestModel()
        {

        }

        public HolidayPayAccruedCalculationRequestModel(DateTime insolvencyDate, DateTime empStartDate, DateTime dismissakDate, decimal contractedHolEntitlement,
                                                        DateTime holidayYearStart, bool isTaxable, int payDay, List<string> shiftPattern, 
                                                        decimal weeklyWage, decimal daysCFwd, decimal daysTaken, decimal ipConfirmedDays)
        {
            InsolvencyDate = insolvencyDate;
            EmpStartDate = empStartDate;
            DismissalDate = dismissakDate;
            ContractedHolEntitlement = contractedHolEntitlement;
            HolidayYearStart = holidayYearStart;
            IsTaxable = isTaxable;
            PayDay = PayDay;
            ShiftPattern = shiftPattern;
            WeeklyWage = weeklyWage;
            DaysCFwd = daysCFwd;
            DaysTaken = daysTaken;
            IpConfirmedDays = ipConfirmedDays;
        }

        public DateTime InsolvencyDate { get; set; }
        public DateTime EmpStartDate { get; set; }
        public DateTime DismissalDate { get; set; }
        public decimal? ContractedHolEntitlement { get; set; }
        public DateTime HolidayYearStart { get; set; }
        public bool? IsTaxable { get; set; }
        public int? PayDay { get; set; }
        public List<String> ShiftPattern { get; set; }
        public decimal? WeeklyWage { get; set; }
        public decimal? DaysCFwd { get; set; }
        public decimal? DaysTaken { get; set; }
        public decimal? IpConfirmedDays { get; set; }
    }
}
