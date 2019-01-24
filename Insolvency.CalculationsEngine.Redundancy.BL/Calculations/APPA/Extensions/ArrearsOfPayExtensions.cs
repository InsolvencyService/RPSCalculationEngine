using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Calculations.APPA.Extensions
{
    public static class ArrearsOfPayExtensions
    {

        public static async Task<decimal> GetAdjustedWeeklyWageAsync(this decimal weeklyWage, List<string> shiftPattern,
            DateTime adjustedPeriodFrom, DateTime adjustedPeriodTo, decimal aPClaimAmount)
        {
            var adjustedWeeklyWage = 0.0m;
            var weeksWorkedInClaim = 0.0m;

            var daysWorkedInClaim = (decimal)(await adjustedPeriodFrom.Date.GetNumBusinessDaysInRange(adjustedPeriodTo.Date, shiftPattern));

            weeksWorkedInClaim = daysWorkedInClaim / shiftPattern.Count;
            Debug.WriteLine(weeksWorkedInClaim);
            if (aPClaimAmount == 0)
            {
                adjustedWeeklyWage = weeklyWage;
            }
            else
            {
                if (weeksWorkedInClaim > 0) adjustedWeeklyWage = aPClaimAmount / weeksWorkedInClaim;
            }

            return await Task.FromResult(adjustedWeeklyWage);
        }

        public static async Task<decimal> GetPreferentialClaimAsync(this decimal totalApPaid,
            decimal apClaimAmount, decimal preferentialLimit)
        {
            var appPercentage = totalApPaid / apClaimAmount;
            var preferentialClaim = totalApPaid * appPercentage;
            if (preferentialClaim > preferentialLimit)
            {
                preferentialClaim = preferentialLimit * appPercentage;
            }

            return await Task.FromResult(preferentialClaim);
        }
    }
}