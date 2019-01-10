using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IAPPACalculationService
    {
        Task<APPACalculationResponseDTO> PerformCalculationAsync(APPACalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}