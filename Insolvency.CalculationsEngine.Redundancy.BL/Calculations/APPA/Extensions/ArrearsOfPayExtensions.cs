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
            DateTime unpaidPeriodFrom, DateTime unpaidPeriodTo, decimal aPClaimAmount)
        {

            var daysWorkedInClaim = (decimal)(await unpaidPeriodFrom.Date.GetNumBusinessDaysInRange(unpaidPeriodTo.Date, shiftPattern));

            decimal adjustedWeeklyWage;
            if (aPClaimAmount == 0)
            {
                adjustedWeeklyWage = weeklyWage;
            }
            else
            {
                adjustedWeeklyWage = (aPClaimAmount / daysWorkedInClaim) * shiftPattern.Count;
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