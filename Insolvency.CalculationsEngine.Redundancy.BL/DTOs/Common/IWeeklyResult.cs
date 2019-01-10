using System;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common
{
    public interface IWeeklyResult
    {
        decimal GrossEntitlement { get; set; }
        decimal TaxDeducted { get; set; }
        decimal NIDeducted { get; set; }
        decimal NetEntitlement { get; set; }
        decimal MaximumEntitlementIn4MonthPeriod { get; set; }
        decimal EmployerEntitlementIn4MonthPeriod { get; set; }
        decimal GrossEntitlementIn4Months { get; set; }
        bool IsSelected { get; set; }
    }
}
