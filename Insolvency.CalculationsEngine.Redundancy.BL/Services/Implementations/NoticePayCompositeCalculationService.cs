
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Exceptions;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Notice.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class NoticeCalculationService : INoticeCalculationService
    {
        private readonly INoticeWorkedNotPaidCalculationService _nwnpService;
        private readonly ICompensatoryNoticePayCalculationService _cnpService;

        public NoticeCalculationService()
        { }
        public NoticeCalculationService(INoticeWorkedNotPaidCalculationService nwnpService, 
            ICompensatoryNoticePayCalculationService cnpService)
        {
            _nwnpService = nwnpService;
            _cnpService = cnpService;
        }

        public async Task<NoticePayCompositeCalculationResponseDTO> PerformNoticePayCompositeCalculationAsync(NoticePayCompositeCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            var result = new NoticePayCompositeCalculationResponseDTO();
            var cnpOutput = new CompensatoryNoticePayCalculationResponseDTO();
            var nwnpOutput = new NoticeWorkedNotPaidCompositeOutput();
            var rp1TraceInfo = new TraceInfo();
            var rp14TraceInfo = new TraceInfo();

            //CNP calculation
            if (data.Cnp != null)
            {
                cnpOutput = await _cnpService.PerformCompensatoryNoticePayCalculationAsync(data.Cnp, options);
                cnpOutput.WeeklyResults.ForEach(x => x.IsSelected = true);
            }

            //NWNP calculation
            if (data.Nwnp != null && data.Nwnp.Any())
            {
                var statutoryMax = ConfigValueLookupHelper.GetStatutoryMax(options, data.Nwnp.FirstOrDefault().InsolvencyDate);

                foreach (var nwnp in data.Nwnp)
                {
                    var traceDdate = new TraceInfoDate();
                    var res = await _nwnpService.PerformNwnpCalculationAsync(nwnp, options, traceDdate);
                    if (res.InputSource == InputSource.Rp1)
                    {
                        //nwnpOutput.nwnpResults.rP1ResultsList.Add(res);
                        nwnpOutput.rp1Results.WeeklyResult.AddRange(res.WeeklyResult);
                        rp1TraceInfo.Dates.Add(traceDdate);
                    }
                    else if (res.InputSource == InputSource.Rp14a)
                    {
                        //nwnpOutput.nwnpResults.rP14aResultsList.Add(res);
                        nwnpOutput.rp14aResults.WeeklyResult.AddRange(res.WeeklyResult);
                        rp14TraceInfo.Dates.Add(traceDdate);
                    }
                }

                var rp1Total = 0m;
                var rp14aTotal = 0m;
                
                //merge pay weeks 
                nwnpOutput.rp1Results = await nwnpOutput.rp1Results.MergePayWeeks(options);
                nwnpOutput.rp1Results.InputSource = InputSource.Rp1;
                nwnpOutput.rp1Results.StatutoryMax = statutoryMax;
                rp1Total = nwnpOutput.rp1Results.WeeklyResult.Sum(x => x.NetEntitlement);

                //merge pay weeks
                nwnpOutput.rp14aResults = await nwnpOutput.rp14aResults.MergePayWeeks(options);
                nwnpOutput.rp14aResults.InputSource = InputSource.Rp14a;
                nwnpOutput.rp14aResults.StatutoryMax = statutoryMax;
                rp14aTotal = nwnpOutput.rp14aResults.WeeklyResult.Sum(x => x.NetEntitlement);

                //select the input source
                // Choose RP1 list or RP14a list with the lowest total NetEntitlement
                if ((rp1Total < rp14aTotal && rp14aTotal != 0 && rp1Total != 0) || (rp1Total > 0 && rp14aTotal == 0))
                {
                    nwnpOutput.SelectedInputSource = InputSource.Rp1;
                    nwnpOutput.rp1Results.WeeklyResult.ForEach(x => x.IsSelected = true);
                    nwnpOutput.rp14aResults.WeeklyResult.ForEach(x => x.IsSelected = false);
                    nwnpOutput.TraceInfo = await rp1TraceInfo.ConvertToJson();
                }
                else
                {
                    nwnpOutput.SelectedInputSource = InputSource.Rp14a;
                    nwnpOutput.rp1Results.WeeklyResult.ForEach(x => x.IsSelected = false);
                    nwnpOutput.rp14aResults.WeeklyResult.ForEach(x => x.IsSelected = true);
                    nwnpOutput.TraceInfo = await rp14TraceInfo.ConvertToJson();

                }
            }

            result.Cnp = cnpOutput;
            result.Nwnp = nwnpOutput;
            return result;
        }
    }
}


