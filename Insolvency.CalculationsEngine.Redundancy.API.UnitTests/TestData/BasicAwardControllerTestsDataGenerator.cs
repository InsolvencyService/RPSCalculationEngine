using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.BasicAward;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class BasicAwardControllerTestsDataGenerator
    {
        public static BasicAwardCalculationRequestModel GetValidRequest()
        {
            return new BasicAwardCalculationRequestModel
            {
                BasicAwardAmount = 500m,
                IsTaxable = true
            };
        }

        public static BasicAwardCalculationResponseDTO GetValidResponse()
        {
            return new BasicAwardCalculationResponseDTO()
            {
                GrossEntitlement = 500m,
                IsTaxable = true,
                TaxDeducted = 100m,
                NIDeducted = 40.56m,
                NetEntitlement = 359.44m,
                PreferentialClaim = 0,
                NonPreferentialClaim = 500m
            };
        }

        public static BasicAwardCalculationRequestModel GetRequestWithNegativeBasicAwardAmount()
        {
            var request = GetValidRequest();
            request.BasicAwardAmount = -1m;
            return request;
        }
    }
}


