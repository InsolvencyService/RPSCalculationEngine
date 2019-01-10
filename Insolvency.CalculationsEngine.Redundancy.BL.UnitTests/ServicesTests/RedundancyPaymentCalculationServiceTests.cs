using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class RedundancyPaymentCalculationServiceTests
    {
        private readonly RedundancyPaymentCalculationsService _redundancyPaymentCalculationService;
        private readonly IOptions<ConfigLookupRoot> _options;

        public RedundancyPaymentCalculationServiceTests()
        {
            _redundancyPaymentCalculationService = new RedundancyPaymentCalculationsService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(RedundancyPaymentTestDataHelper.RedundancyPaymentRequestData))]
        public async Task PerformRedundancyPaymentCalculationForAllAgeGroups(
            RedundancyPaymentCalculationRequestModel request1, RedundancyPaymentResponseDto response1,
            RedundancyPaymentCalculationRequestModel request2, RedundancyPaymentResponseDto response2,
            RedundancyPaymentCalculationRequestModel request3, RedundancyPaymentResponseDto response3,
            RedundancyPaymentCalculationRequestModel request4, RedundancyPaymentResponseDto response4,
            RedundancyPaymentCalculationRequestModel request5, RedundancyPaymentResponseDto response5)
        {
            var inputData = new List<RedundancyPaymentCalculationRequestModel>()
            {
                request1, request2, request3, request4, request5
            };
           
            var expectedResult = new List<RedundancyPaymentResponseDto>()
            {
                response1, response2, response3, response4, response5
            };
            var outputResult = new List<RedundancyPaymentResponseDto>();

            //Act
            foreach (var data in inputData)
            {
                var res =
                    await _redundancyPaymentCalculationService.PerformRedundancyPayCalculationAsync(data, _options);
                outputResult.Add(res);
            }
            //Assert
            int i = 0;
            foreach (var actualResult in outputResult)
            {
                var expectedCalculationResult = expectedResult[i];
                Assert.IsType<RedundancyPaymentResponseDto>(actualResult);
                actualResult.AdjEmploymentStartDate.Should().Be(expectedCalculationResult.AdjEmploymentStartDate);
                actualResult.NoticeDateForRedundancyPay.Should().Be(expectedCalculationResult.NoticeDateForRedundancyPay);
                actualResult.NoticeEntitlementWeeks.Should().Be(expectedCalculationResult.NoticeEntitlementWeeks);
                actualResult.RedundancyPayWeeks.Should().Be(expectedCalculationResult.RedundancyPayWeeks);
                actualResult.YearsOfServiceUpto21.Should().Be(expectedCalculationResult.YearsOfServiceUpto21);
                actualResult.YearsOfServiceFrom22To41.Should().Be(expectedCalculationResult.YearsOfServiceFrom22To41);
                actualResult.YearsServiceOver41.Should().Be(expectedCalculationResult.YearsServiceOver41);
                actualResult.GrossEntitlement.Should().Be(expectedCalculationResult.GrossEntitlement);
                actualResult.EmployerPartPayment.Should().Be(expectedCalculationResult.EmployerPartPayment);
                actualResult.NetEntitlement.Should().Be(expectedCalculationResult.NetEntitlement);
                actualResult.PreferentialClaim.Should().Be(0m);
                actualResult.NonPreferentialClaim.Should().Be(expectedCalculationResult.GrossEntitlement);
                i++;
            }
        }
    }
}