using System;
using System.ComponentModel.DataAnnotations;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate
{
    public class ProjectedNoticeDateCalculationRequestModel
    {
        public ProjectedNoticeDateCalculationRequestModel() { }
        public ProjectedNoticeDateCalculationRequestModel(DateTime empStartDate, DateTime dismissalDate, DateTime noticeGivenDate)
        {
            this.EmploymentStartDate = empStartDate;
            this.DismissalDate = dismissalDate;
            this.DateNoticeGiven = noticeGivenDate;
        }

        [DataType(DataType.DateTime)]
        public DateTime EmploymentStartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DismissalDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateNoticeGiven { get; set; }
    }
}
