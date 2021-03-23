using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Calculations.APPA.Extensions
{
    public static class APPAExtensions
    {

        public static async Task<ArrearsOfPayResponseDTO> MergeWeeklyResults(this List<ArrearsOfPayResponseDTO> list, string inputSource, IOptions<ConfigLookupRoot> options)
        {
            var weekList = list.SelectMany(x => x.WeeklyResult).ToList(); 
            var weeksToMerge = weekList.GroupBy(x => x.PayDate)
                .Where(x => x.Count() > 1)
                .ToList();

            foreach (var week in weeksToMerge)
            {
                var payDate = week.Key;
                var mergedWeek = new ArrearsOfPayWeeklyResult();

                mergedWeek.PayDate = week.First().PayDate;
                mergedWeek.ApPayRate = week.Max(x =>x.ApPayRate);
                mergedWeek.MaximumEntitlement = week.Max(x => x.MaximumEntitlement);
                mergedWeek.EmployerEntitlement = week.Sum(x => x.EmployerEntitlement);
                mergedWeek.MaximumDays = week.Max(x => x.MaximumDays);
                mergedWeek.EmploymentDays = week.Sum(x => x.EmploymentDays);
                mergedWeek.MaximumEntitlementIn4MonthPeriod = week.Max(x => x.MaximumEntitlementIn4MonthPeriod);
                mergedWeek.EmployerEntitlementIn4MonthPeriod = week.Sum(x => x.EmployerEntitlementIn4MonthPeriod);
                mergedWeek.GrossEntitlementIn4Months = Math.Min(mergedWeek.MaximumEntitlementIn4MonthPeriod, mergedWeek.EmployerEntitlementIn4MonthPeriod);

                mergedWeek.GrossEntitlement = Math.Min(mergedWeek.MaximumEntitlement, mergedWeek.EmployerEntitlement);
                mergedWeek.IsTaxable = week.Select(x => x.IsTaxable).FirstOrDefault();

                var taxRate = ConfigValueLookupHelper.GetTaxRate(options, DateTime.Now);
                mergedWeek.TaxDeducted = Math.Round(await mergedWeek.GrossEntitlement.GetTaxDeducted(taxRate, mergedWeek.IsTaxable), 2);
                var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, DateTime.Now);
                var niRate = ConfigValueLookupHelper.GetNIRate(options, DateTime.Now);
                mergedWeek.NIDeducted = Math.Round(await mergedWeek.GrossEntitlement.GetNIDeducted(niThreshold, niRate, mergedWeek.IsTaxable), 2);
                mergedWeek.NetEntitlement = await mergedWeek.GrossEntitlement.GetNetLiability(mergedWeek.TaxDeducted, mergedWeek.NIDeducted);

                //remove the weeks with the same payDate from the selected list. 
                weekList.RemoveAll(x => x.PayDate == payDate);
                weekList.Add(mergedWeek);
            }

            // update weeknumber on the merged list
            var i = 1;
            weekList = weekList.OrderBy(x => x.PayDate).ToList();
            weekList.ForEach(x => x.WeekNumber = i++);

            if (list.Any())
            {
                return new ArrearsOfPayResponseDTO()
                {
                    StatutoryMax = list.First().StatutoryMax,
                    InputSource = inputSource,
                    DngApplied = list.Any(x => x.DngApplied),
                    RunNWNP = list.Any(x => x.RunNWNP),
                    WeeklyResult = weekList
                };
            }
            else
            {
                return new ArrearsOfPayResponseDTO()
                {
                    StatutoryMax = ConfigValueLookupHelper.GetStatutoryMax(options, DateTime.Today),
                    InputSource = inputSource,
                    DngApplied = false,
                    RunNWNP = false,
                };
            }
        }
        }
}