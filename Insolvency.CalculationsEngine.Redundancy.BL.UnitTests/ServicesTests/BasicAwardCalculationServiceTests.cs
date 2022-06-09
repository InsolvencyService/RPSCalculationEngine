using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.BasicAward;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class BasicAwardCalculationServiceTests
    {
        private readonly BasicAwardCalculationService _service;
        private readonly IOptions<ConfigLookupRoot> _options;

        public BasicAwardCalculationServiceTests()
        {
            _service = new BasicAwardCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenIsTaxableIsFalse()
        {
            var request = new BasicAwardCalculationRequestModel
            {
                BasicAwardAmount = 500m,
                IsTaxable = false
            };

            var response = await _service.PerformBasicAwardCalculationAsync(request, _options);

            response.GrossEntitlement.Should().Be(request.BasicAwardAmount);
            response.IsTaxable.Should().Be(request.IsTaxable);
            response.TaxDeducted.Should().Be(0m);
            response.NIDeducted.Should().Be(0m);
            response.NetEntitlement.Should().Be(500m);
            response.PreferentialClaim.Should().Be(0m);
            response.NonPreferentialClaim.Should().Be(response.GrossEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenBasicAwardAmountIsLessThanLowerThreshold()
        {
            var request = new BasicAwardCalculationRequestModel
            {
                BasicAwardAmount = 100m,
                IsTaxable = true
            };

            var response = await _service.PerformBasicAwardCalculationAsync(request, _options);

            response.GrossEntitlement.Should().Be(request.BasicAwardAmount);
            response.IsTaxable.Should().Be(request.IsTaxable);
            response.TaxDeducted.Should().Be(20m);
            response.NIDeducted.Should().Be(0m);
            response.NetEntitlement.Should().Be(80m);
            response.PreferentialClaim.Should().Be(0m);
            response.NonPreferentialClaim.Should().Be(response.GrossEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenBasicAwardAmountIsBetweenLowerAndUpperThreshold()
        {
            var request = new BasicAwardCalculationRequestModel
            {
                BasicAwardAmount = 500m,
                IsTaxable = true
            };

            var response = await _service.PerformBasicAwardCalculationAsync(request, _options);

            response.GrossEntitlement.Should().Be(request.BasicAwardAmount);
            response.IsTaxable.Should().Be(request.IsTaxable);
            response.TaxDeducted.Should().Be(100m);
            response.NIDeducted.Should().Be(41.08m);
            response.NetEntitlement.Should().Be(358.92m);
            response.PreferentialClaim.Should().Be(0m);
            response.NonPreferentialClaim.Should().Be(response.GrossEntitlement);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenBasicAwardAmountIsGreaterThanUpperThreshold()
        {
            var request = new BasicAwardCalculationRequestModel
            {
                BasicAwardAmount = 1000m,
                IsTaxable = true
            };

            var response = await _service.PerformBasicAwardCalculationAsync(request, _options);

            response.GrossEntitlement.Should().Be(request.BasicAwardAmount);
            response.IsTaxable.Should().Be(request.IsTaxable);
            response.TaxDeducted.Should().Be(200m);
            response.NIDeducted.Should().Be(104.02m);
            response.NetEntitlement.Should().Be(695.98m);
            response.PreferentialClaim.Should().Be(0m);
            response.NonPreferentialClaim.Should().Be(response.GrossEntitlement);
        }
    }
}


