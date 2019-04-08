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
            RedundancyPaymentCalculationRequestModel request, RedundancyPaymentResponseDto expectedResult)
        {

            //Act
            var actualResult = await _redundancyPaymentCalculationService.PerformRedundancyPayCalculationAsync(request, _options);

            //Assert
            Assert.IsType<RedundancyPaymentResponseDto>(actualResult);
            actualResult.AdjEmploymentStartDate.Should().Be(expectedResult.AdjEmploymentStartDate);
            actualResult.NoticeDateForRedundancyPay.Should().Be(expectedResult.NoticeDateForRedundancyPay);
            actualResult.NoticeEntitlementWeeks.Should().Be(expectedResult.NoticeEntitlementWeeks);
            actualResult.RedundancyPayWeeks.Should().Be(expectedResult.RedundancyPayWeeks);
            actualResult.YearsOfServiceUpto21.Should().Be(expectedResult.YearsOfServiceUpto21);
            actualResult.YearsOfServiceFrom22To41.Should().Be(expectedResult.YearsOfServiceFrom22To41);
            actualResult.YearsServiceOver41.Should().Be(expectedResult.YearsServiceOver41);
            actualResult.GrossEntitlement.Should().Be(expectedResult.GrossEntitlement);
            actualResult.EmployerPartPayment.Should().Be(expectedResult.EmployerPartPayment);
            actualResult.NetEntitlement.Should().Be(expectedResult.NetEntitlement);
            actualResult.PreferentialClaim.Should().Be(0m);
            actualResult.NonPreferentialClaim.Should().Be(expectedResult.NetEntitlement);
        }
    }
}