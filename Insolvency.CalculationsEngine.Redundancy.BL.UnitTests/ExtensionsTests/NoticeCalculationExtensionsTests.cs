using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Notice.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ExtensionsTests
{
    public class NoticeCalculationExtensionsTests
    {
        private readonly IOptions<ConfigLookupRoot> _options;
        public NoticeCalculationExtensionsTests()
        {
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());

        }
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task MergePayWeeksTests()
        {
            // Arrange
            var inputData =
                new NoticeWorkedNotPaidResponseDTO("rp14a", 508m, new List<NoticeWorkedNotPaidWeeklyResult>(){
                                new NoticeWorkedNotPaidWeeklyResult()
                                    {
                                        WeekNumber = 1,
                                        PayDate = new DateTime(2018, 06, 08),
                                        MaximumEntitlement = 508m,
                                        EmployerEntitlement = 132.1m,
                                        GrossEntitlement = 132.1m,
                                        IsTaxable = true,
                                        TaxDeducted = 26.42m,
                                        NiDeducted = 0m,
                                        NetEntitlement = 105.68m,
                                        MaximumDays = 4,
                                        EmploymentDays = 2,
                                        MaximumEntitlementIn4MonthPeriod = 0m,
                                        EmployerEntitlementIn4MonthPeriod = 0m,
                                        GrossEntitlementIn4Months = 0m,
                                        IsSelected = false
                                    },
                                    new NoticeWorkedNotPaidWeeklyResult()
                                    {
                                        WeekNumber = 2,
                                        PayDate = new DateTime(2018, 6, 08),
                                        MaximumEntitlement = 508m,
                                        EmployerEntitlement = 140.02m,
                                        GrossEntitlement = 140.02m,
                                        IsTaxable = true,
                                        TaxDeducted = 36.05m,
                                        NiDeducted = 0m,
                                        NetEntitlement = 244.01m,
                                        MaximumDays = 6,
                                        EmploymentDays = 2,
                                        MaximumEntitlementIn4MonthPeriod = 0m,
                                        EmployerEntitlementIn4MonthPeriod = 0m,
                                        GrossEntitlementIn4Months = 0m,
                                        IsSelected = false
                                    },
                                    new NoticeWorkedNotPaidWeeklyResult()
                                    {
                                        WeekNumber = 3,
                                        PayDate = new DateTime(2018, 6, 15),
                                        MaximumEntitlement = 508m,
                                        EmployerEntitlement = 330.25m,
                                        GrossEntitlement = 330.25m,
                                        IsTaxable = true,
                                        TaxDeducted = 66.05m,
                                        NiDeducted = 20.19m,
                                        NetEntitlement = 244.01m,
                                        MaximumDays = 7,
                                        EmploymentDays = 5,
                                        MaximumEntitlementIn4MonthPeriod = 0m,
                                        EmployerEntitlementIn4MonthPeriod = 0m,
                                        GrossEntitlementIn4Months = 0m,
                                        IsSelected = false
                                    }
                });
            var expectedResult =
              new NoticeWorkedNotPaidResponseDTO("rp14a", 508m, new List<NoticeWorkedNotPaidWeeklyResult>(){
                                new NoticeWorkedNotPaidWeeklyResult()
                                    {
                                       //merged week 0 & 1 from the input data
                                        WeekNumber = 1,
                                        PayDate = new DateTime(2018, 06, 08),
                                        MaximumEntitlement = 508m,//max of week 0 & 1 from the input data
                                        EmployerEntitlement = 272.12m,//sum of week 0 & 1 from the input data
                                        GrossEntitlement = 272.12m,//recalculated -min of MaximumEntitlement & EmployerEntitlement
                                        IsTaxable = true,
                                        TaxDeducted = 54.42m,//recalculated based on new GrossEntitlement
                                        NiDeducted = 13.21m,//recalculated based on new GrossEntitlement
                                        NetEntitlement = 204.49m,//recalculated based on new GrossEntitlement, ni & tax
                                        MaximumDays = 6, //max of week 0 & 1 from the input data
                                        EmploymentDays = 4,//sum of week 0 & 1 from the input data
                                        MaximumEntitlementIn4MonthPeriod = 0m,//max of week 0 & 1 from the input data
                                        EmployerEntitlementIn4MonthPeriod = 0m,//sum of week 0 & 1 from the input data
                                        GrossEntitlementIn4Months = 0m,//recalculated -min of MaximumEntitlementIn4MonthPeriod & EmployerEntitlementIn4MonthPeriod
                                        IsSelected = false
                                    },
                                    new NoticeWorkedNotPaidWeeklyResult()
                                    {
                                        //weekNumber adjusted for the remaining weeks
                                        WeekNumber = 2,
                                        PayDate = new DateTime(2018, 6, 15),
                                        MaximumEntitlement = 508m,
                                        EmployerEntitlement = 330.25m,
                                        GrossEntitlement = 330.25m,
                                        IsTaxable = true,
                                        TaxDeducted = 66.05m,
                                        NiDeducted = 20.19m,
                                        NetEntitlement = 244.01m,
                                        MaximumDays = 7,
                                        EmploymentDays = 5,
                                        MaximumEntitlementIn4MonthPeriod = 0m,
                                        EmployerEntitlementIn4MonthPeriod = 0m,
                                        GrossEntitlementIn4Months = 0m,
                                        IsSelected = false
                                    }
              });


            // Act
            var actualResult = await inputData.MergePayWeeks(_options); ;

            // Assert
            Assert.IsType<NoticeWorkedNotPaidResponseDTO>(actualResult);

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
