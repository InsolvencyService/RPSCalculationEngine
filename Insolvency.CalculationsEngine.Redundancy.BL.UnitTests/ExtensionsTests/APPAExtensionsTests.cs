using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.APPA.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ExtensionsTests
{
    public class APPAExtensionsTests
    {
        private readonly IOptions<ConfigLookupRoot> _options;

        public APPAExtensionsTests()
        {
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task MergeWeeklyResults_ReturnsEmptyResults_WhenNoResponsesAreSupplied()
        {
            //Arrange
            var responses = new List<ArrearsOfPayResponseDTO>();

            //Act
            var result = await responses.MergeWeeklyResults(InputSource.Rp1, _options);

            //Assert
            result.Should().NotBeNull();
            result.InputSource.Should().Be(InputSource.Rp1);
            result.StatutoryMax.Should().Be(525M);
            result.DngApplied.Should().BeFalse();
            result.RunNWNP.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task MergeWeeklyResults_ReturnsSingleResponse_WhenDisjointWeeks()
        {
            //Arrange
            var responses = new List<ArrearsOfPayResponseDTO>()
            {
                new ArrearsOfPayResponseDTO()
                {
                    StatutoryMax = 508m,
                    InputSource = "rp1",
                    DngApplied = true,
                    RunNWNP= true,
                    WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                    {
                        new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),700m, 508M, 700M, 508M,true,101.60M, 41.52M, 364.88M,7, 5, 508M, 700M, 508M),
                    }
                },
                new ArrearsOfPayResponseDTO()
                {
                    StatutoryMax = 508m,
                    InputSource = "rp1",
                    DngApplied = true,
                    RunNWNP= true,
                    WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                    {
                        new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),700m, 508M, 380M, 380M,true,76M, 26.16M, 277.84M, 7, 5, 508M, 380M, 380M),
                    }
                }
            };

            //Act
            var result = await responses.MergeWeeklyResults(InputSource.Rp1, _options);

            //Assert
            result.StatutoryMax.Should().Be(508M);
            result.InputSource.Should().Be("rp1");
            result.DngApplied.Should().BeTrue();
            result.RunNWNP.Should().BeTrue();
            result.WeeklyResult.Count.Should().Be(2);
            result.WeeklyResult[0].WeekNumber.Should().Be(1);
            result.WeeklyResult[0].PayDate.Should().Be(new DateTime(2018, 10, 6));
            result.WeeklyResult[0].GrossEntitlement.Should().Be(508M);
            result.WeeklyResult[1].WeekNumber.Should().Be(2);
            result.WeeklyResult[1].PayDate.Should().Be(new DateTime(2018, 10, 13));
            result.WeeklyResult[1].GrossEntitlement.Should().Be(380M);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task MergeWeeklyResults_ReturnsSingleResponse_WhenMatchingWeeks()
        {
            //Arrange
            var responses = new List<ArrearsOfPayResponseDTO>()
            {
                new ArrearsOfPayResponseDTO()
                {
                    StatutoryMax = 508m,
                    InputSource = InputSource.Rp1,
                    DngApplied = true,
                    RunNWNP= true,
                    WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                    {
                        new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),500m, 508M, 500M, 500M,true,100M, 40.56M, 359.44M,7, 5, 508M, 500M, 500M),
                        new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 40M, 40M,true,8M, 0M, 32M, 7, 2, 508M, 40M, 40M),

                    }
                },
                new ArrearsOfPayResponseDTO()
                {
                    StatutoryMax = 508m,
                    InputSource = InputSource.Rp1,
                    DngApplied = true,
                    RunNWNP= true,
                    WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                    {
                        new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),428.57M, 508M, 257.14M, 257.14M,true,51.43M, 11.42M, 194.29M, 7, 3, 508M, 257.14M, 257.14M),
                        new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 18.29M, 7, 4, 508M, 22.86M, 22.86M),
                    }
                }
            };

            //Act
            var result = await responses.MergeWeeklyResults(InputSource.Rp1, _options);

            //Assert
            result.StatutoryMax.Should().Be(508M);
            result.InputSource.Should().Be(InputSource.Rp1);
            result.DngApplied.Should().BeTrue();
            result.RunNWNP.Should().BeTrue();
            result.WeeklyResult.Count.Should().Be(3);
            result.WeeklyResult[0].WeekNumber.Should().Be(1);
            result.WeeklyResult[0].PayDate.Should().Be(new DateTime(2018, 10, 6));
            result.WeeklyResult[0].GrossEntitlement.Should().Be(500M);

            result.WeeklyResult[1].WeekNumber.Should().Be(2);
            result.WeeklyResult[1].PayDate.Should().Be(new DateTime(2018, 10, 13));
            result.WeeklyResult[1].ApPayRate.Should().Be(500M);
            result.WeeklyResult[1].MaximumEntitlement.Should().Be(508M);
            result.WeeklyResult[1].EmployerEntitlement.Should().Be(297.14M);
            result.WeeklyResult[1].GrossEntitlement.Should().Be(297.14M);
            result.WeeklyResult[1].IsTaxable.Should().BeTrue();
            result.WeeklyResult[1].TaxDeducted.Should().Be(59.43M);
            result.WeeklyResult[1].NIDeducted.Should().Be(15.74M);
            result.WeeklyResult[1].NetEntitlement.Should().Be(221.97M);
            result.WeeklyResult[1].MaximumDays.Should().Be(7);
            result.WeeklyResult[1].EmploymentDays.Should().Be(5);
            result.WeeklyResult[1].MaximumEntitlementIn4MonthPeriod.Should().Be(508M);
            result.WeeklyResult[1].EmployerEntitlementIn4MonthPeriod.Should().Be(297.14M);
            result.WeeklyResult[1].GrossEntitlementIn4Months.Should().Be(297.14M);

            result.WeeklyResult[2].WeekNumber.Should().Be(3);
            result.WeeklyResult[2].PayDate.Should().Be(new DateTime(2018, 10, 20));
            result.WeeklyResult[2].GrossEntitlement.Should().Be(22.86M);
        }
    }
}