using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IProjectedNoticeDateCalculationService
    {
        Task<ProjectedNoticeDateResponseDTO> PerformProjectedNoticeDateCalculationAsync(ProjectedNoticeDateCalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}