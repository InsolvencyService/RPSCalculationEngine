using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IProtectiveAwardCalculationService
    {
        Task<ProtectiveAwardResponseDTO> PerformProtectiveAwardCalculationAsync(
            ProtectiveAwardCalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}