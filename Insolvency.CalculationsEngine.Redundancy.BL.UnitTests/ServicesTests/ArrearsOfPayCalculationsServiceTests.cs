using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class ArrearsOfPayCalculationsServiceTests
    {
        private readonly ArrearsOfPayCalculationsService _arrearsOfPayCalculationsService;
        private readonly IOptions<ConfigLookupRoot> _options;

        public ArrearsOfPayCalculationsServiceTests()
        {
            _arrearsOfPayCalculationsService = new ArrearsOfPayCalculationsService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_When_EmployeeStartsInFirstWeekOfClaim()
        {
            //Arrange
            var aoPayCalculationRequest = new ArrearsOfPayCalculationRequestModel
            {
                InputSource = InputSource.Rp1,
                InsolvencyDate = new DateTime(2017, 10, 02),
                EmploymentStartDate = new DateTime(2017, 06, 14),
                DismissalDate = new DateTime(2017, 10, 02),
                DateNoticeGiven = new DateTime(2017, 10, 02),
                UnpaidPeriodFrom = new DateTime(2017, 09, 01),
                UnpaidPeriodTo = new DateTime(2017, 09, 30),
                ApClaimAmount = 1141.80m,
                IsTaxable = true,
                PayDay = 6,
                ShiftPattern = new List<string> { "0", "1", "2", "5", "6" },
                WeeklyWage = 282.04m
            };
            var expectedCalculationResult = ArrearsOfPayTestsDataGenerator.GetAoPCalcResult_ForEmploymentNotStartedInFirstWeek();
            //Act
            var actualResult =
                await _arrearsOfPayCalculationsService.PerformCalculationAsync(aoPayCalculationRequest, _options);

            //Assert
            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);
            actualResult.WeeklyResult.Count.Should().Be(expectedCalculationResult.WeeklyResult.Count);
            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_When_DayBefore_ClaimFromDate_IsInShiftPattern()
        {
            var aopRequest = new ArrearsOfPayCalculationRequestModel()
            {
                InputSource = InputSource.Rp1,
                InsolvencyDate = new DateTime(2018, 04, 25),
                EmploymentStartDate = new DateTime(2016, 04, 06),
                DismissalDate = new DateTime(2018, 04, 04),
                UnpaidPeriodFrom = new DateTime(2018, 03, 03),
                DateNoticeGiven = new DateTime(2018, 04, 04),
                UnpaidPeriodTo = new DateTime(2018, 03, 31),
                ApClaimAmount = 1218.74m,
                IsTaxable = true,
                // 6 = Saturday
                PayDay = 6,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 243.75m
            };
            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp1, 489M, false, false, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 03, 03),304.68m, 489M, 0.0M, 0.0M,true,0.0M, 0.0M, 0.0M,7, 0, 489M, 0.0M, 0.0M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 03, 10),304.68m, 489M, 304.68M, 304.68M, true,60.94M, 16.64M, 227.10M, 7, 5, 489M, 304.68M, 304.68M),
                    new ArrearsOfPayWeeklyResult(3, new DateTime(2018, 03, 17),304.68m, 489M, 304.68M, 304.68M,true, 60.94M, 16.64M, 227.10M, 7, 5, 489M, 304.68M, 304.68M),
                    new ArrearsOfPayWeeklyResult(4, new DateTime(2018, 03, 24),304.68m,489M, 304.68M, 304.68M, true,60.94M, 16.64M, 227.10M, 7, 5,489M, 304.68M, 304.68M),
                    new ArrearsOfPayWeeklyResult(5, new DateTime(2018, 03, 31),304.68m,489M, 304.68M, 304.68M,true, 60.94M, 16.64M, 227.10M, 7, 5,489M, 304.68M, 304.68M)
                });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(aopRequest, _options);
            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);
            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenPeriodCrossesDNG()
        {
            var aopRequest = new ArrearsOfPayCalculationRequestModel()
            {
                InputSource = InputSource.Rp1,
                InsolvencyDate = new DateTime(2018, 10, 30),
                EmploymentStartDate = new DateTime(2016, 04, 06),
                DismissalDate = new DateTime(2018, 10, 30),
                DateNoticeGiven = new DateTime(2018, 10, 13),
                UnpaidPeriodFrom = new DateTime(2018, 10, 7),
                UnpaidPeriodTo = new DateTime(2018, 10, 20),
                ApClaimAmount = 1400m,
                IsTaxable = true,
                PayDay = 6,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 320m
            };
            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp1, 508M, true, true, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),700m, 508M, 700M, 508M,true,101.60M, 41.04M, 365.36M,7, 5, 508M, 700M, 508M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),700m, 508M, 0M, 0M,true,0M, 0M, 0M, 7, 5, 508M, 0M, 0M),
                });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(aopRequest, _options);


            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);

            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenPeriodCrossesDNGAndLowerAWW()
        {
            var aopRequest = new ArrearsOfPayCalculationRequestModel()
            {
                InputSource = InputSource.Rp1,
                InsolvencyDate = new DateTime(2018, 10, 30),
                EmploymentStartDate = new DateTime(2016, 04, 06),
                DismissalDate = new DateTime(2018, 10, 30),
                DateNoticeGiven = new DateTime(2018, 10, 13),
                UnpaidPeriodFrom = new DateTime(2018, 10, 7),
                UnpaidPeriodTo = new DateTime(2018, 10, 20),
                ApClaimAmount = 700M,
                IsTaxable = true,
                PayDay = 6,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 400m
            };
            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp1, 508M, true, true, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),350M, 508M, 350M, 350M,true,70M, 22.08M, 257.92M,7, 5, 508M, 350M, 350M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),350M, 508M, 0M, 0M,true, 0M, 0M, 0M, 7, 5, 508M, 0M, 0M),
                });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(aopRequest, _options);


            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);

            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenMultipleRequests()
        {
            var requests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 30),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 30),
                    DateNoticeGiven = new DateTime(2018, 10, 6),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                    UnpaidPeriodTo = new DateTime(2018, 10, 9),
                    ApClaimAmount = 700M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 400m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 30),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 30),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 400m
                }
            };

            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp1, 508M, true, true, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
            {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),500m, 508M, 500M, 500M,true,100M, 40.08M, 359.92M,7, 5, 508M, 500M, 500M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 257.14M, 257.14M,true,51.43M, 10.94M, 194.77M, 7, 5, 508M, 257.14M, 257.14M),
                    new ArrearsOfPayWeeklyResult(3, new DateTime(2018, 10, 20),428.57M, 508M, 0M, 0M,true,0M, 0M, 0M, 7, 4, 508M, 0M, 0M),
            });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(requests, InputSource.Rp1, _options);

            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);

            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithMultipleRequests_ThenOnlyRp1ResponsesAreReturned()
        {
            var requests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 30),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 30),
                    DateNoticeGiven = new DateTime(2018, 10, 6),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                    UnpaidPeriodTo = new DateTime(2018, 10, 9),
                    ApClaimAmount = 700M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 400m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp14a,
                    InsolvencyDate = new DateTime(2018, 10, 30),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 30),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 400m
                }
            };

            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp1, 508M, true, true, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
            {
                new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),500m, 508M, 500M, 500M,true,100M, 40.08M, 359.92M,7, 5, 508M, 500M, 500M),
                new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 0M, 0M,true,0M, 0M, 0M, 7, 2, 508M, 0M, 0M),
            });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(requests, InputSource.Rp1, _options);

            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);

            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenNoticeGivenDuringArrearsOfPay_AndLessThatOneYearsService()
        {
            var aopRequest = new ArrearsOfPayCalculationRequestModel()
            {
                InputSource = InputSource.Rp14a,
                InsolvencyDate = new DateTime(2019, 4, 3),
                EmploymentStartDate = new DateTime(2018, 9, 3),
                DismissalDate = new DateTime(2019, 3, 28),
                DateNoticeGiven = new DateTime(2019, 3, 12),
                UnpaidPeriodFrom = new DateTime(2019, 3, 1),
                UnpaidPeriodTo = new DateTime(2019, 3, 31),
                ApClaimAmount = 1452.38M,
                IsTaxable = true,
                // 6 = Saturday
                PayDay = 6,
                ShiftPattern = new List<string> { "0", "1", "2", "4", "6" },
                WeeklyWage = 302.58m
            };
            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp14a, 508M, true, true, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2019, 3, 2), 363.10M, 508M, 72.62M, 72.62M,true, 14.52M, 0M, 58.10M,7, 1, 508M, 72.62M, 72.62M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2019, 3, 9), 363.10M, 508M, 363.10M, 363.10M,true, 72.62M, 23.65M, 266.83M, 7, 5, 508M, 363.10M, 363.10M),
                    new ArrearsOfPayWeeklyResult(3, new DateTime(2019, 3, 16), 363.10M, 217.71M, 217.86M, 217.71M,true, 43.54M, 6.21M, 167.96M, 3, 5, 217.71M, 217.86M, 217.71M),
                    new ArrearsOfPayWeeklyResult(4, new DateTime(2019, 3, 23), 363.10M, 508M, 145.24M, 145.24M,true, 29.05M, 0M, 116.19M, 7, 5, 508M, 145.24M, 145.24M),
                    new ArrearsOfPayWeeklyResult(5, new DateTime(2019, 3, 30), 363.10M, 362.86M, 290.48M, 290.48M,true, 58.10M, 14.94M, 217.44M, 5, 4, 362.86M, 290.48M, 290.48M),
                });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(aopRequest, _options);


            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);

            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenInsolvencyDayBeforePay()
        {
            var aopRequest = new ArrearsOfPayCalculationRequestModel()
            {
                InputSource = InputSource.Rp1,
                InsolvencyDate = new DateTime(2018, 8, 22),
                EmploymentStartDate = new DateTime(2018, 04, 06),
                DismissalDate = new DateTime(2018, 8, 22),
                DateNoticeGiven = new DateTime(2018, 8, 22),
                UnpaidPeriodFrom = new DateTime(2018, 8, 10),
                UnpaidPeriodTo = new DateTime(2018, 8, 22),
                ApClaimAmount = 621M,
                IsTaxable = true,
                // 4 = Thursday
                PayDay = 4,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 345m
            };
            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp1, 508M, false, false, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 8, 16), 345M, 508M, 345M, 345M,true, 69M, 21.48M, 254.52M,7, 5, 508M, 345M, 345M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 8, 23), 345M, 435.43M, 276M, 276M,true, 55.2M, 13.2M, 207.6M, 6, 4, 435.43M, 276M, 276M),
                });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(aopRequest, _options);


            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);

            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }



        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenMaxEntitlementInFourMonths_EmployerEntitlementInFourMonths()
        {
            var aopRequest = new ArrearsOfPayCalculationRequestModel()
            {
                InputSource = InputSource.Rp14a,
                InsolvencyDate = new DateTime(2017, 3, 1),
                EmploymentStartDate = new DateTime(2014, 2, 1),
                DismissalDate = new DateTime(2017, 2, 28),
                DateNoticeGiven = new DateTime(2017, 1, 20),
                UnpaidPeriodFrom = new DateTime(2017, 1, 15),
                UnpaidPeriodTo = new DateTime(2018, 2, 28),
                ApClaimAmount = 5000.00M,
                IsTaxable = true,
                // 6 = Saturday
                PayDay = 6,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 400.00m
            };
            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp14a, 479M, true, true, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2017, 1, 21), 781.25M, 410.57M, 781.25M, 410.57M, true, 82.11M, 29.35M, 299.11M, 6, 5, 410.57M, 781.25M, 410.57M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2017, 1, 28), 781.25M, 479M, 0M, 0M, true, 0M, 0M, 0M, 7, 5, 479M, 0M, 0M),
                    new ArrearsOfPayWeeklyResult(3, new DateTime(2017, 2, 4), 781.25M, 479M, 0M, 0M, true, 0M, 0M, 0M, 7, 5, 479M, 0M, 0M),
                    new ArrearsOfPayWeeklyResult(4, new DateTime(2017, 2, 11), 781.25M, 479M, 781.25M, 479M, true, 95.80M, 37.56M, 345.64M, 7, 5, 479M, 781.25M, 479M),
                    new ArrearsOfPayWeeklyResult(5, new DateTime(2017, 2, 18), 781.25M, 479M, 781.25M, 479M, true, 95.80M, 37.56M, 345.64M, 7, 5, 479M, 781.25M, 479M),
                    new ArrearsOfPayWeeklyResult(6, new DateTime(2017, 2, 25), 781.25M, 479M, 781.25M, 479M, true, 95.80M, 37.56M, 345.64M, 7, 5, 479M, 781.25M, 479M),
                    new ArrearsOfPayWeeklyResult(7, new DateTime(2017, 3, 4), 781.25M, 205.29M, 312.50M, 205.29M, true, 41.06M, 4.71M, 159.52M, 3, 2, 205.29M, 312.50M, 205.29M),
                });

            var actualResult = await _arrearsOfPayCalculationsService.PerformCalculationAsync(aopRequest, _options);


            actualResult.InputSource.Should().Be(expectedCalculationResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedCalculationResult.StatutoryMax);
            actualResult.DngApplied.Should().Be(expectedCalculationResult.DngApplied);
            actualResult.RunNWNP.Should().Be(expectedCalculationResult.RunNWNP);

            for (var expectedCalcResultIndex = 0;
                expectedCalcResultIndex < expectedCalculationResult.WeeklyResult.Count;
                expectedCalcResultIndex++)
            {
                actualResult.WeeklyResult[expectedCalcResultIndex].WeekNumber.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].WeekNumber);
                actualResult.WeeklyResult[expectedCalcResultIndex].PayDate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].PayDate);
                actualResult.WeeklyResult[expectedCalcResultIndex].ApPayRate.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].ApPayRate);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].IsTaxable.Should()
                    .Be(expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].IsTaxable);
                actualResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].TaxDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NIDeducted.Should().Be(
                    expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NIDeducted);
                actualResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].NetEntitlement);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmploymentDays);
                actualResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months.Should().Be(
                        expectedCalculationResult.WeeklyResult[expectedCalcResultIndex].GrossEntitlementIn4Months);
            }
        }
    }
}