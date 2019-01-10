using System;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayPayAccruedWeeklyResult 
    {
        public HolidayPayAccruedWeeklyResult()
        { }

        public HolidayPayAccruedWeeklyResult(int weekNumber, decimal maximumEntitlement, decimal employerEntitlement,
                                            decimal grossEntitlement, bool isTaxable, decimal taxDeducated, decimal niDeducted, decimal netEntitlement,
                                            decimal preferentialClaim, decimal nonPreferentialClaim)
        {
            WeekNumber = weekNumber;
            MaximumEntitlement = maximumEntitlement;
            EmployerEntitlement = employerEntitlement;
            GrossEntitlement = grossEntitlement;
            IsTaxable = isTaxable;
            TaxDeducted = taxDeducated;
            NiDeducted = niDeducted;
            NetEntitlement = netEntitlement;
            PreferentialClaim = preferentialClaim;
            NonPreferentialClaim = nonPreferentialClaim;
        }

        public int WeekNumber { get; set; }
        public decimal MaximumEntitlement { get; set; }
        public decimal EmployerEntitlement { get; set; }
        public bool IsTaxable { get; set; }
        public decimal PreferentialClaim { get; set; }
        public decimal NonPreferentialClaim { get; set; }

        public decimal GrossEntitlement { get; set; }
        public decimal TaxDeducted { get; set; }
        public decimal NiDeducted { get; set; }
        public decimal NetEntitlement { get; set; }
    }
}