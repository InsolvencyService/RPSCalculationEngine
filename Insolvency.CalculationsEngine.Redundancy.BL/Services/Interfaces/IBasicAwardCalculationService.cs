using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.BasicAward;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IBasicAwardCalculationService
    {
        Task<BasicAwardCalculationResponseDTO> PerformBasicAwardCalculationAsync(
            BasicAwardCalculationRequestModel data, 
            IOptions<ConfigLookupRoot> options);
    }
}