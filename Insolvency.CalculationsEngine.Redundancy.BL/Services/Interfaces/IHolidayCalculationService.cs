using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IHolidayCalculationService
    {
        Task<HolidayCalculationResponseDTO> PerformHolidayCalculationAsync(HolidayCalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}