using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;
using System.Collections.Generic;
using System;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class HolidayTakenNotPaidCalculationsServiceTests
    {
        private readonly HolidayTakenNotPaidCalculationService _holidayTakenNotPaidCalculationService;
        private readonly IOptions<ConfigLookupRoot> _options;
        public HolidayTakenNotPaidCalculationsServiceTests()
        {
            _holidayTakenNotPaidCalculationService = new HolidayTakenNotPaidCalculationService ();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());

        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(HolidayTakenNotPaidTestDataHelper))]
        public async Task PerformHolidayTakenNotPaidCalculation(
            HolidayTakenNotPaidCalculationRequestModel requestModel,
            string inputSource,
            decimal maxDaysInCurrentHolidayYear,
            decimal maxDaysInTotal,
            HolidayTakenNotPaidResponseDTO expectedResult
            )
        {
            //Act 1
            var request = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                requestModel
            };

            var actualResult1 =
                await _holidayTakenNotPaidCalculationService.PerformCalculationAsync(
                    request, inputSource, maxDaysInCurrentHolidayYear, maxDaysInTotal, new DateTime(2018, 1, 1), _options);

            //Assert 1
            actualResult1.InputSource.Should().Be(expectedResult.InputSource);
            actualResult1.StatutoryMax.Should().Be(expectedResult.StatutoryMax);
            actualResult1.WeeklyResult.Count.Should().Be(expectedResult.WeeklyResult.Count);
            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
                for (var actualCalcResultIndex = expectedCalcResultIndex;
                    actualCalcResultIndex <= expectedCalcResultIndex;
                    actualCalcResultIndex++)
                {
                    actualResult1.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                        .Be(expectedResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                        .Be(expectedResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                         expectedResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].NiDeducted.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].NiDeducted);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                    actualResult1.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);

                }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayTakenNotPaidCalculation_WithHTNPIn12MonthsPrior()
        {
            //Act 1
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var request = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 9, 5), new DateTime(2018, 9, 5), new DateTime(2018, 5, 14), new DateTime(2018, 5, 26), 546.57m, shiftPattern, 6, true),
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 9, 5), new DateTime(2018, 9, 5), new DateTime(2017, 9, 6),  new DateTime(2017, 9, 7),  546.57m, shiftPattern, 6, true)
            };

            var result = await _holidayTakenNotPaidCalculationService.PerformCalculationAsync(
                    request, InputSource.Rp1, 10.2146m, 11.2146m, new DateTime(2017, 9, 8), _options);

            // Assert 
            result.WeeklyResult.Count.Should().Be(4);
            result.WeeklyResult[0].PayDate.Should().Be(new DateTime(2017, 9, 9));
            result.WeeklyResult[0].EmploymentDays.Should().Be(0.7854m);
            result.WeeklyResult[0].IsSelected.Should().BeFalse();

            result.WeeklyResult[1].PayDate.Should().Be(new DateTime(2017, 9, 9));
            result.WeeklyResult[1].EmploymentDays.Should().Be(1.2146m);
            result.WeeklyResult[1].IsSelected.Should().BeTrue();

            result.WeeklyResult[2].PayDate.Should().Be(new DateTime(2018, 5, 19));
            result.WeeklyResult[2].EmploymentDays.Should().Be(5m);
            result.WeeklyResult[2].IsSelected.Should().BeTrue();

            result.WeeklyResult[3].PayDate.Should().Be(new DateTime(2018, 5, 26));
            result.WeeklyResult[3].EmploymentDays.Should().Be(5m);
            result.WeeklyResult[3].IsSelected.Should().BeTrue();
        }


        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformHolidayTakenNotPaidCalculation_WithHTNPAfterInsoolvencyDate()
        {
            //Act 1
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var request = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 9, 5), new DateTime(2018, 9, 10), new DateTime(2018, 9, 1), new DateTime(2018, 9, 10), 500m, shiftPattern, 6, true),
            };

            var result = await _holidayTakenNotPaidCalculationService.PerformCalculationAsync(
                    request, InputSource.Rp1, 10m, 12m, new DateTime(2018, 1, 1), _options);

            // Assert 
            result.WeeklyResult.Count.Should().Be(1);
            result.WeeklyResult[0].PayDate.Should().Be(new DateTime(2018, 9, 8));
            result.WeeklyResult[0].EmploymentDays.Should().Be(3m);
            result.WeeklyResult[0].IsSelected.Should().BeTrue();
        }
    }
}
