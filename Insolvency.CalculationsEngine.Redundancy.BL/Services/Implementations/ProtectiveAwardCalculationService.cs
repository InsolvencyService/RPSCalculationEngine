using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Exceptions;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class ProtectiveAwardCalculationService : IProtectiveAwardCalculationService
    {
        public async Task<ProtectiveAwardResponseDTO> PerformProtectiveAwardCalculationAsync(
            ProtectiveAwardCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            var statMaxDate = data.DismissalDate;
            if (data.InsolvencyDate.Date > statMaxDate)
                statMaxDate = data.InsolvencyDate.Date;
            if (data.TribunalAwardDate.Date > statMaxDate)
                statMaxDate = data.TribunalAwardDate.Date;

            var statutoryMax = ConfigValueLookupHelper.GetStatutoryMax(options, statMaxDate);
            var taxRate = ConfigValueLookupHelper.GetTaxRate(options, data.DismissalDate);
            var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, data.DismissalDate);
            var niRate = ConfigValueLookupHelper.GetNIRate(options, data.DismissalDate);

            DayOfWeek payDay = (DayOfWeek)data.PayDay;

            bool isTaxable = (await data.DismissalDate.GetTaxYear() == await data.TribunalAwardDate.GetTaxYear());

            //calculate paAwardEndDate e.g. paAwardStartDate + 30 days
            var protectiveAwardEndDate = data.ProtectiveAwardStartDate.Date.AddDays(data.ProtectiveAwardDays.Value - 1);

            // get pay weeks in PA Award
            var endDate = protectiveAwardEndDate;
            if (endDate.DayOfWeek != payDay)
                endDate = endDate.AddDays(7);

            // set pref period start date
            DateTime prefPeriodStartDate = data.InsolvencyDate.Date.AddMonths(-4);

            var payDays = await data.ProtectiveAwardStartDate.Date.GetDaysInRange(endDate, payDay);

            //step through payWeek
            var counter = 1;
            var payLines = new List<ProtectiveAwardPayLine>();
            foreach (var payWeekEnd in payDays)
            {
                var employmentDays = 0;
                var maximumDays = 0;
                var benefitClaimedAmount = 0.00m;
                var employmentDaysInPrefPeriod = 0;
                var maximumDaysInPrefPeriod = 0;

                for (int j = 6; j >= 0; j--)
                {
                    DateTime day = payWeekEnd.AddDays(-j);

                    if (day >= data.ProtectiveAwardStartDate.Date && day <= protectiveAwardEndDate.Date)
                    {
                        //is this day in shift paattern (a working day)?
                        if (await data.ShiftPattern.ContainsDayWeek(day.DayOfWeek))
                        {
                            employmentDays++;

                            if (day >= prefPeriodStartDate && day <= data.InsolvencyDate)
                                employmentDaysInPrefPeriod++;
                        }

                        maximumDays++;

                        if (day >= prefPeriodStartDate && day <= data.InsolvencyDate)
                            maximumDaysInPrefPeriod++;
                    }

                    // determine benefits claimed in week
                    foreach (var benefit in data.Benefits)
                    {
                        if (day >= benefit.BenefitStartDate.Date && day <= benefit.BenefitEndDate.Date)
                        {
                            decimal benefitDailyRate = await benefit.BenefitAmount.GetDailyAmount(
                                benefit.BenefitStartDate.Date,
                                benefit.BenefitEndDate.Date);
                            benefitClaimedAmount += benefitDailyRate;
                        }
                    }
                }

                // calculate Employer Liability for week
                decimal empLiability = data.WeeklyWage / data.ShiftPattern.Count * employmentDays;
                decimal employerEntitlementInPrefPeriod = data.WeeklyWage / data.ShiftPattern.Count * employmentDaysInPrefPeriod;

                //get Maximum Liability
                decimal maxLiability = statutoryMax;
                decimal maximumEntitlementInPrefPeriod = statutoryMax / 7 * maximumDaysInPrefPeriod;

                //if first week
                if (payWeekEnd == payDays.First())
                {
                    //if there is only one week OR claimant did start employment in the first week of claim
                    if ((payDays.Count() == 1) || data.EmploymentStartDate.Date >= payDays.First().AddDays(-7))
                        maxLiability = statutoryMax / 7M * maximumDays;
                }
                // if it is the last week
                else if (payWeekEnd == payDays.Last())
                {
                    if (payWeekEnd > protectiveAwardEndDate)
                        maxLiability = statutoryMax / 7 * maximumDays;
                }

                decimal grossEntitlement = Math.Min(empLiability, maxLiability) - benefitClaimedAmount;
                grossEntitlement = Math.Max(grossEntitlement, 0m);
                decimal taxDeducted = Math.Round(await grossEntitlement.GetTaxDeducted(taxRate, isTaxable), 2);
                decimal niDeducted = Math.Round(await grossEntitlement.GetNIDeducted(niThreshold, niRate, true), 2);

                grossEntitlement = Math.Round(grossEntitlement, 2);
                decimal netEntitlement = grossEntitlement - taxDeducted - niDeducted;

                payLines.Add(new ProtectiveAwardPayLine()
                {
                    WeekNumber = counter++,
                    PayDate = payWeekEnd,
                    BenefitsClaimed = Math.Round(benefitClaimedAmount, 2),
                    GrossEntitlement = grossEntitlement,
                    TaxDeducted = taxDeducted,
                    NIDeducted = niDeducted,
                    NetEntitlement = netEntitlement,
                    IsSelected = false,
                    MaximumEntitlementIn4MonthPeriod = Math.Round(maximumEntitlementInPrefPeriod, 2),
                    EmployerEntitlementIn4MonthPeriod = Math.Round(employerEntitlementInPrefPeriod, 2),
                    GrossEntitlementIn4Months = Math.Round(Math.Min(maximumEntitlementInPrefPeriod, employerEntitlementInPrefPeriod), 2)
                });
            };

            var result = new ProtectiveAwardResponseDTO()
            {
                IsTaxable = isTaxable,
                StatutoryMax = statutoryMax,
                PayLines = payLines
            };
            return await Task.FromResult(result);
        }
    }
}