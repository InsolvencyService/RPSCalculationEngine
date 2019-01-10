using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IHolidayPayAccruedCalculationService
    {
        Task<HolidayPayAccruedResponseDTO> PerformHolidayPayAccruedCalculationAsync(HolidayPayAccruedCalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}