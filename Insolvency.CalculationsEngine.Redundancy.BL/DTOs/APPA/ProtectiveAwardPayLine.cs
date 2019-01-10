using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using System;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA
{
    public class ProtectiveAwardPayLine : IWeeklyResult
    {
        public ProtectiveAwardPayLine()
        {
        }

        public ProtectiveAwardPayLine(int weekNumber, DateTime payDate, decimal benefitsClaimed, decimal grossEntitlement, decimal taxDeducted,
                                        decimal niDeducted, decimal netEntitlement, decimal maximumEntitlementIn4MonthPeriod,
                                        decimal employerEntitlementIn4MonthPeriod, decimal grossEntitlementIn4Months, bool isSelected = false)
        {
            WeekNumber = weekNumber;
            PayDate = payDate;
            BenefitsClaimed = benefitsClaimed;
            GrossEntitlement = grossEntitlement;
            TaxDeducted = taxDeducted;
            NIDeducted = niDeducted;
            NetEntitlement = netEntitlement;
            MaximumEntitlementIn4MonthPeriod = maximumEntitlementIn4MonthPeriod;
            EmployerEntitlementIn4MonthPeriod = employerEntitlementIn4MonthPeriod;
            GrossEntitlementIn4Months = grossEntitlementIn4Months;
            IsSelected = isSelected;
        }

        public int WeekNumber { get; set; }

        public DateTime PayDate { get; set; }

        public decimal BenefitsClaimed { get; set; }

        public decimal GrossEntitlement { get; set; }

        public decimal TaxDeducted { get; set; }

        public decimal NIDeducted { get; set; }

        public decimal NetEntitlement { get; set; }

        public bool IsSelected { get; set; }

        public decimal MaximumEntitlementIn4MonthPeriod { get; set; }

        public decimal EmployerEntitlementIn4MonthPeriod { get; set; }

        public decimal GrossEntitlementIn4Months { get; set; }
    }
}
