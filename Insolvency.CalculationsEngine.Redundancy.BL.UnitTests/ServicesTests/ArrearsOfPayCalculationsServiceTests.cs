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
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),700m, 508M, 380M, 380M,true,76M, 25.68M, 278.32M, 7, 5, 508M, 380M, 380M),
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
            var expectedCalculationResult = new ArrearsOfPayResponseDTO(InputSource.Rp1, 508M, true, false, weeklyResult: new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),350M, 508M, 350M, 350M,true,70M, 22.08M, 257.92M,7, 5, 508M, 350M, 350M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),350M, 508M, 0M, 0M,true, 0M, 0M, 0M, 7, 5, 508M, 350M, 350M),
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
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 297.14M, 297.14M,true,59.43M, 15.74M, 221.97M, 7, 5, 508M, 297.14M, 297.14M),
                    new ArrearsOfPayWeeklyResult(3, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 18.29M, 7, 4, 508M, 22.86M, 22.86M),
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
                new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 40M, 40M,true,8M, 0M, 32M, 7, 2, 508M, 40M, 40M),
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
    }
}