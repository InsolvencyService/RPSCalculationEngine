using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class APPACalculationService : IAPPACalculationService
    {
        private readonly IArrearsOfPayCalculationsService _apService;
        private readonly IProtectiveAwardCalculationService _paService;

        public APPACalculationService(IArrearsOfPayCalculationsService apService,
            IProtectiveAwardCalculationService paService)
        {
            _apService = apService;
            _paService = paService;
        }

        public async Task<APPACalculationResponseDTO> PerformCalculationAsync(APPACalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            var allWeeks = new List<IWeeklyResult>();
            var result = new APPACalculationResponseDTO();
            var rp1TraceInfo = new TraceInfo();
            var rp14TraceInfo = new TraceInfo();
            if (data.Ap != null && data.Ap.Any())
            {
                result.Ap = new ArrearsOfPayAggregateOutput();
                result.Ap.RP1ResultsList = await _apService.PerformCalculationAsync(data.Ap, InputSource.Rp1, options, rp1TraceInfo);
                result.Ap.RP14aResultsList = await _apService.PerformCalculationAsync(data.Ap, InputSource.Rp14a, options, rp14TraceInfo);

                var rp1Any = result.Ap.RP1ResultsList != null && result.Ap.RP1ResultsList.WeeklyResult.Any();
                var rp1Sum = rp1Any ? result.Ap.RP1ResultsList.WeeklyResult.Sum(x => x.NetEntitlement) : 0M;

                var rp14aAny = result.Ap.RP14aResultsList != null && result.Ap.RP14aResultsList.WeeklyResult.Any();
                var rp14aSum = rp14aAny ? result.Ap.RP14aResultsList.WeeklyResult.Sum(x => x.NetEntitlement) : 0M;

                if ((rp1Any && rp1Sum == 0) || (rp1Sum > 0 && rp1Sum < rp14aSum) || (rp1Any && !rp14aAny))
                {
                    result.Ap.SelectedInputSource = InputSource.Rp1;
                    allWeeks.AddRange(result.Ap.RP1ResultsList.WeeklyResult);
                    result.Ap.TraceInfo = await rp1TraceInfo.ConvertToJson();
                }
                else
                {
                    result.Ap.SelectedInputSource = InputSource.Rp14a;
                    allWeeks.AddRange(result.Ap.RP14aResultsList.WeeklyResult);
                    result.Ap.TraceInfo = await rp14TraceInfo.ConvertToJson();
                }
            }

            if (data.Pa != null)
            {
                result.Pa = await _paService.PerformProtectiveAwardCalculationAsync(data.Pa, options);
                allWeeks.AddRange(result.Pa.PayLines);
               
            }

            allWeeks.OrderByDescending(x => x.NetEntitlement)
                    .ThenByDescending(x => x.GrossEntitlementIn4Months)
                    .Take(8)
                    .ToList()
                    .ForEach(x => x.IsSelected = true);

            return result;
        }
    }
}