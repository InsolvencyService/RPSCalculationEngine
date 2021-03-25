using System;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IHolidayTakenNotPaidCalculationService
    {
        Task<HolidayTakenNotPaidResponseDTO> PerformCalculationAsync(
            List<HolidayTakenNotPaidCalculationRequestModel> data,
            string inputSource,
            decimal maxDaysInCurrentHolidayYear,
            decimal maxDaysInTotal,
            DateTime? holidayYearStart,
            IOptions<ConfigLookupRoot> options,
            TraceInfo traceInfo = null);
    }
}
