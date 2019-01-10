using System;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ArrearsOfPayCalculationRequestModel
    {
        //rp1 rp14a
        public string InputSource { get; set; }

        public DateTime InsolvencyDate { get; set; }

        public DateTime EmploymentStartDate { get; set; }

        public DateTime DismissalDate { get; set; }

        public DateTime DateNoticeGiven { get; set; }

        public DateTime UnpaidPeriodFrom { get; set; }

        public DateTime UnpaidPeriodTo { get; set; }

        public decimal ApClaimAmount { get; set; }

        public bool IsTaxable { get; set; }

        public int? PayDay { get; set; }

        public List<string> ShiftPattern { get; set; }

        public decimal WeeklyWage { get; set; }
    }
}