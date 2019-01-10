using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class CompensatoryNoticePayBenefit
    {
        public CompensatoryNoticePayBenefit()
        { }

        [DataType(DataType.DateTime)]
        public DateTime BenefitStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BenefitEndDate { get; set; }

        public decimal BenefitAmount { get; set; }
    }
}
