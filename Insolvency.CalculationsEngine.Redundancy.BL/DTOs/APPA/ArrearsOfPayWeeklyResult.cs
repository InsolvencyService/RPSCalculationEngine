using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using System;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ArrearsOfPayWeeklyResult : IWeeklyResult
    {

        public ArrearsOfPayWeeklyResult() 
        {

        }

        public ArrearsOfPayWeeklyResult(int weekNumber, DateTime payDate, decimal apPayRate, decimal maximumEntitlement,
            decimal employerEntitlement, decimal grossEntitlement, bool isTaxable, decimal taxDeducated, decimal niDeducted, decimal netEntitlement, int maxDays, int empDays, 
            decimal maximumEntitlementIn4MonthPeriod, decimal employerEntitlementIn4MonthPeriod, decimal grossEntitlementIn4Months,
            bool isSelected = false)
        {
            WeekNumber = weekNumber;
            PayDate = payDate;
            ApPayRate = apPayRate;
            MaximumEntitlement = maximumEntitlement;
            EmployerEntitlement = employerEntitlement;
            GrossEntitlement = grossEntitlement;
            IsTaxable = isTaxable;
            TaxDeducted = taxDeducated;
            NIDeducted = niDeducted;
            NetEntitlement = netEntitlement;
            MaximumDays = maxDays;
            EmploymentDays = empDays;
            MaximumEntitlementIn4MonthPeriod = maximumEntitlementIn4MonthPeriod;
            EmployerEntitlementIn4MonthPeriod = employerEntitlementIn4MonthPeriod;
            GrossEntitlementIn4Months = grossEntitlementIn4Months;
            IsSelected = isSelected;
        }

        public int WeekNumber { get; set; }
        public DateTime PayDate { get; set; }
        public decimal ApPayRate { get; set; }
        public decimal MaximumEntitlement { get; set; }
        public decimal EmployerEntitlement { get; set; }
        public decimal GrossEntitlement { get; set; }
        public bool IsTaxable { get; set; }
        public decimal TaxDeducted { get; set; }
        public decimal NIDeducted { get; set; }
        public decimal NetEntitlement { get; set; }
        public int MaximumDays { get; set; }
        public int EmploymentDays { get; set; }
        public decimal MaximumEntitlementIn4MonthPeriod { get; set; }
        public decimal EmployerEntitlementIn4MonthPeriod { get; set; }
        public decimal GrossEntitlementIn4Months { get; set; }
        public bool IsSelected { get; set; }
    }
}