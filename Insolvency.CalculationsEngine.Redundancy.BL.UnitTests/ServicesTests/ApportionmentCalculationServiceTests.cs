using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class ApportionmentCalculationServiceTests
    {
        private readonly ApportionmentCalculationService _service;
        private readonly IOptions<ConfigLookupRoot> _options;

        public ApportionmentCalculationServiceTests()
        {
            _service = new ApportionmentCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectPrefAndNonPrefClaimValues_WhenTupeAndExceedsLimit()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 898.00m,
                GrossEntitlement = 1002.24m,
                TotalClaimedInFourMonth = 4698.00m,
                TupeStatus = true
            };

            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should()
                .Be(ConfigValueLookupHelper.GetPreferentialLimit(_options, DateTime.Now));
            Math.Round(result.NonPrefClaim, 2).Should()
                .Be(Math.Round(request.TotalClaimedInFourMonth, 2) - Math.Round(result.PrefClaim, 2));
            result.TupeStatus.Should().Be(request.TupeStatus);
            result.ApportionmentPercentage.Should().Be(100.0m);

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectPrefAndNonPrefClaimValues_WhenTupeAndDoesNotExceedLimit()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 689.04m,
                GrossEntitlement = 689.04m,
                TotalClaimedInFourMonth = 3445.20m,
                TupeStatus = true
            };
            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should().Be(689.04m);
            Math.Round(result.NonPrefClaim, 2).Should()
                .Be(Math.Round(request.TotalClaimedInFourMonth, 2) - Math.Round(result.PrefClaim, 2));
            result.TupeStatus.Should().Be(request.TupeStatus);
            result.ApportionmentPercentage.Should().Be(100.0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectPrefAndNonPrefClaimValues_WhenNotTupeAndDoesNotExceedsLimitAndFullyPaidIn4Months()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 689.04m,
                GrossEntitlement = 689.04m,
                TotalClaimedInFourMonth = 3445.20m,
                TupeStatus = false
            };
            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should().Be(160m);
            Math.Round(result.NonPrefClaim, 2).Should()
                .Be(Math.Round(request.TotalClaimedInFourMonth, 2) - Math.Round(result.PrefClaim, 2));
            result.TupeStatus.Should().Be(request.TupeStatus);
            result.ApportionmentPercentage.Should().Be(20);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectPrefAndNonPrefClaimValues_WhenNotTupeAndDoesNotExceedsLimitAndNotFullyPaidIn4Months()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 925.00m,
                GrossEntitlement = 1835.00m,
                TotalClaimedInFourMonth = 2598.00m,
                TupeStatus = false
            };

            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should().Be(284.83m);
            Math.Round(result.NonPrefClaim, 2).Should()
                .Be(Math.Round(request.TotalClaimedInFourMonth, 2) - Math.Round(result.PrefClaim, 2));
            result.TupeStatus.Should().Be(request.TupeStatus);
            result.ApportionmentPercentage.Should().Be(35.6043m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectPrefAndNonPrefClaimValues_WhenNotTupeAndExceedsLimit()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 998.00m,
                GrossEntitlement = 998.00m,
                TotalClaimedInFourMonth = 998.00m,
                TupeStatus = false
            };

            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should()
                .Be(ConfigValueLookupHelper.GetPreferentialLimit(_options, DateTime.Now));
            Math.Round(result.NonPrefClaim, 2).Should()
                .Be(Math.Round(request.TotalClaimedInFourMonth, 2) - Math.Round(result.PrefClaim, 2));
            result.TupeStatus.Should().Be(request.TupeStatus);
            result.ApportionmentPercentage.Should().Be(100.0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectPrefAndNonPrefClaimValues_WhenFalseTupeStatusAndTotalClaimedIsZero()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 998.00m,
                GrossEntitlement = 998.00m,
                TotalClaimedInFourMonth = 0.0m,
                TupeStatus = false
            };

            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should().Be(0.00m);
            Math.Round(result.NonPrefClaim, 2).Should()
                .Be(Math.Round(request.TotalClaimedInFourMonth, 2));
            result.TupeStatus.Should().Be(request.TupeStatus);
            result.ApportionmentPercentage.Should().Be(100.0m);
        }
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectApportionmentPercentage_WhenTupeStatusIsTrue()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 925.0m,
                GrossEntitlement = 1835.0m,
                TotalClaimedInFourMonth = 2598.0m,
                TupeStatus = true
            };

            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should().Be(800.0m);
            Math.Round(result.NonPrefClaim, 2).Should().Be(1798.0m);
            result.ApportionmentPercentage.Should().Be(100.0m);    
            result.TupeStatus.Should().Be(request.TupeStatus == true);

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task ReturnsCorrectApportionmentPercentage_WhenTotalClaimedIsLessThanOrEqualTo800()
        {
            // Arrange
            var request = new ApportionmentCalculationRequestModel()
            {
                GrossPaidInFourMonth = 799.99m,
                GrossEntitlement = 800m,
                TotalClaimedInFourMonth = 800m,
                TupeStatus = false
            };

            // Act
            var result = await _service.PerformApportionmentCalculationAsync(request, _options);

            // Assert
            Math.Round(result.PrefClaim, 2).Should().Be(799.99m);
            Math.Round(result.NonPrefClaim, 2).Should().Be(0.01m);
            result.ApportionmentPercentage.Should().Be(100.0m);
            result.TupeStatus.Should().Be(false);
        }
    }
}
