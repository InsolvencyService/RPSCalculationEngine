using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class ApportionmentCalculationService : IApportionmentCalculationService
    {
        public async Task<ApportionmentCalculationResponseDTO> PerformApportionmentCalculationAsync(
            ApportionmentCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            decimal preferentialClaim = 0.0m;
            decimal preferentialLimit = ConfigValueLookupHelper.GetPreferentialLimit(options, DateTime.Now);
            decimal apportionmentPercentage = 1.0m;
            if (data.TupeStatus)
            {
                preferentialClaim = Math.Min(data.GrossPaidInFourMonth, preferentialLimit);
            }
            if (data.TupeStatus == false && data.TotalClaimedInFourMonth > 0.0m)
            {
                apportionmentPercentage = (data.TotalClaimedInFourMonth <= preferentialLimit) ? 1m :
                    data.GrossPaidInFourMonth / data.TotalClaimedInFourMonth;

                preferentialClaim = (data.TotalClaimedInFourMonth > preferentialLimit
                    ? preferentialLimit * apportionmentPercentage
                    : data.GrossPaidInFourMonth * apportionmentPercentage);
            }
            apportionmentPercentage = Math.Round(apportionmentPercentage * 100, 4);
            var result = new ApportionmentCalculationResponseDTO()
            {
                ApportionmentPercentage = apportionmentPercentage,
                PrefClaim = Math.Round(preferentialClaim, 2),
                NonPrefClaim = Math.Round((data.TotalClaimedInFourMonth - preferentialClaim), 2),
                TupeStatus = data.TupeStatus
            };
            return await Task.FromResult(result);
        }
    }
}

