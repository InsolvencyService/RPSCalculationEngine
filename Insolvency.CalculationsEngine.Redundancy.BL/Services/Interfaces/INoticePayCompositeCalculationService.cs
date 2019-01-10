using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface INoticeCalculationService
    {
        Task<NoticePayCompositeCalculationResponseDTO> PerformNoticePayCompositeCalculationAsync(NoticePayCompositeCalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}