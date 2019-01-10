using System;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayTakenNotPaidWeeklyResult 
    {
        public HolidayTakenNotPaidWeeklyResult()
        { }

        public HolidayTakenNotPaidWeeklyResult(int weekNumber, DateTime payDate ,decimal maximumEntitlement, decimal employerEntitlement, decimal grossEntitlement,
        bool isTaxable, decimal taxDeducted, decimal niDeducted, decimal netEntitlement, int maximumDays, decimal employmentDays,
        decimal maximumEntitlementIn4MonthPeriod, decimal employerEntitlementIn4MonthPeriod, decimal grossEntitlementIn4Months, bool isSelected)
        {
            WeekNumber = weekNumber;
            PayDate = payDate;
            MaximumEntitlement = maximumEntitlement;
            EmployerEntitlement = employerEntitlement;
            GrossEntitlement = grossEntitlement;
            IsTaxable = isTaxable;
            TaxDeducted = taxDeducted;
            NiDeducted = niDeducted;
            NetEntitlement = netEntitlement;
            MaximumDays = maximumDays;
            EmploymentDays = employmentDays;
            MaximumEntitlementIn4MonthPeriod = maximumEntitlementIn4MonthPeriod;
            EmployerEntitlementIn4MonthPeriod = employerEntitlementIn4MonthPeriod;
            GrossEntitlementIn4Months = grossEntitlementIn4Months;
            IsSelected = isSelected;
        }

        public int WeekNumber { get; set; }
        public DateTime PayDate { get; set; }
        public decimal MaximumEntitlement { get; set; }
        public decimal EmployerEntitlement { get; set; }
        public bool IsTaxable { get; set; }
        public int MaximumDays { get; set; }
        public decimal EmploymentDays { get; set; }
        public decimal GrossEntitlement { get; set; }
        public decimal TaxDeducted { get; set; }
        public decimal NiDeducted { get; set; }
        public decimal NetEntitlement { get; set; }
        public decimal GrossEntitlementIn4Months { get; set; }
        public bool IsSelected { get; set; }
        public decimal MaximumEntitlementIn4MonthPeriod { get; set; }
        public decimal EmployerEntitlementIn4MonthPeriod { get; set; }
    }
}
