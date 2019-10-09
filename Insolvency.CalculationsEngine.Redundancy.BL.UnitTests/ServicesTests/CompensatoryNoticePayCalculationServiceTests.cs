using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class CompensatoryNoticePayCalculationServiceTests
    {
        private readonly CompensatoryNoticePayCalculationService _service;
        private readonly IOptions<ConfigLookupRoot> _options;

        public CompensatoryNoticePayCalculationServiceTests()
        {
            _service = new CompensatoryNoticePayCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }


        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForAfter5thApril2018()
        {
            var request = CompensatoryNoticePayControllerTestsDataGenerator.GetValidRequest();

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);


            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(61.72m);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(136.42m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(109.14m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(27.28m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(41.150m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(203.39m);
            response.WeeklyResults[1].NIDeducted.Should().Be(4.49m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(158.22m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(85.71m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(40.68m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForBefore5thApril2018()
        {
            var request = CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithDismissalDateBefore5thApril2018();

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 3, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 3, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(489m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 3, 15));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(61.72M);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(109.14m);
            response.WeeklyResults[0].IsTaxable.Should().BeFalse();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(109.14m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(27.28m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(41.15m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(162.71m);
            response.WeeklyResults[1].NIDeducted.Should().Be(0m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(162.71m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(85.71m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(40.68m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForBenefitHigherThanWeekly()
        {
            var request = CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithBenefitHigherThanWeeklyWage();

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(375m);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(0m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(0m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);
            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].NIDeducted.Should().Be(19.71m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(244.49m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(66.05m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForNewEmploymentHigherThanWeekly()
        {
            var request = CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithNewEmploymentHigherThanWeeklyWage();

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);
            
            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(0m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(0m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(375m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].NIDeducted.Should().Be(19.71m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(244.49m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(66.05m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForWageIncreaseHigherThanWeekly()
        {
            var request = CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithWageIncreaseHigherThanWeeklyWage();

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);
            
            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(0m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(0m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(375m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].NIDeducted.Should().Be(19.71m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(244.49m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(66.05m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForWhenThereIsDeceasedDate()
        {
            var request = CompensatoryNoticePayControllerTestsDataGenerator.GetRequestWithDeceasedDate();

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 11));
            response.DaysInClaim.Should().Be(4);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 8, 24));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(3.86m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(154.66m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(39.63m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(66.05m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(66.05m);
            response.WeeklyResults[1].NIDeducted.Should().Be(0m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(52.84m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(13.21m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForNotationalBenefitOverrides()
        {
            var request = new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                NotionalBenefitOverrides = new List<CompensatoryNoticePayNotionalBenefitOverride>()
                {
                    new CompensatoryNoticePayNotionalBenefitOverride()
                    {
                        NotionalBenefitOverrideStartDate = new DateTime(2018, 06, 05),
                        NotionalBenefitOverrideEndDate = new DateTime(2018, 06, 08),
                    }
                }
            };

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(3.86m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(154.66m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(39.63m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].NIDeducted.Should().Be(19.71m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(244.49m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(66.05m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }


        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_GrossEntitlementDoesNotExceedStatMax()
        {
            var request = new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2014, 10, 30),
                InsolvencyDate = new DateTime(2016, 10, 30),
                DismissalDate = new DateTime(2016, 10, 30),
                DateNoticeGiven = new DateTime(2016, 10, 30),
                WeeklyWage = 650m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1976, 10, 30),
                DeceasedDate = null
            };

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2016, 11, 13));
            response.DaysInClaim.Should().Be(10);
            response.NoticeStartDate.Should().Be(new DateTime(2016, 10, 31));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(958m);
            response.StatutoryMax.Should().Be(479m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2016, 11, 13));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(650m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(479m);
            response.WeeklyResults[0].IsTaxable.Should().BeFalse();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(479m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(130m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(650m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(461.49m);
            response.WeeklyResults[1].NIDeducted.Should().Be(0m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(461.49m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(73.14m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(115.37m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForBenefitWithNoEndDate()
        {
            var request = new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                Benefits = new List<CompensatoryNoticePayBenefit>()
                {
                    new CompensatoryNoticePayBenefit()
                    {
                        BenefitStartDate = new DateTime(2018, 06, 05),
                        BenefitAmount = 220
                    }
                }
            };

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(60m);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(138.15m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(110.52m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(27.63m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(140m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(190.25m);
            response.WeeklyResults[1].NIDeducted.Should().Be(2.91m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(149.29m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(38.05m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForNewEmploymentWithNoEndDate()
        {
            var request = new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                NewEmployments = new List<CompensatoryNoticePayNewEmployment>()
                {
                    new CompensatoryNoticePayNewEmployment()
                    {
                        NewEmploymentStartDate = new DateTime(2018, 06, 05),
                        NewEmploymentWage = 220
                    }
                }
            };

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(138.15m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(110.52m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(60m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(27.63m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(190.25m);
            response.WeeklyResults[1].NIDeducted.Should().Be(2.91m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(149.29m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(140m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(38.05m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForWageIncreaseWithNoEndDate()
        {
            var request = new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                WageIncreases = new List<CompensatoryNoticePayWageIncrease>()
                {
                    new CompensatoryNoticePayWageIncrease()
                    {
                        WageIncreaseStartDate = new DateTime(2018, 06, 05),
                        WageIncreaseAmount = 220
                    }
                }
            };

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            response.CompensationEndDate.Should().Be(new DateTime(2018, 6, 15));
            response.DaysInClaim.Should().Be(8);
            response.NoticeStartDate.Should().Be(new DateTime(2018, 6, 2));
            response.NoticeWeeksDue.Should().Be(2);
            response.MaxCNPEntitlement.Should().Be(660.5m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2018, 6, 15));
            response.WeeklyResults.Count.Should().Be(2);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(198.15m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(138.15m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(0m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(110.52m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(27.63m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(60m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(response.WeeklyResults[0].NetEntitlement);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(330.25m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(190.25m);
            response.WeeklyResults[1].NIDeducted.Should().Be(2.91m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(149.29m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(38.05m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(140m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(response.WeeklyResults[1].NetEntitlement);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_ForClaimsPostApril2018()
        {
            //{ "cnp":{ "benefits":[],"dateNoticeGiven":"2019-01-01T01:00:00","dateOfBirth":"1969-01-01T01:00:00","deceasedDate":null,"dismissalDate":"2019-01-01T01:00:00","insolvencyDate":"2018-12-03T01:00:00","insolvencyEmploymentStartDate":"2016-01 01T01:00:00","isTaxable":true,"newEmployments":[],"notionalBenefitOverrides":[],"shiftPattern":["1","2","3","4","5"],"wageIncreases":[],"weeklyWage":961.5400},"nwnp":null,"rp1NotRequired":false}

            var request = new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 01, 01),
                InsolvencyDate = new DateTime(2018, 12, 3),
                DismissalDate = new DateTime(2019, 1, 1),
                DateNoticeGiven = new DateTime(2019, 1, 1),
                WeeklyWage = 961.54m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1969, 1, 1),
                DeceasedDate = null
            };

            var response = await _service.PerformCompensatoryNoticePayCalculationAsync(request, _options);

            //{"nwnp":{"selectedInputSource":null,"rp1Results":{"inputSource":null,"statutoryMax":0.0,"weeklyResult":[]},"rp14aResults":{"inputSource":null,"statutoryMax":0.0,"weeklyResult":[]}},"cnp"

            //:{ "noticeWeeksDue":3,"statutoryMax":508.0,"maxCNPEntitlement":1524.0,"noticeStartDate":"2019-01-02T00:00:00","projectedNoticeDate":"2019-01-22T00:00:00","compensationEndDate":"2019-01-22T00:00:00","daysInClaim":15,"weeklyResults":[{"weekNumber":1,"employerEntitlement":961.54,"benefitsDeducted":0.0,"newEmploymentDeducted":0.0,"wageIncreaseDeducted":0.0,"notionalBenefitDeducted"

            //        :0.0,"grossEntitlement":508.00,"isTaxable":true,"notionalTaxDeducted":0.0,"taxDeducted":101.60,"niDeducted":41.04,"netEntitlement":365.36,"preferentialClaim":0.0,"nonPreferentialClaim":365.36,"isSelected":true}

            //    ,{"weekNumber":2,"employerEntitlement":961.54,"benefitsDeducted":0.0,"newEmploymentDeducted":0.0,"wageIncreaseDeducted":0.0,"notionalBenefitDeducted":73.14,"grossEntitlement":508.00,"isTaxable":true,

            //        "notionalTaxDeducted":0.0,"taxDeducted":101.60,"niDeducted":41.04,"netEntitlement":365.36,"preferentialClaim":0.0,"nonPreferentialClaim":365.36,"isSelected":true},{"weekNumber":3,"employerEntitlement"

            //        :961.54,"benefitsDeducted":0.0,"newEmploymentDeducted":0.0,"wageIncreaseDeducted":0.0,"notionalBenefitDeducted":73.14,"grossEntitlement":508.00,"isTaxable":true,"notionalTaxDeducted":0.0,"taxDeducted"

            //        :101.60,"niDeducted":41.04,"netEntitlement":365.36,"preferentialClaim":0.0,"nonPreferentialClaim":365.36,"isSelected":true}]}}

            response.CompensationEndDate.Should().Be(new DateTime(2019, 1, 22));
            response.DaysInClaim.Should().Be(15);
            response.NoticeStartDate.Should().Be(new DateTime(2019, 1, 2));
            response.NoticeWeeksDue.Should().Be(3);
            response.MaxCNPEntitlement.Should().Be(1524m);
            response.StatutoryMax.Should().Be(508m);
            response.ProjectedNoticeDate.Should().Be(new DateTime(2019, 1, 22));
            response.WeeklyResults.Count.Should().Be(3);

            response.WeeklyResults[0].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[0].EmployerEntitlement.Should().Be(961.54m);
            response.WeeklyResults[0].GrossEntitlement.Should().Be(508m);
            response.WeeklyResults[0].IsTaxable.Should().BeTrue();
            response.WeeklyResults[0].NIDeducted.Should().Be(41.04m);
            response.WeeklyResults[0].NetEntitlement.Should().Be(365.36m);
            response.WeeklyResults[0].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalBenefitDeducted.Should().Be(0m);
            response.WeeklyResults[0].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[0].TaxDeducted.Should().Be(101.60m);
            response.WeeklyResults[0].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[0].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[0].NonPreferentialClaim.Should().Be(508m);
            response.WeeklyResults[0].WeekNumber.Should().Be(1);

            response.WeeklyResults[1].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[1].EmployerEntitlement.Should().Be(961.54m);
            response.WeeklyResults[1].GrossEntitlement.Should().Be(508m);
            response.WeeklyResults[1].IsTaxable.Should().BeTrue();
            response.WeeklyResults[1].NIDeducted.Should().Be(41.04m);
            response.WeeklyResults[1].NetEntitlement.Should().Be(365.36m);
            response.WeeklyResults[1].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[1].NotionalBenefitDeducted.Should().Be(73.14m);
            response.WeeklyResults[1].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[1].TaxDeducted.Should().Be(101.60m);
            response.WeeklyResults[1].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[1].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[1].NonPreferentialClaim.Should().Be(508m);
            response.WeeklyResults[1].WeekNumber.Should().Be(2);

            response.WeeklyResults[2].BenefitsDeducted.Should().Be(0m);
            response.WeeklyResults[2].EmployerEntitlement.Should().Be(961.54m);
            response.WeeklyResults[2].GrossEntitlement.Should().Be(508m);
            response.WeeklyResults[2].IsTaxable.Should().BeTrue();
            response.WeeklyResults[2].NIDeducted.Should().Be(41.04m);
            response.WeeklyResults[2].NetEntitlement.Should().Be(365.36m);
            response.WeeklyResults[2].NewEmploymentDeducted.Should().Be(0m);
            response.WeeklyResults[2].NotionalBenefitDeducted.Should().Be(73.14m);
            response.WeeklyResults[2].NotionalTaxDeducted.Should().Be(0m);
            response.WeeklyResults[2].TaxDeducted.Should().Be(101.60m);
            response.WeeklyResults[2].WageIncreaseDeducted.Should().Be(0m);
            response.WeeklyResults[2].PreferentialClaim.Should().Be(0m);
            response.WeeklyResults[2].NonPreferentialClaim.Should().Be(508m);
            response.WeeklyResults[2].WeekNumber.Should().Be(3);
        }
    }
}