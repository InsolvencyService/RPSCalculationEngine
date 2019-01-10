using System;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class ProjectedNoticeDateCalculationServiceTests
    {
        private readonly ProjectedNoticeDateCalculationService _service;
        private readonly IOptions<ConfigLookupRoot> _options;

        public ProjectedNoticeDateCalculationServiceTests()
        {
            _service = new ProjectedNoticeDateCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }


        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenDismissalDateEarliest()
        {
            var request = new ProjectedNoticeDateCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                DismissalDate = new DateTime(2018, 01, 01),
                DateNoticeGiven = new DateTime(2018, 01, 05)
            };

            await TestCalculation(request, new DateTime(2018, 01, 08));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenDateNoticeGivenEarliest()
        {
            var request = new ProjectedNoticeDateCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                DismissalDate = new DateTime(2018, 01, 05),
                DateNoticeGiven = new DateTime(2018, 01, 01)
            };

            await TestCalculation(request, new DateTime(2018, 01, 08));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WhenGreaterThan12YearsService()
        {
            var request = new ProjectedNoticeDateCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2002, 02, 01),
                DismissalDate = new DateTime(2018, 01, 05),
                DateNoticeGiven = new DateTime(2018, 01, 01)
            };

            await TestCalculation(request, new DateTime(2018, 03, 26));
        }

        private async Task TestCalculation(ProjectedNoticeDateCalculationRequestModel request, DateTime expectedResult)
        {
            //Act 
            var actualResult = await _service.PerformProjectedNoticeDateCalculationAsync(request,  _options);

            //Assert 
            actualResult.ProjectedNoticeDate.Should().Be(expectedResult);
        }
    }
}