using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Microsoft.Extensions.Options;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IRedundanyPayCalculationsService
    {
        Task<RedundancyPaymentResponseDto> PerformRedundancyPayCalculationAsync(RedundancyPaymentCalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}