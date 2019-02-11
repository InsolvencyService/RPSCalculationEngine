using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Holiday.Extensions;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Insolvency.CalculationsEngine.Redundancy.Common.Exceptions;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class HolidayPayAccruedCalculationService : IHolidayPayAccruedCalculationService
    {
        public async Task<HolidayPayAccruedResponseDTO> PerformHolidayPayAccruedCalculationAsync(
            HolidayPayAccruedCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            var response = new ResponseDto<HolidayPayAccruedResponseDTO>();
            var calculationResult = new HolidayPayAccruedResponseDTO();

            var shiftPattern = data.ShiftPattern;
            var twelveMonthsPrior = data.InsolvencyDate.Date.AddMonths(-12).AddDays(1);
            var adjHolYearStart = await data.GetHolidayYearStart();
            if (adjHolYearStart < twelveMonthsPrior)
                adjHolYearStart = twelveMonthsPrior;

            var holYearEndDate = await data.GetHolidayYearEnd();
            var statMaxWeeklyPay = ConfigValueLookupHelper.GetStatutoryMax(options, data.DismissalDate);

            // Calculate totals for holiday pay accrued
            int totalBusinessDaysInClaim = await adjHolYearStart.GetNumBusinessDaysInRange(holYearEndDate, shiftPattern);

            int totalWorkingDaysInClaim = 0;
            totalWorkingDaysInClaim = await totalWorkingDaysInClaim.GetTotalWorkingDaysInHolidayClaim(
                data.ShiftPattern, adjHolYearStart, holYearEndDate.Date, data.DismissalDate.Date, 
                data.InsolvencyDate.Date, data.EmpStartDate.Date);

            decimal limitedDaysCFwd = 0.00m;
            limitedDaysCFwd = await limitedDaysCFwd.GetLimitedDaysCFwd(shiftPattern, data.DaysCFwd.GetValueOrDefault());

            decimal statHolEntitlement = 0.00m;
            statHolEntitlement = await statHolEntitlement.GetStatutoryHolidayEntitlement(shiftPattern);

            decimal adjHolidayEntitlement = await statHolEntitlement.GetAdjustedHolidayEntitlement(data.ContractedHolEntitlement.GetValueOrDefault());

            decimal proRataAccruedDays = 0.00m;
            proRataAccruedDays = await proRataAccruedDays.GetProRataAccruedDays((decimal)adjHolidayEntitlement, totalBusinessDaysInClaim,
                                                                                totalWorkingDaysInClaim, limitedDaysCFwd, 
                                                                                data.DaysTaken.GetValueOrDefault(),
                                                                                shiftPattern,
                                                                                data.IpConfirmedDays);

            calculationResult.BusinessDaysInClaim = totalBusinessDaysInClaim;
            calculationResult.StatutoryMax = Math.Round(statMaxWeeklyPay, 2);
            calculationResult.WorkingDaysInClaim = totalWorkingDaysInClaim;
            calculationResult.HolidaysOwed = Math.Round(adjHolidayEntitlement, 4);
            calculationResult.ProRataAccruedDays = Math.Round(proRataAccruedDays, 4);

            // Calculate weekly breakdown of holiday pay accrued
            var taxRate = ConfigValueLookupHelper.GetTaxRate(options, DateTime.Now);
            var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, DateTime.Now);
            var niRate = ConfigValueLookupHelper.GetNIRate(options, DateTime.Now);

            decimal entitlementWeeks = (proRataAccruedDays / data.ShiftPattern.Count);
            int wholeWeeks = (int)Math.Floor(entitlementWeeks);
            decimal partWeekDays = (entitlementWeeks - wholeWeeks) * shiftPattern.Count;

            decimal maximumEntitlement = 0.00m;
            decimal employerEntitlement = 0.00m;

            var weeklyResults = new List<HolidayPayAccruedWeeklyResult>();
            decimal maxDays;
            decimal empDays;

            // Loop through each holiday entitlement week
            for (var weekNumber = 1; weekNumber <= Math.Ceiling(entitlementWeeks); weekNumber++)
            {
                var weeklyResult = new HolidayPayAccruedWeeklyResult();
                maxDays = 0;
                empDays = 0;

                if(weekNumber <= wholeWeeks)
                {
                    maximumEntitlement = statMaxWeeklyPay;
                    employerEntitlement = data.WeeklyWage.GetValueOrDefault();
                }

                if (weekNumber > wholeWeeks)
                {
                    // This must be the last (partial) week so loop through each day, checking for working days
                    for (var weekDayNumber = 0; weekDayNumber <= 6; weekDayNumber++)
                    {
                        if (shiftPattern.Contains(weekDayNumber.ToString()))
                        {
                            if (partWeekDays > 1)
                            {
                                partWeekDays--;
                                maxDays++;
                                empDays++;
                            }
                            else if (partWeekDays > 0)
                            {
                                maxDays = maxDays + partWeekDays;
                                empDays = empDays + partWeekDays;
                                partWeekDays = 0;
                            }
                        }
                        else if (partWeekDays > 0)
                            maxDays++;

                        if (weekDayNumber == 6)
                        {
                            maximumEntitlement = (maxDays * statMaxWeeklyPay / 7);
                            employerEntitlement = (empDays * data.WeeklyWage.GetValueOrDefault() / shiftPattern.Count);
                        }
                    }
                }

                decimal grossEntitlement = 0.00m;
                grossEntitlement = await grossEntitlement.GetGrossEntitlement(maximumEntitlement, employerEntitlement);

                decimal taxDeducted = Math.Round(await grossEntitlement.GetTaxDeducted(taxRate, (bool)data.IsTaxable), 2);
                decimal niDeducted = Math.Round(await grossEntitlement.GetNIDeducted(niThreshold, niRate, (bool)data.IsTaxable), 2);

                grossEntitlement = Math.Round(grossEntitlement, 2);
                
                weeklyResult.WeekNumber = weekNumber;
                weeklyResult.MaximumEntitlement = Math.Round(maximumEntitlement, 2);
                weeklyResult.EmployerEntitlement = Math.Round(employerEntitlement, 2);
                weeklyResult.GrossEntitlement = grossEntitlement;
                weeklyResult.IsTaxable = (bool)data.IsTaxable;
                weeklyResult.TaxDeducted = taxDeducted;
                weeklyResult.NiDeducted = niDeducted;
                weeklyResult.NetEntitlement = grossEntitlement - taxDeducted - niDeducted;
                weeklyResult.PreferentialClaim = grossEntitlement;
                weeklyResult.NonPreferentialClaim = 0m;
                weeklyResults.Add(weeklyResult);
            }

            calculationResult.WeeklyResults = weeklyResults;
            return await Task.FromResult(calculationResult);
        }
    }
}


