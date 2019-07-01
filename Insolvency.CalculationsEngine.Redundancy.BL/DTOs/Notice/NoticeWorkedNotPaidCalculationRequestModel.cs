using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class NoticeWorkedNotPaidCalculationRequestModel
    {
        public NoticeWorkedNotPaidCalculationRequestModel()
        {
        }

        //rp1 rp14a
        public string InputSource { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EmploymentStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime InsolvencyDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DismissalDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateNoticeGiven { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UnpaidPeriodFrom { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UnpaidPeriodTo { get; set; }

        public decimal WeeklyWage { get; set; }

        public List<string> ShiftPattern { get; set; }

        public int? PayDay { get; set; }

        public bool IsTaxable { get; set; }

        public decimal ApClaimAmount { get; set; }
    }
}
