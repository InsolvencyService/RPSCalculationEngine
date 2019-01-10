using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class CompensatoryNoticePayCalculationRequestModel
    {
        public CompensatoryNoticePayCalculationRequestModel()
        {
            Benefits = new List<CompensatoryNoticePayBenefit>();
            NewEmployments = new List<CompensatoryNoticePayNewEmployment>();
            WageIncreases = new List<CompensatoryNoticePayWageIncrease>();
            NotionalBenefitOverrides = new List<CompensatoryNoticePayNotionalBenefitOverride>();
        }

        [DataType(DataType.DateTime)]
        public DateTime InsolvencyEmploymentStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime InsolvencyDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DismissalDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateNoticeGiven { get; set; }

        public decimal WeeklyWage { get; set; }

        public List<string> ShiftPattern { get; set; }

        public bool IsTaxable { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeceasedDate { get; set; }

        public List<CompensatoryNoticePayBenefit> Benefits { get; set; }

        public List<CompensatoryNoticePayNewEmployment> NewEmployments { get; set; }

        public List<CompensatoryNoticePayWageIncrease> WageIncreases { get; set; }

        public List<CompensatoryNoticePayNotionalBenefitOverride> NotionalBenefitOverrides { get; set; }
    }
}
