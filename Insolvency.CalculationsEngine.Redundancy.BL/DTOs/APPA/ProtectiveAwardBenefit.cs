using System;
using System.ComponentModel.DataAnnotations;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ProtectiveAwardBenefit
    {
        [DataType(DataType.DateTime)]
        public DateTime BenefitStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BenefitEndDate { get; set; }

        public decimal BenefitAmount { get; set; }
    }
}
