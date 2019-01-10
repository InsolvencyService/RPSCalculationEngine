using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface ICompensatoryNoticePayCalculationService
    {
        Task<CompensatoryNoticePayCalculationResponseDTO> PerformCompensatoryNoticePayCalculationAsync(
            CompensatoryNoticePayCalculationRequestModel data, 
            IOptions<ConfigLookupRoot> options);
    }
}