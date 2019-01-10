using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Holiday.Extensions
{
    public static class HolidayTakenNotPaidExtensions
    {
        public static async Task<List<DateTime>> GetHTNPDays(this List<HolidayTakenNotPaidCalculationRequestModel> data, string inputSource, DateTime startDate, DateTime endDate)
        {
            var firstRequest = data.First();

            // collate all the htnp periods into days 
            var htnpDays = new List<DateTime>();
            foreach (var req in data.Where(x => x.InputSource == inputSource))
                htnpDays.AddRange(await req.UnpaidPeriodFrom.GetBusinessDaysInRange(req.UnpaidPeriodTo, firstRequest.ShiftPattern));

            return htnpDays
                .Where(x => x >= startDate.Date && x <= endDate.Date)
                .OrderByDescending(x => x)
                .ToList();
        }
    }
}