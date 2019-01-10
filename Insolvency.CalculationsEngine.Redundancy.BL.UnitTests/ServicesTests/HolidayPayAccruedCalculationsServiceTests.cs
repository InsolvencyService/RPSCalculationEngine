using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class HolidayPayAccruedCalculationsServiceTests
    {
        private readonly HolidayPayAccruedCalculationService _holidayPayAccruedCalculationService;
        private readonly IOptions<ConfigLookupRoot> _options;
        public HolidayPayAccruedCalculationsServiceTests()
        {
            _holidayPayAccruedCalculationService = new HolidayPayAccruedCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_CalculatedHolidayPayAccruedResponse()
        {
            // Arrange
            var inputData = await Task.FromResult(HolidayPayAccruedTestsDataGenerator.GetValidRequestData());

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(479.00m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(260.00m);
            outputData.Result.WorkingDaysInClaim.Should().Be(56.00m);
            outputData.Result.ProRataAccruedDays.Should().Be(8.0308m);

            outputData.Result.WeeklyResults.Count.Should().Be(2);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(479.00m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(48.65m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(9.75m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(184.85m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);

            outputData.Result.WeeklyResults[1].WeekNumber.Should().Be(2);
            outputData.Result.WeeklyResults[1].MaximumEntitlement.Should().Be(275.82m);
            outputData.Result.WeeklyResults[1].EmployerEntitlement.Should().Be(147.45m);
            outputData.Result.WeeklyResults[1].GrossEntitlement.Should().Be(147.45m);
            outputData.Result.WeeklyResults[1].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[1].TaxDeducted.Should().Be(29.49m);
            outputData.Result.WeeklyResults[1].NiDeducted.Should().Be(0.00m);
            outputData.Result.WeeklyResults[1].NetEntitlement.Should().Be(117.96m);
            outputData.Result.WeeklyResults[1].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[1].GrossEntitlement);
            outputData.Result.WeeklyResults[1].NonPreferentialClaim.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayPayAccruedCalculationAsync_Return_CalculatedHolidayPayAccruedResponseWithIpConfirmedDaysOverride()
        {
            // Arrange
            var inputData = await Task.FromResult(HolidayPayAccruedTestsDataGenerator.GetRequestDataWithIpConfirmedDays());

            // Act
            var outputData = await Task.FromResult(_holidayPayAccruedCalculationService.PerformHolidayPayAccruedCalculationAsync(inputData, _options));

            // Assert
            outputData.Result.StatutoryMax.Should().Be(479.00m);
            outputData.Result.HolidaysOwed.Should().Be(28);
            outputData.Result.BusinessDaysInClaim.Should().Be(260.00m);
            outputData.Result.WorkingDaysInClaim.Should().Be(56.00m);
            outputData.Result.ProRataAccruedDays.Should().Be(5m);

            outputData.Result.WeeklyResults.Count.Should().Be(1);
            outputData.Result.WeeklyResults[0].WeekNumber.Should().Be(1);
            outputData.Result.WeeklyResults[0].MaximumEntitlement.Should().Be(479.00m);
            outputData.Result.WeeklyResults[0].EmployerEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].GrossEntitlement.Should().Be(243.25m);
            outputData.Result.WeeklyResults[0].IsTaxable.Should().Be(true);
            outputData.Result.WeeklyResults[0].TaxDeducted.Should().Be(48.65m);
            outputData.Result.WeeklyResults[0].NiDeducted.Should().Be(9.75m);
            outputData.Result.WeeklyResults[0].NetEntitlement.Should().Be(184.85m);
            outputData.Result.WeeklyResults[0].PreferentialClaim.Should().Be(outputData.Result.WeeklyResults[0].GrossEntitlement);
            outputData.Result.WeeklyResults[0].NonPreferentialClaim.Should().Be(0m);
        }
    }
}

