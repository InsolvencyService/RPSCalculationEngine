using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class RefundOfNotionalTaxCalculationServiceTests
    {
        private readonly RefundOfNotionalTaxCalculationService _refundOfNotionalTaxCalculationService;
        private readonly IOptions<ConfigLookupRoot> _options;

        public RefundOfNotionalTaxCalculationServiceTests()
        {
            _refundOfNotionalTaxCalculationService = new RefundOfNotionalTaxCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(RefundOfNotionalTaxTestDataHelper.RefundOfNotionalTaxRequestData))]
        public async Task PerformRefundOfNotionalTaxCalculation(
            RefundOfNotionalTaxCalculationRequestModel request1, RefundOfNotionalTaxResponseDto response1,
            RefundOfNotionalTaxCalculationRequestModel request2, RefundOfNotionalTaxResponseDto response2, RefundOfNotionalTaxCalculationRequestModel request3, RefundOfNotionalTaxResponseDto response3)
        {
            var inputData = new List<RefundOfNotionalTaxCalculationRequestModel>()
            {
                request1, request2, request3
            };

            var expectedResult = new List<RefundOfNotionalTaxResponseDto>()
            {
                response1, response2, response3
            };
            var outputResult = new List<RefundOfNotionalTaxResponseDto>();

            //Act
            foreach (var data in inputData)
            {
                var res =
                    await _refundOfNotionalTaxCalculationService.PerformRefundOfNotionalTaxCalculationAsync(data, _options);
                outputResult.Add(res);
            }
            //Assert
            int i = 0;
            foreach (var actualResult in outputResult)
            {
                var expectedCalculationResult = expectedResult[i];
                Assert.IsType<RefundOfNotionalTaxResponseDto>(actualResult);
                actualResult.TaxableEarning.Should().Be(expectedCalculationResult.TaxableEarning);
                actualResult.TaxAllowance.Should().Be(expectedCalculationResult.TaxAllowance);
                actualResult.MaximumCNPEntitlement.Should().Be(expectedCalculationResult.MaximumCNPEntitlement);
                actualResult.CnpPaid.Should().Be(expectedCalculationResult.CnpPaid);
                actualResult.CnpTaxDeducted.Should().Be(expectedCalculationResult.CnpTaxDeducted);
                actualResult.MaximumRefundLimit.Should().Be(expectedCalculationResult.MaximumRefundLimit);
                actualResult.CNPPaidAfterRefund.Should().Be(expectedCalculationResult.CNPPaidAfterRefund);
                actualResult.RefundAmount.Should().Be(expectedCalculationResult.RefundAmount);
                i++;
            }
        }
    }
}
