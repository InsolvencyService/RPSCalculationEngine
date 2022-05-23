using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class NoticeWorkedNotPaidCalculationsServiceTests
    {
        private readonly NoticeWorkedNotPaidCalculationService _noticeWorkedNotPaidCalculationService;
        private readonly IOptions<ConfigLookupRoot> _options;
        public NoticeWorkedNotPaidCalculationsServiceTests()
        {
            _noticeWorkedNotPaidCalculationService = new NoticeWorkedNotPaidCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());

        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithPeriodOutside4Month()
        {
            var request = new NoticeWorkedNotPaidCalculationRequestModel()
            {
                InputSource = InputSource.Rp14a,
                EmploymentStartDate = new DateTime(2015, 8, 2),
                InsolvencyDate = new DateTime(2018, 7, 26),
                DateNoticeGiven = new DateTime(2018, 7, 20),
                DismissalDate = new DateTime(2018, 8, 8),
                UnpaidPeriodFrom = new DateTime(2018, 7, 27),
                UnpaidPeriodTo = new DateTime(2018, 8, 8),
                WeeklyWage = 320,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                PayDay = 6,
                IsTaxable = true,
                ApClaimAmount = 100
            };

            var expectedResults = new NoticeWorkedNotPaidResponseDTO(InputSource.Rp14a, 508, weeklyResult: new List<NoticeWorkedNotPaidWeeklyResult>()
            {
                new NoticeWorkedNotPaidWeeklyResult()
                {
                    WeekNumber = 1,
                    PayDate = new DateTime(2018, 8, 03),
                    MaximumEntitlement = 508m,
                    EmployerEntitlement = 55.56M,
                    GrossEntitlement = 55.56M,
                    IsTaxable = true,
                    TaxDeducted = 11.11M,
                    NiDeducted = 0m,
                    NetEntitlement = 44.45M,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 0M,
                    EmployerEntitlementIn4MonthPeriod = 0M,
                    GrossEntitlementIn4Months = 0m
                },
                new NoticeWorkedNotPaidWeeklyResult()
                {
                    WeekNumber = 2,
                    PayDate = new DateTime(2018, 8, 10),
                    MaximumEntitlement = 362.86m,
                    EmployerEntitlement = 33.33M,
                    GrossEntitlement = 33.33M,
                    IsTaxable = true,
                    TaxDeducted = 6.67M,
                    NiDeducted = 0m,
                    NetEntitlement = 26.66M,
                    MaximumDays = 5,
                    EmploymentDays = 3,
                    MaximumEntitlementIn4MonthPeriod = 0M,
                    EmployerEntitlementIn4MonthPeriod = 0m,
                    GrossEntitlementIn4Months = 0m
                }
            });

            await TestCalculation(request, expectedResults);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithPeriodIn4Month()
        {
            var request = new NoticeWorkedNotPaidCalculationRequestModel()
            {
                InputSource = InputSource.Rp14a,
                EmploymentStartDate = new DateTime(2015, 8, 2),
                InsolvencyDate = new DateTime(2018, 7, 30),
                DateNoticeGiven = new DateTime(2018, 7, 20),
                DismissalDate = new DateTime(2018, 8, 8),
                UnpaidPeriodFrom = new DateTime(2018, 7, 20),
                UnpaidPeriodTo = new DateTime(2018, 8, 8),
                WeeklyWage = 320,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                PayDay = 6,
                IsTaxable = true,
                ApClaimAmount = 100
            };

            var expectedResults = new NoticeWorkedNotPaidResponseDTO(InputSource.Rp14a, 508, weeklyResult: new List<NoticeWorkedNotPaidWeeklyResult>()
            {
                new NoticeWorkedNotPaidWeeklyResult()
                {
                    WeekNumber = 1,
                    PayDate = new DateTime(2018, 7, 27),
                    MaximumEntitlement = 508m,
                    EmployerEntitlement = 35.71m,
                    GrossEntitlement = 35.71m,
                    IsTaxable = true,
                    TaxDeducted = 7.14m,
                    NiDeducted = 0m,
                    NetEntitlement = 28.57m,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 508m,
                    EmployerEntitlementIn4MonthPeriod = 35.71m,
                    GrossEntitlementIn4Months = 35.71m
                },
                new NoticeWorkedNotPaidWeeklyResult()
                {
                    WeekNumber = 2,
                    PayDate = new DateTime(2018, 8, 03),
                    MaximumEntitlement = 508m,
                    EmployerEntitlement = 35.71m,
                    GrossEntitlement = 35.71m,
                    IsTaxable = true,
                    TaxDeducted = 7.14m,
                    NiDeducted = 0m,
                    NetEntitlement = 28.57m,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 217.71m,
                    EmployerEntitlementIn4MonthPeriod = 7.14m,
                    GrossEntitlementIn4Months = 7.14m
                },
                new NoticeWorkedNotPaidWeeklyResult()
                {
                    WeekNumber = 3,
                    PayDate = new DateTime(2018, 8, 10),
                    MaximumEntitlement = 362.86m,
                    EmployerEntitlement = 21.43m,
                    GrossEntitlement = 21.43m,
                    IsTaxable = true,
                    TaxDeducted = 4.29m,
                    NiDeducted = 0m,
                    NetEntitlement = 17.14m,
                    MaximumDays = 5,
                    EmploymentDays = 3,
                    MaximumEntitlementIn4MonthPeriod = 0m,
                    EmployerEntitlementIn4MonthPeriod = 0m,
                    GrossEntitlementIn4Months = 0m
                }
            });

            await TestCalculation(request, expectedResults);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenNoticeGivenDuringArrearsOfPay_AndLessThatOneYearsService()
        {
            var request = new NoticeWorkedNotPaidCalculationRequestModel()
            {
                InputSource = InputSource.Rp14a,
                EmploymentStartDate = new DateTime(2018, 9, 3),
                InsolvencyDate = new DateTime(2019, 4, 3),
                DateNoticeGiven = new DateTime(2019, 3, 12),
                DismissalDate = new DateTime(2019, 3, 28),
                UnpaidPeriodFrom = new DateTime(2019, 3, 1),
                UnpaidPeriodTo = new DateTime(2019, 3, 31),
                WeeklyWage = 302.58m,
                ShiftPattern = new List<string> { "0", "1", "2", "4", "6" },
                PayDay = 6,
                IsTaxable = true,
                ApClaimAmount = 1452.38M
            };

            var expectedResults = new NoticeWorkedNotPaidResponseDTO(InputSource.Rp14a, 508, weeklyResult: new List<NoticeWorkedNotPaidWeeklyResult>()
            {
                new NoticeWorkedNotPaidWeeklyResult()
                {
                    WeekNumber = 1,
                    PayDate = new DateTime(2019, 3, 19),
                    MaximumEntitlement = 508m,
                    EmployerEntitlement = 330.09m,
                    GrossEntitlement = 330.09m,
                    IsTaxable = true,
                    TaxDeducted = 66.02m,
                    NiDeducted = 18.56m,
                    NetEntitlement = 245.51m,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 508m,
                    EmployerEntitlementIn4MonthPeriod = 330.09m,
                    GrossEntitlementIn4Months = 330.09m
                }
            });

            await TestCalculation(request, expectedResults);
        }

        private async Task TestCalculation(
            NoticeWorkedNotPaidCalculationRequestModel request, NoticeWorkedNotPaidResponseDTO expectedResult)
        {
            //Act 
            var actualResult = await _noticeWorkedNotPaidCalculationService.PerformNwnpCalculationAsync(request, _options);
            
            //Assert 
            actualResult.InputSource.Should().Be(expectedResult.InputSource);
            actualResult.StatutoryMax.Should().Be(expectedResult.StatutoryMax);

            actualResult.WeeklyResult.Count.Should().Be(expectedResult.WeeklyResult.Count);
            for (var index = 0; index < expectedResult.WeeklyResult.Count; index++)
            {
                actualResult.WeeklyResult[index].WeekNumber.Should().Be(expectedResult.WeeklyResult[index].WeekNumber);
                actualResult.WeeklyResult[index].PayDate.Should().Be(expectedResult.WeeklyResult[index].PayDate);
                actualResult.WeeklyResult[index].MaximumEntitlement.Should().Be(expectedResult.WeeklyResult[index].MaximumEntitlement);
                actualResult.WeeklyResult[index].EmployerEntitlement.Should().Be(expectedResult.WeeklyResult[index].EmployerEntitlement);
                actualResult.WeeklyResult[index].GrossEntitlement.Should().Be(expectedResult.WeeklyResult[index].GrossEntitlement);
                actualResult.WeeklyResult[index].IsTaxable.Should().Be(expectedResult.WeeklyResult[index].IsTaxable);
                actualResult.WeeklyResult[index].TaxDeducted.Should().Be(expectedResult.WeeklyResult[index].TaxDeducted);
                actualResult.WeeklyResult[index].NiDeducted.Should().Be(expectedResult.WeeklyResult[index].NiDeducted);
                actualResult.WeeklyResult[index].NetEntitlement.Should().Be(expectedResult.WeeklyResult[index].NetEntitlement);
                actualResult.WeeklyResult[index].MaximumDays.Should().Be(expectedResult.WeeklyResult[index].MaximumDays);
                actualResult.WeeklyResult[index].EmploymentDays.Should().Be(expectedResult.WeeklyResult[index].EmploymentDays);
                actualResult.WeeklyResult[index].MaximumEntitlementIn4MonthPeriod.Should().Be(expectedResult.WeeklyResult[index].MaximumEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[index].EmployerEntitlementIn4MonthPeriod.Should().Be(expectedResult.WeeklyResult[index].EmployerEntitlementIn4MonthPeriod);
                actualResult.WeeklyResult[index].GrossEntitlementIn4Months.Should().Be(expectedResult.WeeklyResult[index].GrossEntitlementIn4Months);
            }
        }
    }
}
