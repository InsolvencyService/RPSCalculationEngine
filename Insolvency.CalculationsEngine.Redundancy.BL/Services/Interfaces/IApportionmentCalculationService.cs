using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IApportionmentCalculationService
    {
        Task<ApportionmentCalculationResponseDTO> PerformApportionmentCalculationAsync(
            ApportionmentCalculationRequestModel data,
            IOptions<ConfigLookupRoot> options);
    }
}
