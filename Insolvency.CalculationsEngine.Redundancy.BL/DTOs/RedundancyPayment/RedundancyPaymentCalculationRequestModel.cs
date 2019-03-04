using System;
using System.ComponentModel.DataAnnotations;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment
{
    public class RedundancyPaymentCalculationRequestModel
    {
        public RedundancyPaymentCalculationRequestModel() { }
        public RedundancyPaymentCalculationRequestModel(DateTime empStartDate, DateTime dismissalDate, DateTime noticeGivenDate, DateTime dob, decimal weeklyWage, decimal empPartPayment,int empBreak)
        {
            this.EmploymentStartDate = empStartDate;
            this.DismissalDate = dismissalDate;
            this.DateNoticeGiven = noticeGivenDate;
            this.DateOfBirth = dob;
            this.WeeklyWage = weeklyWage;
            this.EmployerPartPayment = empPartPayment;
            this.EmploymentBreaks = empBreak;
        }

        [Required(ErrorMessage = "You must provide the employment start date")]
        [DataType(DataType.Date)]
        public DateTime EmploymentStartDate { get; set; }

        [Required(ErrorMessage = "You must provide the last day on which the employee worked")]
        [DataType(DataType.Date)]
        public DateTime DismissalDate { get; set; }

        [Required(ErrorMessage = "You must provide the date notice given to the employee")]
        [DataType(DataType.Date)]
        public DateTime DateNoticeGiven { get; set; }

        [DataType(DataType.Date)]
        public DateTime ClaimReceiptDate { get; set; }

        [Required(ErrorMessage = "You must provide the date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "You must provide weekly wage")]
        public decimal WeeklyWage { get; set; }

        [Required(ErrorMessage = "You must provide the partial payment amount made by the employer")]
        public decimal EmployerPartPayment { get; set; }
        
        [Required(ErrorMessage = "You must provide the number of days lost due to break in employemnment")]
        public int EmploymentBreaks { get; set; }
    }
}
