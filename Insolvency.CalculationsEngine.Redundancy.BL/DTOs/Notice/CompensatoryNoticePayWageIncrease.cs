using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class CompensatoryNoticePayWageIncrease
    {
        public CompensatoryNoticePayWageIncrease()
        { }

        [DataType(DataType.DateTime)]
        public DateTime WageIncreaseStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? WageIncreaseEndDate { get; set; }

        public decimal WageIncreaseAmount { get; set; }
    }
}
