using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class CompensatoryNoticePayCalculationService : ICompensatoryNoticePayCalculationService
    {
        private static DateTime CutOverDate = new DateTime(2018, 4, 5);

        public async Task<CompensatoryNoticePayCalculationResponseDTO> PerformCompensatoryNoticePayCalculationAsync(
            CompensatoryNoticePayCalculationRequestModel request,
            IOptions<ConfigLookupRoot> options)
        {
            //set notice start date
            var noticeStartDate = request.DateNoticeGiven.Date.AddDays(1);

            //calculate days notice worked
            var dismissalDate = request.DismissalDate.Date;
            var insolvencyDate = request.InsolvencyDate.Date;

            //set paydays_day.number means the integer index of the day of the week 0 = Sunday
            var payDay = noticeStartDate.AddDays(-1).DayOfWeek;

            //calculate years of service between employment start date and dismissal date
            int yearsOfService = await request.InsolvencyEmploymentStartDate.Date.GetServiceYearsAsync(dismissalDate);

            yearsOfService = Math.Max(yearsOfService, 1);
            yearsOfService = Math.Min(yearsOfService, 12);

            var statutaryMax = ConfigValueLookupHelper.GetStatutoryMax(options, dismissalDate);
            var taxRate = ConfigValueLookupHelper.GetTaxRate(options, dismissalDate);
            var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, dismissalDate);
            var niRate = ConfigValueLookupHelper.GetNIRate(options, dismissalDate);
            var waitingDays = ConfigValueLookupHelper.GetBenefitsWaitingDays(options, dismissalDate);
            var notionalBenefitMonthlyRate = ConfigValueLookupHelper.GetNotionalBenefitsMonthlyRate(options, dismissalDate, 
                await request.DateOfBirth.Date.GetAge(dismissalDate));
            decimal notionalBenefitDailyRate = notionalBenefitMonthlyRate * 12m / 365m;

            var projectedNoticeDate = noticeStartDate.AddDays(yearsOfService * 7 - 1);

            //calculate daily rates
            decimal dailyPayRate = request.WeeklyWage / request.ShiftPattern.Count;
            decimal dailyStatutoryMaximum = statutaryMax / 7m;

            // set claim boudaries
            var claimStartDate = dismissalDate.AddDays(1);
            var claimEndDate = projectedNoticeDate;
            if (request.DeceasedDate.HasValue && request.DeceasedDate.Value.Date < claimEndDate)
                claimEndDate = request.DeceasedDate.Value.Date;

            var adjClaimEndDate = claimEndDate;
            if (adjClaimEndDate.DayOfWeek != payDay)
                adjClaimEndDate = adjClaimEndDate.AddDays(7);

            int daysInClaim = await claimStartDate.GetNumBusinessDaysInRange(claimEndDate, request.ShiftPattern);

            // find paydays in claim period
            var payDays = await claimStartDate.GetDaysInRange(adjClaimEndDate, payDay);

            var weekIndex = 1;
            var notionalDaysCount = 0;
            List<CompensatoryNoticePayResult> weeklyResults = new List<CompensatoryNoticePayResult>();

            foreach (var payWeekEnd in payDays)
            {
                var totalBenefitAmount = 0m;
                var totalNewEmploymentAmount = 0m;
                var totalWageIncreaseAmount = 0m;
                var notionalBenefitAmount = 0m;
                var maximumDays = 0;
                var employerDays = 0;

                // step through days in each pay week
                for (int j = 6; j >= 0; j--)
                {
                    // set date to be checked to consecxutive days in pay week
                    var claimDay = payWeekEnd.AddDays(-j);

                    //check if claimday is in the claimperiod
                    if (claimDay >= claimStartDate && claimDay <= claimEndDate)
                    {
                        bool benefitOrIncomeReceived = false;

                        foreach (var benefit in request.Benefits)
                        {
                            if (claimDay >= benefit.BenefitStartDate.Date && claimDay <= benefit.BenefitEndDate.Date)
                            {
                                benefitOrIncomeReceived = true;
                                totalBenefitAmount += await benefit.BenefitAmount.GetDailyAmount(benefit.BenefitStartDate, benefit.BenefitEndDate);
                            }
                        }

                        foreach (var newEmployment in request.NewEmployments)
                        {
                            if (claimDay >= newEmployment.NewEmploymentStartDate.Date && claimDay <= newEmployment.NewEmploymentEndDate.Date)
                            {
                                benefitOrIncomeReceived = true;
                                var newEmploymentEarnings = newEmployment.NewEmploymentWage;
                                if (newEmployment.NewEmploymentWeeklyWage.HasValue)
                                    newEmploymentEarnings = Math.Max(newEmploymentEarnings, await newEmployment.NewEmploymentWeeklyWage.Value.GetEarnings(newEmployment.NewEmploymentStartDate, newEmployment.NewEmploymentEndDate));
                                totalNewEmploymentAmount += await newEmploymentEarnings.GetDailyAmount(newEmployment.NewEmploymentStartDate, newEmployment.NewEmploymentEndDate);
                            }
                        }


                        foreach (var wageIncrease in request.WageIncreases)
                        {
                            if (claimDay >= wageIncrease.WageIncreaseStartDate.Date && claimDay <= wageIncrease.WageIncreaseEndDate.Date)
                            {
                                benefitOrIncomeReceived = true;
                                totalWageIncreaseAmount += await wageIncrease.WageIncreaseAmount.GetDailyAmount(wageIncrease.WageIncreaseStartDate, wageIncrease.WageIncreaseEndDate);
                            }
                        }

                        foreach (var benefitOverride in request.NotionalBenefitOverrides)
                        {
                            if (claimDay >= benefitOverride.NotionalBenefitOverrideStartDate.Date && claimDay <= benefitOverride.NotionalBenefitOverrideEndDate.Date)
                                benefitOrIncomeReceived = true;
                        }

                        if (benefitOrIncomeReceived)
                        {
                            notionalDaysCount = 0;
                        }
                        else 
                        {
                            notionalDaysCount++;

                            if (notionalDaysCount > waitingDays)
                                notionalBenefitAmount += notionalBenefitDailyRate;
                        }

                        if (await request.ShiftPattern.ContainsDayWeek(claimDay.DayOfWeek))
                            employerDays++;
                        maximumDays++;
                    }
                }

                //apply mitigation
                decimal employerEntitlement = employerDays * dailyPayRate;
                decimal maximumEntitlement = maximumDays * dailyStatutoryMaximum;

                decimal mitigation = totalBenefitAmount + totalNewEmploymentAmount + totalWageIncreaseAmount + notionalBenefitAmount;
                decimal grossEntitlement = Math.Max(employerEntitlement - mitigation, 0);

                decimal notationalTaxDeducted = 0m;
                decimal taxDeducted = 0m;
                decimal niDeducted = 0m;
                decimal netEntitlement = 0m;
                bool isTaxable = false;

                if (dismissalDate <= CutOverDate)
                {
                    notationalTaxDeducted = Math.Round(await grossEntitlement.GetTaxDeducted(taxRate, request.IsTaxable), 2);

                    grossEntitlement = Math.Round(Math.Min(grossEntitlement - notationalTaxDeducted, maximumEntitlement), 2);
                    netEntitlement = grossEntitlement;
                }
                else
                {
                    isTaxable = true;

                    //Apply StatutoryMaximum
                    grossEntitlement = Math.Min(grossEntitlement, maximumEntitlement);
                    taxDeducted = Math.Round(await grossEntitlement.GetTaxDeducted(taxRate, request.IsTaxable), 2);
                    niDeducted = Math.Round(await grossEntitlement.GetNIDeducted(niThreshold, niRate, request.IsTaxable), 2);

                    grossEntitlement = Math.Round(grossEntitlement, 2);
                    netEntitlement = Math.Min(grossEntitlement - taxDeducted - niDeducted, Math.Round(maximumEntitlement, 2));
                }

                weeklyResults.Add(new CompensatoryNoticePayResult()
                {
                    WeekNumber = weekIndex,
                    EmployerEntitlement = Math.Round(employerEntitlement, 2),
                    BenefitsDeducted = Math.Round(totalBenefitAmount, 2),
                    NewEmploymentDeducted = Math.Round(totalNewEmploymentAmount, 2),
                    WageIncreaseDeducted = Math.Round(totalWageIncreaseAmount, 2),
                    NotionalBenefitDeducted = Math.Round(notionalBenefitAmount, 2),
                    GrossEntitlement = grossEntitlement,
                    IsTaxable = isTaxable,
                    NotionalTaxDeducted = notationalTaxDeducted,
                    TaxDeducted = taxDeducted,
                    NIDeducted = niDeducted,
                    NetEntitlement = netEntitlement,
                    PreferentialClaim = 0m,
                    NonPreferentialClaim = netEntitlement
                });

                weekIndex++;
            }

            var response = new CompensatoryNoticePayCalculationResponseDTO()
            {
                NoticeWeeksDue = payDays.Count,
                StatutoryMax = Math.Round(statutaryMax, 2),
                MaxCNPEntitlement = Math.Round(payDays.Count * Math.Min(statutaryMax, request.WeeklyWage), 2),
                NoticeStartDate = noticeStartDate,
                ProjectedNoticeDate = projectedNoticeDate,
                CompensationEndDate = claimEndDate,
                DaysInClaim = daysInClaim,
                WeeklyResults = weeklyResults
            };

            return await Task.FromResult(response);
        }
    }
}
