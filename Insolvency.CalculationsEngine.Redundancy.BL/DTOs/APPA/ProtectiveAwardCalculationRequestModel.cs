using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ProtectiveAwardCalculationRequestModel
    {
        public ProtectiveAwardCalculationRequestModel()
        {
            PayDay = -1;
            //Benefits = new List<ProtectiveAwardBenefit>();
        }

        [DataType(DataType.DateTime)]
        public DateTime InsolvencyDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EmploymentStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DismissalDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TribunalAwardDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ProtectiveAwardStartDate { get; set; }

        public int? ProtectiveAwardDays { get; set; }

        public int? PayDay { get; set; }

        public decimal WeeklyWage { get; set; }

        public List<string> ShiftPattern { get; set; }

        //public List<ProtectiveAwardBenefit> Benefits { get; set; }

        public DateTime paBenefitStartDate { get; set; }

        public decimal paBenefitAmount { get; set; }
    }
}
