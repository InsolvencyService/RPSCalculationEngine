using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.BasicAward;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class BasicAwardCalculationService : IBasicAwardCalculationService
    {
        
        public async Task<BasicAwardCalculationResponseDTO> PerformBasicAwardCalculationAsync(
            BasicAwardCalculationRequestModel data,
            IOptions<ConfigLookupRoot> options)
        {
            DateTime date = DateTime.Today;

            var taxRate = ConfigValueLookupHelper.GetTaxRate(options, date);
            var niThreshold = ConfigValueLookupHelper.GetNIThreshold(options, date);

            var niUpperThreshold = ConfigValueLookupHelper.GetNIUpperThreshold(options, date);
            var niRate = ConfigValueLookupHelper.GetNIRate(options, date);
            var niUpperRate = ConfigValueLookupHelper.GetNIUpperRate(options, date);
            decimal taxDeducted = Math.Round(await data.BasicAwardAmount.GetTaxDeducted(taxRate, data.IsTaxable), 2);
            decimal niDeducted = Math.Round(await data.BasicAwardAmount.GetNIDeducted(niThreshold, niUpperThreshold, niRate, niUpperRate, data.IsTaxable), 2);

            var response = new BasicAwardCalculationResponseDTO()
            {
                GrossEntitlement = Math.Round(data.BasicAwardAmount, 2),
                IsTaxable = data.IsTaxable,
                TaxDeducted = taxDeducted,
                NIDeducted = niDeducted,
                NetEntitlement = Math.Round(data.BasicAwardAmount, 2) - taxDeducted - niDeducted,
                PreferentialClaim = 0m,
                NonPreferentialClaim = Math.Round(data.BasicAwardAmount, 2)
            };

            return await Task.FromResult(response);
        }
    }
}