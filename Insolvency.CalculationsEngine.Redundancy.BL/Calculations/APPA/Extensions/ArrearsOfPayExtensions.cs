using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Itenso.TimePeriod;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Calculations.APPA.Extensions
{
    public static class ArrearsOfPayExtensions
    {

        public static async Task<decimal> GetAdjustedWeeklyWageAsync(this decimal weeklyWage, List<string> shifPattern,
            DateTime adjustedPeriodFrom, DateTime adjustedPeriodTo, decimal aPClaimAmount)
        {
            var adjustedWeeklyWage = 0.0m;
            var weeksWorkedInClaim = 0.0m;
            var daysWorkedInClaim = 0.0m;

            foreach (var day in shifPattern)
            {
                daysWorkedInClaim = daysWorkedInClaim +
                                    await GetShiftDaysInClaimPeriodAsync(adjustedPeriodFrom.Date, adjustedPeriodTo.Date,
                                        Convert.ToInt32(day));
                Debug.WriteLine(daysWorkedInClaim);
            }

            weeksWorkedInClaim = daysWorkedInClaim / shifPattern.Count;
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

        public static async Task<int> GetShiftDaysInClaimPeriodAsync(this DateTime adjustedClaimPeriodFrom,
            DateTime adjustedClaimPeriodTo, int? shiftDay)
        {
            var filter = new CalendarPeriodCollectorFilter();
            filter.WeekDays.Add(await shiftDay.GetEnumValueAsync());
            //add 1 day to adjustedClaimPeriodTo to capture last day
            var claimPeriod =
                new CalendarTimeRange(adjustedClaimPeriodFrom.Date, adjustedClaimPeriodTo.Date.AddDays(1));
            var payDayCollector =
                new CalendarPeriodCollector(filter, claimPeriod);
            payDayCollector.CollectDays();

            return await Task.FromResult(payDayCollector.Periods.Count);
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