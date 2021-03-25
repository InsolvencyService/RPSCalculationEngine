using System.Collections.Generic;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IArrearsOfPayCalculationsService
    {
        Task<ArrearsOfPayResponseDTO> PerformCalculationAsync(
            List<ArrearsOfPayCalculationRequestModel> data, string inputSource, IOptions<ConfigLookupRoot> options, TraceInfo traceInfo= null);

        Task<ArrearsOfPayResponseDTO> PerformCalculationAsync(ArrearsOfPayCalculationRequestModel data, IOptions<ConfigLookupRoot> _options, TraceInfo traceInfo = null);
        
    }
}