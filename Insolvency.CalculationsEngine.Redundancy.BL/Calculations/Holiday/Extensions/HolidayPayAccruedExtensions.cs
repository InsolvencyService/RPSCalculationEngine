using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Holiday.Extensions
{
    public static class HolidayPayAccruedExtensions
    {
        public static async Task<DateTime> GetHolidayYearStart(this HolidayPayAccruedCalculationRequestModel model)
        {
            return await Task.FromResult(model.EmpStartDate.Date > model.HolidayYearStart.Date ? model.EmpStartDate.Date : model.HolidayYearStart.Date);
        }

        public static async Task<DateTime> GetHolidayYearEnd(this HolidayPayAccruedCalculationRequestModel model)
        {
            var holidayStart = model.HolidayYearStart.Date;
            if (holidayStart < model.EmpStartDate.Date)
                holidayStart = model.EmpStartDate.Date;

            return await Task.FromResult(holidayStart.AddMonths(12).AddDays(-1));
        }

        public static async Task<int> GetTotalBusinessDaysInHolidayClaim(this int totalBusinessDaysInClaim, DateTime adjHolYearStart, DateTime holYearEndDate,
                                                                        List<string> shiftPattern)
        {
            int totalDaysInClaim = 0;
            string weekDayNames = await shiftPattern.GetShiftDayNames();

            for (var date = adjHolYearStart; date < holYearEndDate; date = date.AddDays(1))
            {
                if (weekDayNames.Contains(date.DayOfWeek.ToString()))
                    totalDaysInClaim++;
            }
            return await Task.FromResult(totalDaysInClaim);
        }

        public static async Task<int> GetTotalWorkingDaysInHolidayClaim(this int totalWorkingDaysInClaim,
            List<string> shiftPattern, DateTime holidayYearStart, 
            DateTime holYearEndDate, DateTime dismissalDate, DateTime insolvencyDate, DateTime empStartDate)
        {
            int totalWorkedDaysInClaim = 0;

            List<DateTime> datesList = new List<DateTime> { holidayYearStart, empStartDate };
            var workedDaysStart = datesList.Max();
            var workedDaysEnd = dismissalDate < insolvencyDate ? dismissalDate : insolvencyDate;

            string shiftDayNames = await shiftPattern.GetShiftDayNames();
            for (var date = workedDaysStart; date <= workedDaysEnd; date = date.AddDays(1))
            {
                if (shiftDayNames.Contains(date.DayOfWeek.ToString()))
                    totalWorkedDaysInClaim++;
            }

            return await Task.FromResult(totalWorkedDaysInClaim);
        }

        public static async Task<decimal> GetAdjustedHolidayEntitlement(this decimal statHolEntitlement, decimal contractHolEntitlement)
        {
            decimal adjHolidayEntitlement = Math.Max(statHolEntitlement, contractHolEntitlement);
            return await Task.FromResult(adjHolidayEntitlement);
        }

        public static async Task<decimal> GetLimitedDaysCFwd(this decimal limitedDaysCFwd, List<string> shiftPattern, decimal daysCFwd)
        {
            limitedDaysCFwd = (decimal)(shiftPattern.Count * 1.6);
            if (limitedDaysCFwd > 8)
                limitedDaysCFwd = 8;
            if (daysCFwd < limitedDaysCFwd)
                limitedDaysCFwd = daysCFwd;

            return await Task.FromResult(limitedDaysCFwd);
        }

        public static async Task<decimal> GetProRataAccruedDays(this decimal proRataAccruedDays, decimal adjHolidayEntitlement, decimal totalBusinessDaysInHolidayClaim, 
                                                                decimal totalWorkingDaysInHolidayClaim, decimal limitedDaysCFwd, decimal daysTaken, List<string> shiftPattern, decimal? ipConfDaysDue)
        {
            proRataAccruedDays = (adjHolidayEntitlement / totalBusinessDaysInHolidayClaim) * totalWorkingDaysInHolidayClaim + limitedDaysCFwd - daysTaken;
            proRataAccruedDays = Math.Max(0, proRataAccruedDays);

            if (ipConfDaysDue.HasValue)
                proRataAccruedDays = Math.Min(ipConfDaysDue.Value, proRataAccruedDays);
            proRataAccruedDays = Math.Min(6 *shiftPattern.Count, proRataAccruedDays);

            return await Task.FromResult(proRataAccruedDays);
        }

        public static async Task<decimal> GetGrossEntitlement(this decimal grossEntitlement, decimal maxEntitlement, decimal employerEntitlement )
        {
            grossEntitlement = employerEntitlement < maxEntitlement ? employerEntitlement : maxEntitlement;

            return await Task.FromResult(grossEntitlement);
        }


        public static async Task<decimal> GetIrregularProRataAccruedDays(this decimal proRataAccruedDays, decimal adjHolidayEntitlement, decimal totalBusinessDaysInHolidayClaim,
                                                                decimal totalWorkingDaysInHolidayClaim, decimal limitedDaysCFwd, decimal daysTaken, List<string> shiftPattern, decimal? holidayAccruedCore)
        {
            proRataAccruedDays = (adjHolidayEntitlement / totalBusinessDaysInHolidayClaim) * totalWorkingDaysInHolidayClaim;
            proRataAccruedDays = Math.Max(0, proRataAccruedDays);                 

            proRataAccruedDays = proRataAccruedDays + limitedDaysCFwd - daysTaken;

            proRataAccruedDays = Math.Max(0, proRataAccruedDays);

            proRataAccruedDays = Math.Min(6 * shiftPattern.Count, proRataAccruedDays);

            return await Task.FromResult(proRataAccruedDays);
        }

    }
}