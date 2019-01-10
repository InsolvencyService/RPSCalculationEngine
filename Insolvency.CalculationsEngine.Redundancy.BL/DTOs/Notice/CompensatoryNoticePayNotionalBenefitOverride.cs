using System;
using System.ComponentModel.DataAnnotations;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class CompensatoryNoticePayNotionalBenefitOverride
    {
        public CompensatoryNoticePayNotionalBenefitOverride()
        { }

        [DataType(DataType.DateTime)]
        public DateTime NotionalBenefitOverrideStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime NotionalBenefitOverrideEndDate { get; set; }
    }
}
