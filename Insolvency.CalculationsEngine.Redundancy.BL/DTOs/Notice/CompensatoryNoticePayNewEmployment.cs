using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class CompensatoryNoticePayNewEmployment
    {
        public CompensatoryNoticePayNewEmployment()
        { }

        [DataType(DataType.DateTime)]
        public DateTime NewEmploymentStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime NewEmploymentEndDate { get; set; }

        public decimal NewEmploymentWage { get; set; }

        public decimal? NewEmploymentWeeklyWage { get; set; }
    }
}
