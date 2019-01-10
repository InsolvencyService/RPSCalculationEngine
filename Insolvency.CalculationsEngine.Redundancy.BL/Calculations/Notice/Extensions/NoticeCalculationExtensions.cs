using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Notice.Extensions
{
    public static class NoticeCalculationExtensions
    {
        public static async Task<NoticeWorkedNotPaidResponseDTO> MergePayWeeks(this NoticeWorkedNotPaidResponseDTO nwnpList, IOptions<ConfigLookupRoot> options)
        {
            var weeksToMerge = nwnpList.WeeklyResult.GroupBy(x => x.PayDate).Where(x => x.Count() > 1).ToList();
            foreach (var week in weeksToMerge)
            {
                var payDate = week.Key;

                var mergedWeek = new NoticeWorkedNotPaidWeeklyResult();

                mergedWeek.PayDate = week.Select(x => x.PayDate).FirstOrDefault();

                mergedWeek.MaximumDays = week.Max(x => x.MaximumDays);
                mergedWeek.EmploymentDays = week.Sum(x => x.EmploymentDays);
                mergedWeek.MaximumEntitlement = week.Max(x => x.MaximumEntitlement);
                mergedWeek.EmployerEntitlement = week.Sum(x => x.EmployerEntitlement);
                mergedWeek.GrossEntitlement = Math.Min(mergedWeek.MaximumEntitlement, mergedWeek.EmployerEntitlement);

                mergedWeek.IsTaxable = week.Select(x => x.IsTaxable).FirstOrDefault();
                var taxRate = ConfigValueLookupHelper.GetTaxRate(options, DateTime.Now);
                mergedWeek.TaxDeducted = Math.Round(await mergedWeek.GrossEntitlement.GetTaxDeducted(taxRate, mergedWeek.IsTaxable), 2);
                var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, DateTime.Now);
                var niRate = ConfigValueLookupHelper.GetNIRate(options, DateTime.Now);
                mergedWeek.NiDeducted = Math.Round(await mergedWeek.GrossEntitlement.GetNIDeducted(niThreshold, niRate, mergedWeek.IsTaxable), 2);

                mergedWeek.NetEntitlement = await mergedWeek.GrossEntitlement.GetNetLiability(mergedWeek.TaxDeducted, mergedWeek.NiDeducted);

                mergedWeek.MaximumEntitlementIn4MonthPeriod = week.Max(x => x.MaximumEntitlementIn4MonthPeriod);
                mergedWeek.EmployerEntitlementIn4MonthPeriod = week.Sum(x => x.EmployerEntitlementIn4MonthPeriod);
                mergedWeek.GrossEntitlementIn4Months = Math.Min(mergedWeek.MaximumEntitlementIn4MonthPeriod, mergedWeek.EmployerEntitlementIn4MonthPeriod);

                mergedWeek.IsSelected = false;
                //remove the weeks with the same payDate from the selected list. 
                nwnpList.WeeklyResult.RemoveAll(x => x.PayDate == payDate);
                //add the merged week for that paydate
                nwnpList.WeeklyResult.Add(mergedWeek);
            }

            //update weeknumber on the merged list
            var i = 1;
            nwnpList.WeeklyResult = nwnpList.WeeklyResult.OrderBy(x => x.PayDate).ToList();
            nwnpList.WeeklyResult.ForEach(x => x.WeekNumber = i++);



            return nwnpList;
        }

    }
}
