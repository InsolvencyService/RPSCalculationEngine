using System;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class NoticeWorkedNotPaidWeeklyResult
    {
        public NoticeWorkedNotPaidWeeklyResult()
        {
            
        }

        public int WeekNumber { get; set; }
        public DateTime PayDate { get; set; }
        public decimal MaximumEntitlement { get; set; }
        public decimal EmployerEntitlement { get; set; }
        public decimal GrossEntitlement { get; set; }
        public bool IsTaxable { get; set; }
        public decimal TaxDeducted { get; set; }
        public decimal NiDeducted { get; set; }
        public decimal NetEntitlement { get; set; }
        public int MaximumDays { get; set; }
        public int EmploymentDays { get; set; }
        public decimal MaximumEntitlementIn4MonthPeriod { get; set; }
        public decimal EmployerEntitlementIn4MonthPeriod { get; set; }
        public decimal GrossEntitlementIn4Months { get; set; }
        public bool IsSelected { get; set; }
    }
}
