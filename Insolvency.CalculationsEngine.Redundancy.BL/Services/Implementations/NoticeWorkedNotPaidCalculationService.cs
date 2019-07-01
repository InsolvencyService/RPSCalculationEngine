using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Notice.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class NoticeWorkedNotPaidCalculationService : INoticeWorkedNotPaidCalculationService
    {
        public async Task<NoticeWorkedNotPaidResponseDTO> PerformNwnpCalculationAsync(NoticeWorkedNotPaidCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            var calculationResult = new NoticeWorkedNotPaidResponseDTO();
            var weeklyResult = new List<NoticeWorkedNotPaidWeeklyResult>();
            decimal statutoryMax = ConfigValueLookupHelper.GetStatutoryMax(options, data.InsolvencyDate.Date);

            int yearsOfService = await data.EmploymentStartDate.Date.GetServiceYearsAsync(data.DismissalDate.Date);
            yearsOfService = Math.Max(yearsOfService, 1);
            yearsOfService = Math.Min(yearsOfService, 12);

            //calculate last date they can claim
            var noticeStartDate = data.DateNoticeGiven.Date.AddDays(1);
            var adjUnpaidPeriodFrom = (data.UnpaidPeriodFrom.Date > noticeStartDate) ? data.UnpaidPeriodFrom.Date : noticeStartDate;

            var entitlementEndDate = noticeStartDate.AddDays(yearsOfService * 7).AddDays(-1);

            //The maximum end date is notice start date + number of entitlement weeks
            var adjUnpaidPeriodTo = (data.UnpaidPeriodTo.Date < entitlementEndDate) ? data.UnpaidPeriodTo.Date : entitlementEndDate;
            adjUnpaidPeriodTo = (data.DismissalDate.Date < adjUnpaidPeriodTo) ? data.DismissalDate.Date : adjUnpaidPeriodTo;

            // Extend adjusted period by 7 days to capture last working week 
            var extendedAdjustedPeriodTo = adjUnpaidPeriodTo;
            if (extendedAdjustedPeriodTo.DayOfWeek != data.DateNoticeGiven.DayOfWeek)
                extendedAdjustedPeriodTo = extendedAdjustedPeriodTo.AddDays(7);

            var payDays = await adjUnpaidPeriodFrom
                .AddDays(1)
                .GetDaysInRange(extendedAdjustedPeriodTo, data.DateNoticeGiven.DayOfWeek);

            // set pref period start date
            DateTime prefPeriodStartDate = data.InsolvencyDate.Date.AddMonths(-4);

            //step through paydaysCollection
            for (var paydaysCollectionIndex = 0;
                paydaysCollectionIndex < payDays.Count;
                paydaysCollectionIndex++)
            {
                var employmentDaysInPrefPeriod = 0;
                var maximumDaysInPrefPeriod = 0;
                var employmentDays = 0;
                var maximumDays = 0;
                var maximumEntitlement = 0.0m;

                // Debug.WriteLine($"Week start = {pDay.Start}");
                //step through days of week for this payday
                var weekDates = await payDays[paydaysCollectionIndex].Date.GetWeekDatesAsync();
                var weekDatesListIndex = 6;
                while (weekDatesListIndex >= 0)
                {
                    var date = weekDates[weekDatesListIndex];
                    var dayNum = (int)date.DayOfWeek;
                    //is this day a working day?
                    if (data.ShiftPattern.Contains(dayNum.ToString()))
                    {
                        if (date >= adjUnpaidPeriodFrom.Date && date <= adjUnpaidPeriodTo.Date)
                        {
                            employmentDays++;

                            if (date >= prefPeriodStartDate && date <= data.InsolvencyDate)
                                employmentDaysInPrefPeriod++;
                        }
                    }
                    //maxDays does not rely on working days
                    if (date <= extendedAdjustedPeriodTo.Date && date <= adjUnpaidPeriodTo.Date)
                    {
                        maximumDays++;

                        if (date >= prefPeriodStartDate && date <= data.InsolvencyDate)
                            maximumDaysInPrefPeriod++;
                    }
                    weekDatesListIndex--;
                }

                var adjustedPeriodFrom = await data.UnpaidPeriodFrom.Date.GetAdjustedPeriodFromAsync(data.InsolvencyDate.Date);
                var adjustedPeriodTo = await data.UnpaidPeriodTo.Date.GetAdjustedPeriodToAsync(data.InsolvencyDate.Date, data.DismissalDate.Date);

                decimal adjustedWeeklyWage = await data.WeeklyWage.GetAdjustedWeeklyWageAsync(data.ShiftPattern, adjustedPeriodFrom, adjustedPeriodTo, data.ApClaimAmount);

                //calculate Employer Liability for week
                var employerEntitlement = adjustedWeeklyWage / data.ShiftPattern.Count * employmentDays;
                var employerEntitlementInPrefPeriod = adjustedWeeklyWage / data.ShiftPattern.Count * employmentDaysInPrefPeriod;
                var maximumEntitlementInPrefPeriod = statutoryMax / 7 * maximumDaysInPrefPeriod;

                //calculate Statutory Maximum liability for week

                // if it is the last week
                if (paydaysCollectionIndex == payDays.Count - 1)
                {
                    //if last payday is after dismissal date
                    if (payDays[paydaysCollectionIndex].Date > data.DismissalDate.Date)
                    {
                        maximumEntitlement = statutoryMax / 7m * maximumDays;
                    }
                    else
                    {
                        maximumEntitlement = statutoryMax;
                        maximumDays = 7;
                        if (payDays[paydaysCollectionIndex].Date < data.DismissalDate.Date && payDays[paydaysCollectionIndex].Date < data.InsolvencyDate)
                        {
                            maximumEntitlementInPrefPeriod = statutoryMax;
                        }
                    }
                }
                else
                {
                    maximumEntitlement = statutoryMax;
                }

                var grossEntitlement = Math.Min(maximumEntitlement, employerEntitlement);
                var taxRate = ConfigValueLookupHelper.GetTaxRate(options, DateTime.Now);
                var taxDeducated = Math.Round(await grossEntitlement.GetTaxDeducted(taxRate, data.IsTaxable), 2);
                var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, DateTime.Now);
                var niRate = ConfigValueLookupHelper.GetNIRate(options, DateTime.Now);
                var niDeducted = Math.Round(await grossEntitlement.GetNIDeducted(niThreshold, niRate, data.IsTaxable), 2);

                grossEntitlement = Math.Round(grossEntitlement, 2);
                var netLiability = await grossEntitlement.GetNetLiability(taxDeducated, niDeducted);

                Debug.WriteLine(
                    $"week : {payDays[paydaysCollectionIndex]} MaxEntitlement : {maximumEntitlement} || EmpEntitlement = {employerEntitlement} || Min : {Math.Min(maximumEntitlement, employerEntitlement)} ");

                weeklyResult.Add(new NoticeWorkedNotPaidWeeklyResult
                {
                    WeekNumber = paydaysCollectionIndex + 1,
                    PayDate = payDays[paydaysCollectionIndex].Date,
                    MaximumEntitlement = Math.Round(maximumEntitlement, 2),
                    EmployerEntitlement = Math.Round(employerEntitlement, 2),
                    GrossEntitlement = grossEntitlement,
                    IsTaxable = data.IsTaxable,
                    TaxDeducted = taxDeducated,
                    NiDeducted = niDeducted,
                    NetEntitlement = netLiability,
                    MaximumDays = maximumDays,
                    EmploymentDays = employmentDays,
                    MaximumEntitlementIn4MonthPeriod = Math.Round(maximumEntitlementInPrefPeriod, 2),
                    EmployerEntitlementIn4MonthPeriod = Math.Round(employerEntitlementInPrefPeriod, 2),
                    GrossEntitlementIn4Months = Math.Round(Math.Min(maximumEntitlementInPrefPeriod, employerEntitlementInPrefPeriod), 2)
                });

            } //outter loop payday collection

            calculationResult.InputSource = data.InputSource;
            calculationResult.StatutoryMax = statutoryMax;
            calculationResult.WeeklyResult = weeklyResult;
            return await Task.FromResult(calculationResult);
        }
    }
}
