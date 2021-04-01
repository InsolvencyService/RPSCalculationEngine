using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Holiday.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class HolidayTakenNotPaidCalculationService : IHolidayTakenNotPaidCalculationService
    {
        private class Week
        {
            public DateTime PayDate { get; set; }
            public bool IsSelected { get; set; }
            public decimal EmploymentDays { get; set; }
            public decimal EmploymentDaysInPrefPeriod { get; set; }
        }

        public async Task<HolidayTakenNotPaidResponseDTO> PerformCalculationAsync(
            List<HolidayTakenNotPaidCalculationRequestModel> data,
            string inputSource,
            decimal maxDaysInCurrentHolidayYear,
            decimal maxDaysInTotal,
            DateTime? holidayYearStart,
            IOptions<ConfigLookupRoot> options, TraceInfo traceInfo = null)
        {
            var statutoryMax = ConfigValueLookupHelper.GetStatutoryMax(options, data.First().InsolvencyDate);

            var calculationResult = new HolidayTakenNotPaidResponseDTO();
            calculationResult.StatutoryMax = Math.Round(statutoryMax, 2);
            calculationResult.InputSource = inputSource;

            var firstRequest = data.FirstOrDefault(r => r.InputSource == inputSource);
            if (firstRequest != null)
            {
                var tweleveMonthsPrior = firstRequest.InsolvencyDate.Date.AddMonths(-12).AddDays(1);
                var htnpEndDate = firstRequest.DismissalDate.Date < firstRequest.InsolvencyDate.Date ? firstRequest.DismissalDate.Date : firstRequest.InsolvencyDate.Date;

                var weeks = new List<Week>();

                if (holidayYearStart.HasValue)
                {
                    var htnpDaysInCurrentHolidayYear = await data.GetHTNPDays(inputSource, holidayYearStart.Value.Date, htnpEndDate);
                    var htnpDaysIn12MonthsPrior = await data.GetHTNPDays(inputSource, tweleveMonthsPrior, holidayYearStart.Value.Date.AddDays(-1));

                    await SelectDaysAndConvertToWeeks(weeks, htnpDaysInCurrentHolidayYear, firstRequest, maxDaysInCurrentHolidayYear);

                    var numHtnpDaysSelected = weeks.Where(x => x.IsSelected).Sum(x => x.EmploymentDays);
                    await SelectDaysAndConvertToWeeks(weeks, htnpDaysIn12MonthsPrior, firstRequest, maxDaysInTotal - numHtnpDaysSelected);
                }
                else
                {
                    var htnpDays = await data.GetHTNPDays(inputSource, tweleveMonthsPrior, htnpEndDate);
                    await SelectDaysAndConvertToWeeks(weeks, htnpDays, firstRequest, maxDaysInTotal);
                }

                DateTime prefPeriodStartDate = firstRequest.InsolvencyDate.Date.AddMonths(-4);
                DateTime prefPeriodEndDate = (firstRequest.DismissalDate < firstRequest.InsolvencyDate) ? firstRequest.DismissalDate.Date : firstRequest.InsolvencyDate.Date;

                // generate the output weeks
                int weekNum = 1;

                foreach (var week in weeks.OrderBy(x => x.PayDate).ThenBy(x => x.IsSelected))
                {
                    var weekStartDate = week.PayDate.AddDays(-6);
                    var maximumDays = await weekStartDate.GetNumDaysInIntersectionOfTwoRanges(week.PayDate, DateTime.MinValue.Date, firstRequest.DismissalDate.Date);
                    var maximumDaysInPrefPeriod = await weekStartDate.GetNumDaysInIntersectionOfTwoRanges(week.PayDate, prefPeriodStartDate, prefPeriodEndDate);

                    //calculate Employer Liability for week
                    var employerEntitlement = firstRequest.WeeklyWage / firstRequest.ShiftPattern.Count * week.EmploymentDays;
                    var employerEntitlementInPrefPeriod = firstRequest.WeeklyWage / firstRequest.ShiftPattern.Count * week.EmploymentDaysInPrefPeriod;
                    var maximumEntitlement = statutoryMax / 7 * maximumDays;
                    var maximumEntitlementInPrefPeriod = statutoryMax / 7 * maximumDaysInPrefPeriod;

                    var grossEntitlement = Math.Min(maximumEntitlement, employerEntitlement);
                    var taxRate = ConfigValueLookupHelper.GetTaxRate(options, DateTime.Now);
                    var taxDeducated = Math.Round(await grossEntitlement.GetTaxDeducted(taxRate, firstRequest.IsTaxable), 2);
                    var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, DateTime.Now);
                    var niRate = ConfigValueLookupHelper.GetNIRate(options, DateTime.Now);
                    var niDeducted = Math.Round(await grossEntitlement.GetNIDeducted(niThreshold, niRate, firstRequest.IsTaxable), 2);

                    grossEntitlement = Math.Round(grossEntitlement, 2);
                    var netLiability = await grossEntitlement.GetNetLiability(taxDeducated, niDeducted);

                    calculationResult.WeeklyResult.Add(new HolidayTakenNotPaidWeeklyResult()
                    {
                        WeekNumber = weekNum++,
                        PayDate = week.PayDate,
                        IsSelected = week.IsSelected,
                        MaximumEntitlement = Math.Round(maximumEntitlement, 2),
                        EmployerEntitlement = Math.Round(employerEntitlement, 2),
                        GrossEntitlement = grossEntitlement,
                        IsTaxable = firstRequest.IsTaxable,
                        TaxDeducted = taxDeducated,
                        NiDeducted = niDeducted,
                        NetEntitlement = netLiability,
                        MaximumDays = maximumDays,
                        EmploymentDays = Math.Round(week.EmploymentDays, 4),
                        MaximumEntitlementIn4MonthPeriod = Math.Round(maximumEntitlementInPrefPeriod, 2),
                        EmployerEntitlementIn4MonthPeriod = Math.Round(employerEntitlementInPrefPeriod, 2),
                        GrossEntitlementIn4Months = Math.Round(Math.Min(maximumEntitlementInPrefPeriod, employerEntitlementInPrefPeriod), 2),
                    });
                }
                foreach (var req in data.Where(x => x.InputSource == inputSource))
                    traceInfo?.Dates.Add(new TraceInfoDate
                    {
                       StartDate = req.UnpaidPeriodFrom,
                       EndDate = req.UnpaidPeriodTo
                   });
            }
            return await Task.FromResult(calculationResult);
        }

        private async Task SelectDaysAndConvertToWeeks(IList<Week> weeks, IList<DateTime> htnpDays, HolidayTakenNotPaidCalculationRequestModel firstRequest, decimal daysToSelect)
        {
            decimal baseSelected = weeks.Sum(x => x.EmploymentDays);
            for (int i = 0; i < htnpDays.Count; i++)
            {
                var payDate = await htnpDays[i].GetPayDay((DayOfWeek)firstRequest.PayDay);
                decimal numSelected = weeks.Sum(x => x.EmploymentDays) - baseSelected;
                decimal amountSelected = 0m;
                decimal amountUnselected = 0m;

                if (numSelected < Math.Floor(daysToSelect))
                    amountSelected = 1m;
                else if (numSelected >= Math.Ceiling(daysToSelect))
                    amountUnselected = 1m;
                else
                {
                    amountSelected = daysToSelect % 1m;
                    amountUnselected = 1m - amountSelected;
                }

                if (amountSelected > 0m)
                {
                    Week weekSelected = GetWeek(weeks, payDate, true);
                    UpdateWeek(weekSelected, htnpDays[i], amountSelected, firstRequest);
                }
                if (amountUnselected > 0m)
                {
                    Week weekUnselected = GetWeek(weeks, payDate, false);
                    UpdateWeek(weekUnselected, htnpDays[i], amountUnselected, firstRequest);
                }
            }
        }

        private Week GetWeek(IList<Week> weeks, DateTime payDate, bool isSelected)
        {
            Week week = weeks.FirstOrDefault(x => x.PayDate == payDate && x.IsSelected == isSelected);
            if (week == null)
            {
                week = new Week()
                {
                    PayDate = payDate,
                    IsSelected = isSelected
                };
                weeks.Add(week);
            }

            return week;
        }

        private void UpdateWeek(Week week, DateTime date, decimal amount, HolidayTakenNotPaidCalculationRequestModel data)
        {
            DateTime prefPeriodStartDate = data.InsolvencyDate.Date.AddMonths(-4);

            week.EmploymentDays += amount;
            if (date >= prefPeriodStartDate && date <= data.InsolvencyDate)
                week.EmploymentDaysInPrefPeriod += amount;
        }
    }
}
