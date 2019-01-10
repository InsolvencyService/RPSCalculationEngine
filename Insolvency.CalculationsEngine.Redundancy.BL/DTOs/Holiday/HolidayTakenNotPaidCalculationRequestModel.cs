using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayTakenNotPaidCalculationRequestModel
    {
        public HolidayTakenNotPaidCalculationRequestModel()
        {

        }

        public HolidayTakenNotPaidCalculationRequestModel(string inputSource, DateTime insolvencyDate,
            DateTime dismissalDate, DateTime unpaidPeriodFrom, DateTime unpaidPeriodTo, decimal weeklyWage, List<string> shiftPattern,
            int? payDay, bool isTaxable)
        {
            InputSource = inputSource;
            InsolvencyDate = insolvencyDate;
            DismissalDate = dismissalDate;
            UnpaidPeriodFrom = unpaidPeriodFrom;
            UnpaidPeriodTo = unpaidPeriodTo;
            WeeklyWage = weeklyWage;
            ShiftPattern = shiftPattern;
            PayDay = payDay;
            IsTaxable = isTaxable;

        }

        //rp1 rp14a
        public string InputSource { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime InsolvencyDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DismissalDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UnpaidPeriodFrom { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UnpaidPeriodTo { get; set; }

        public decimal WeeklyWage { get; set; }

        public List<string> ShiftPattern { get; set; }

        public int? PayDay { get; set; }

        public bool IsTaxable { get; set; }
    }
}
