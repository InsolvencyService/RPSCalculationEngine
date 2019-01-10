using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using System;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Calculations.RedundancyPaymentCalculation.Extensions
{
    public static class RedundancyPaymentExtensions
    {

        public static async Task<int> GetNoticeEntitlementWeeks(this DateTime empStartDate, DateTime empEndDate)
        {
            var yearsOfService = await empStartDate.GetServiceYearsAsync(empEndDate);
            if (yearsOfService > 12)
            {
                return (12);
            }
            else
            {
                return (yearsOfService);
            }
        }

        public static async Task<DateTime> GetAdjustedEmploymentStartDate(this DateTime employmentStartDate, int totalDaysLost)
        {
            var adjDate = employmentStartDate.AddDays(totalDaysLost);
            return await Task.FromResult(adjDate);
        }

        public static async Task<DateTime> GetRelevantDismissalDate(this DateTime dismissialDate, DateTime projectedNoticeDate)
        {
            if (projectedNoticeDate <= dismissialDate)
            {
                return await Task.FromResult(dismissialDate);
            }
            else
            {
                return await Task.FromResult(projectedNoticeDate);
            }
        }
        
        public static async Task<int> GetYearsOfServiceFrom22To41(this DateTime DateOfBirth, DateTime adjStartDate, DateTime relevantDismissalDate)
        {
            var birthday21 = DateOfBirth.AddYears(21);
            var birthday22 = DateOfBirth.AddYears(22);
            var birthday41 = DateOfBirth.AddYears(41);
            var birthday42 = DateOfBirth.AddYears(42);
            int yearsFrom22To41 = 0;
            if(relevantDismissalDate.Date <= birthday21.Date || adjStartDate >= birthday41)
            {
                //return zero value
                return yearsFrom22To41;
            }
            var startDate = adjStartDate > birthday22 ? adjStartDate : birthday22;
                if (relevantDismissalDate < birthday42)
                {
                    yearsFrom22To41 = await startDate.GetServiceYearsAsync(relevantDismissalDate);
                }
                else
                {
                    yearsFrom22To41 = await startDate.GetServiceYearsAsync(birthday42);
                }
            return yearsFrom22To41;
        }

        public static async Task<int> GetYearsOfServiceOver41(this DateTime DateOfBirth, DateTime adjStartDate, DateTime relevantDismissalDate)
        {
            var birthday41 = DateOfBirth.AddYears(41);
            int yearsOver41 = 0;
            if (relevantDismissalDate > birthday41)
            {
                if (adjStartDate > birthday41)
                {
                    yearsOver41 = await adjStartDate.GetServiceYearsAsync(relevantDismissalDate);
                }
                else
                {
                    yearsOver41 = await birthday41.GetServiceYearsAsync(relevantDismissalDate);
                }
            }
            if(yearsOver41 > 20)
            {
                //limit to 20 years
                yearsOver41 = 20;
            }
            return yearsOver41;
        }

    }
}
