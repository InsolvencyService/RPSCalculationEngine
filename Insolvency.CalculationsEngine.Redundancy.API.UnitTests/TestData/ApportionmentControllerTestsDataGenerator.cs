using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public class ApportionmentControllerTestsDataGenerator
    {
        public static ApportionmentCalculationRequestModel GetValidRequest()
        {
            return new ApportionmentCalculationRequestModel()
            {
                GrossEntitlement = 0.0m,
                GrossPaidInFourMonth = 0.0m,
                TotalClaimedInFourMonth = 0.0m,
                TupeStatus = false
            };
        }
        public static ApportionmentCalculationRequestModel GetValidRequestWithDataExampleOne()
        {
            return new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 689.04m,
                GrossEntitlement = 689.04m,
                TotalClaimedInFourMonth = 3445.20m,
                TupeStatus = true
            };
        }
        public static ApportionmentCalculationResponseDTO GetValidResponseForExampleOne()
        {
            return new ApportionmentCalculationResponseDTO()
            {
                PrefClaim = 689.04m,
                ApportionmentPercentage = 0.0m,
                TupeStatus = true,
                NonPrefClaim = 2756.16m
            };
        }


        public static ApportionmentCalculationRequestModel GetBadPayload()
        {
            return null;
        }
        public static ApportionmentCalculationRequestModel GetRequestWithNegativeGrossPaidInFourMonth()
        {
            return new ApportionmentCalculationRequestModel()
            {
                GrossEntitlement = 0.0m,
                GrossPaidInFourMonth = -1000.0m,
                TotalClaimedInFourMonth = 0.0m,
                TupeStatus = false
            };
        }
        public static ApportionmentCalculationRequestModel GetRequestWithNegativeGrossEntitlement()
        {
            return new ApportionmentCalculationRequestModel()
            {
                GrossEntitlement = -1000.0m,
                GrossPaidInFourMonth = 0.0m,
                TotalClaimedInFourMonth = 0.0m,
                TupeStatus = false
            };
        }
        public static ApportionmentCalculationRequestModel GetRequestWithNegativeTotalClaimed()
        {
            return new ApportionmentCalculationRequestModel()
            {
                GrossEntitlement = 0.0m,
                GrossPaidInFourMonth = 0.0m,
                TotalClaimedInFourMonth = -1000.0m,
                TupeStatus = false
            };
        }
        public static ApportionmentCalculationRequestModel GetRequestWithNoTupeStatus()
        {
            return new ApportionmentCalculationRequestModel()
            {
                GrossEntitlement = 0.0m,
                GrossPaidInFourMonth = 0.0m,
                TotalClaimedInFourMonth = -1000.0m,
            };
        }
    }
}
