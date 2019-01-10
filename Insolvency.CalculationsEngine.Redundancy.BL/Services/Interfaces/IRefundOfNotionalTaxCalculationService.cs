using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Microsoft.Extensions.Options;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces
{
    public interface IRefundOfNotionalTaxCalculationService
    {
        Task<RefundOfNotionalTaxResponseDto> PerformRefundOfNotionalTaxCalculationAsync(RefundOfNotionalTaxCalculationRequestModel data, IOptions<ConfigLookupRoot> options);
    }
}