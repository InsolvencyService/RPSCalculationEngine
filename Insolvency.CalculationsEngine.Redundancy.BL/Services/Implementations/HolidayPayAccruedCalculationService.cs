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
using System.Collections;
using System.Threading;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Diagnostics.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;

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
            var statMaxWeeklyPay = ConfigValueLookupHelper.GetStatutoryMax(options, data.InsolvencyDate);

            // Calculate totals for holiday pay accrued
            int totalBusinessDaysInClaim = await data.HolidayYearStart.GetNumBusinessDaysInRange(holYearEndDate, shiftPattern);

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
                    var weekDayNumber = data.PayDay;

                    // This must be the last (partial) week so loop through each day, checking for working days
                    for (var day = 0; day <= 6; day++)
                    {
                        weekDayNumber++;
                        if (weekDayNumber > (int) DayOfWeek.Saturday)
                        {
                            weekDayNumber = (int) DayOfWeek.Sunday;
                        }

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
                       

                        if (day == 6)
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

        public async Task<HolidayPayAccruedResponseDTO> PerformHolidayPayAccruedForIrregularHoursWorkersCalculationAsync(IrregularHolidayPayAccruedCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            var calculationResult = new HolidayPayAccruedResponseDTO();

            var shiftPattern = data.ShiftPattern;

            /////Step-1 -Subtract 12 months from the insolvency date and add one day

            var twelveMonthsPrior = data.InsolvencyDate.Date.AddMonths(-12).AddDays(1);

            /////2. If the holiday year start is before the date from step 1, 
            /////replace the holiday year start with the value from step 1 as the adjusted holiday year start.

           var adjHolYearStart = await data.GetHolidayYearStart();
            if (adjHolYearStart < twelveMonthsPrior)
                adjHolYearStart = twelveMonthsPrior;

            /////3. Get the statutory maximum applicable on the adjusted holiday year start from step 2 from the config table

            var holYearEndDate = await data.GetHolidayYearEnd();
            var statMaxWeeklyPay = ConfigValueLookupHelper.GetStatutoryMax(options, data.InsolvencyDate);

            ///4. Calculate the total business days in claim using the common procedure “Get Number of Business Days in Range” with the following values:
            ///a.startDate: the calculation input “Holday Year Start” (not the adjusted date)
            ///b.endDate: the calculation input “Holday Year End”
            ///c.shiftPattern: the calculation input “Shift Pattern”
            // Calculate totals for holiday pay accrued
            int totalBusinessDaysInClaim = await data.HolidayYearStart.GetNumBusinessDaysInRange(holYearEndDate, shiftPattern);

            ///5. Find the total working days in claim using a sub-calculation called “Get Total Working Days In Holiday Claim”:
            ///a.Find the later of the adjusted holiday year start from step 2 and the calcualation input “Employment start date”
            ///b.Find the earlier of the calculation input “dismissal date” and the calcualation input “insolvency date”
            ///c.Follow the same procedure as in “Get number of business days in range
            ///
            int totalWorkingDaysInClaim = 0;
            totalWorkingDaysInClaim = await totalWorkingDaysInClaim.GetTotalWorkingDaysInHolidayClaim(
                data.ShiftPattern, adjHolYearStart, holYearEndDate.Date, data.DismissalDate.Date,
                data.InsolvencyDate.Date, data.EmpStartDate.Date);

            ///6. Find the total working days in claim using a sub-calculation called “Get Limited Days Carried Forward”:
            ///a.Multiply the number of days in the shift pattern by 1.8
            ///b.If the number of days is greater than 8, cap it at 8
            /// (c) If the result is less than the calculation input “DaysCForward” then cap the value at 8
            /// UNLESS HolidaysCarriedOverCoreSource = “Override” in which case, use the actual value in DaysCForward
            /// (ie. this will already be set to be the override figure in the Calling Code)

            decimal limitedDaysCFwd = 0.00m;
            if(data.HolidaysCarriedOverCoreSource == InputSource.Override)
            {
                limitedDaysCFwd =data.DaysCFwd.GetValueOrDefault();
            }
            else
            {
                limitedDaysCFwd = await limitedDaysCFwd.GetLimitedDaysCFwd(shiftPattern, data.DaysCFwd.GetValueOrDefault());
            }
           

            ///7. Find the statutory holiday entitlement using a sub-calculation called “Get statutory holiday entitlement”:
            ///a.Multiply the number of days in the shift pattern by 5.6
            ///b.If the number of days is greater than 28, cap it at 28

            decimal statHolEntitlement = 0.00m;
            statHolEntitlement = await statHolEntitlement.GetStatutoryHolidayEntitlement(shiftPattern);

            ///8. Find the adjusted holiday entitlement using a sub-calculation called “Get adjusted holiday entitlement”:
            /// a.Return the greater of the statutory holiday entitlement from step 7 and the calculation input “Contracted holiday entitlement”
           /// decimal contractedHolentitlement = ConfigValueLookupHelper.Get_Irregular_Hour_Worker_ContractedHolEntitlement(options);
            decimal holidayAccruedDaysCore = data.HolidayAccruedDaysCore.GetValueOrDefault();
            decimal adjHolidayEntitlement = await statHolEntitlement.GetAdjustedHolidayEntitlement(holidayAccruedDaysCore);

            ///9.Find the statutory holiday entitlement using a sub-calculation called “Get pro-rata accrued days”
            /// a.Divide the adjusted holiday entitlement(step 8) by the total business days in claim(step 4)
            ///b.Multiply that number by the total working days in holiday claim(step 5)
            ///Removed- New Step after (b) - Step 9bb. Compare Holidays Accrued Core and the result of 9b.  Take the lowest of the 2 figures.
            ///c.Add the limited days carried forward(step 6)
            ///d.Subtract the calculation input “days taken”
            ///e. Don't perform the step e
            ///f.Multiply the days in the shift pattern by 6, and cap at that value


            decimal proRataAccruedDays = 0.00m;
            proRataAccruedDays = await proRataAccruedDays.GetIrregularProRataAccruedDays((decimal)adjHolidayEntitlement, totalBusinessDaysInClaim,
                                                                                totalWorkingDaysInClaim, limitedDaysCFwd,
                                                                                data.DaysTaken.GetValueOrDefault(),
                                                                                shiftPattern,
                                                                                data.HolidayAccruedDaysCore);

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

                if (weekNumber <= wholeWeeks)
                {
                    maximumEntitlement = statMaxWeeklyPay;
                    employerEntitlement = data.WeeklyWage.GetValueOrDefault();
                }

                if (weekNumber > wholeWeeks)
                {
                    var weekDayNumber = data.PayDay;

                    // This must be the last (partial) week so loop through each day, checking for working days
                    for (var day = 0; day <= 6; day++)
                    {
                        weekDayNumber++;
                        if (weekDayNumber > (int)DayOfWeek.Saturday)
                        {
                            weekDayNumber = (int)DayOfWeek.Sunday;
                        }

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


                        if (day == 6)
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


