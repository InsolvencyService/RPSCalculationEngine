using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class ProtectiveAwardCalculationServiceTests
    {
        private readonly ProtectiveAwardCalculationService _service;
        private readonly IOptions<ConfigLookupRoot> _options;

        public ProtectiveAwardCalculationServiceTests()
        {
            _service = new ProtectiveAwardCalculationService();
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithoutBenefit()
        {
            var request = new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2017, 07, 24),
                InsolvencyDate = new DateTime(2017, 08, 03),
                DismissalDate = new DateTime(2017, 08, 07),
                TribunalAwardDate = new DateTime(2017, 09, 01),
                ProtectiveAwardStartDate = new DateTime(2017, 08, 08),
                ProtectiveAwardDays = 90,
                PayDay = 6,
                WeeklyWage = 348.60m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" }
            };

            var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);

            response.IsTaxable.Should().BeTrue();
            response.PayLines.Count.Should().Be(14);

            response.PayLines[0].BenefitsClaimed.Should().Be(0m);
            response.PayLines[0].GrossEntitlement.Should().Be(278.88m);
            response.PayLines[0].TaxDeducted.Should().Be(55.78m);
            response.PayLines[0].NIDeducted.Should().Be(10.67m);
            response.PayLines[0].NetEntitlement.Should().Be(212.43m);
            response.PayLines[0].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[1].BenefitsClaimed.Should().Be(0m);
            response.PayLines[1].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[1].TaxDeducted.Should().Be(69.72m);
            response.PayLines[1].NIDeducted.Should().Be(19.03m);
            response.PayLines[1].NetEntitlement.Should().Be(259.85m);
            response.PayLines[1].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[2].BenefitsClaimed.Should().Be(0m);
            response.PayLines[2].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[2].TaxDeducted.Should().Be(69.72m);
            response.PayLines[2].NIDeducted.Should().Be(19.03m);
            response.PayLines[2].NetEntitlement.Should().Be(259.85m);
            response.PayLines[2].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[3].BenefitsClaimed.Should().Be(0m);
            response.PayLines[3].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[3].TaxDeducted.Should().Be(69.72m);
            response.PayLines[3].NIDeducted.Should().Be(19.03m);
            response.PayLines[3].NetEntitlement.Should().Be(259.85m);
            response.PayLines[3].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[4].BenefitsClaimed.Should().Be(0m);
            response.PayLines[4].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[4].TaxDeducted.Should().Be(69.72m);
            response.PayLines[4].NIDeducted.Should().Be(19.03m);
            response.PayLines[4].NetEntitlement.Should().Be(259.85m);
            response.PayLines[4].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[5].BenefitsClaimed.Should().Be(0m);
            response.PayLines[5].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[5].TaxDeducted.Should().Be(69.72m);
            response.PayLines[5].NIDeducted.Should().Be(19.03m);
            response.PayLines[5].NetEntitlement.Should().Be(259.85m);
            response.PayLines[5].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[5].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[5].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[6].BenefitsClaimed.Should().Be(0m);
            response.PayLines[6].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[6].TaxDeducted.Should().Be(69.72m);
            response.PayLines[6].NIDeducted.Should().Be(19.03m);
            response.PayLines[6].NetEntitlement.Should().Be(259.85m);
            response.PayLines[6].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[6].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[6].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[7].BenefitsClaimed.Should().Be(0m);
            response.PayLines[7].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[7].TaxDeducted.Should().Be(69.72m);
            response.PayLines[7].NIDeducted.Should().Be(19.03m);
            response.PayLines[7].NetEntitlement.Should().Be(259.85m);
            response.PayLines[7].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[7].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[7].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[8].BenefitsClaimed.Should().Be(0m);
            response.PayLines[8].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[8].TaxDeducted.Should().Be(69.72m);
            response.PayLines[8].NIDeducted.Should().Be(19.03m);
            response.PayLines[8].NetEntitlement.Should().Be(259.85m);
            response.PayLines[8].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[8].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[8].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[9].BenefitsClaimed.Should().Be(0m);
            response.PayLines[9].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[9].TaxDeducted.Should().Be(69.72m);
            response.PayLines[9].NIDeducted.Should().Be(19.03m);
            response.PayLines[9].NetEntitlement.Should().Be(259.85m);
            response.PayLines[9].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[9].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[9].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[10].BenefitsClaimed.Should().Be(0m);
            response.PayLines[10].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[10].TaxDeducted.Should().Be(69.72m);
            response.PayLines[10].NIDeducted.Should().Be(19.03m);
            response.PayLines[10].NetEntitlement.Should().Be(259.85m);
            response.PayLines[10].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[10].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[10].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[11].BenefitsClaimed.Should().Be(0m);
            response.PayLines[11].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[11].TaxDeducted.Should().Be(69.72m);
            response.PayLines[11].NIDeducted.Should().Be(19.03m);
            response.PayLines[11].NetEntitlement.Should().Be(259.85m);
            response.PayLines[11].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[11].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[11].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[12].BenefitsClaimed.Should().Be(0m);
            response.PayLines[12].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[12].TaxDeducted.Should().Be(69.72m);
            response.PayLines[12].NIDeducted.Should().Be(19.03m);
            response.PayLines[12].NetEntitlement.Should().Be(259.85m);
            response.PayLines[12].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[12].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[12].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[13].BenefitsClaimed.Should().Be(0m);
            response.PayLines[13].GrossEntitlement.Should().Be(0m);
            response.PayLines[13].TaxDeducted.Should().Be(0m);
            response.PayLines[13].NIDeducted.Should().Be(0m);
            response.PayLines[13].NetEntitlement.Should().Be(0m);
            response.PayLines[13].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[13].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[13].GrossEntitlementIn4Months.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithSingleBenefit()
        {
            var request = new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2017, 07, 24),
                InsolvencyDate = new DateTime(2017, 08, 03),
                DismissalDate = new DateTime(2017, 08, 07),
                TribunalAwardDate = new DateTime(2017, 09, 01),
                ProtectiveAwardStartDate = new DateTime(2017, 08, 08),
                ProtectiveAwardDays = 90,
                PayDay = 6,
                WeeklyWage = 348.60m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                paBenefitStartDate = new DateTime(2017, 08, 13),
                //BenefitEndDate = new DateTime(2017, 10, 07),
                paBenefitAmount = 120m

            };

            var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);

            response.IsTaxable.Should().BeTrue();
            response.PayLines.Count.Should().Be(14);

            response.PayLines[0].BenefitsClaimed.Should().Be(0m);
            response.PayLines[0].GrossEntitlement.Should().Be(278.88m);
            response.PayLines[0].TaxDeducted.Should().Be(55.78m);
            response.PayLines[0].NIDeducted.Should().Be(10.67m);
            response.PayLines[0].NetEntitlement.Should().Be(212.43m);
            response.PayLines[0].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[1].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[1].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[1].TaxDeducted.Should().Be(67.74m);
            response.PayLines[1].NIDeducted.Should().Be(17.85m);
            response.PayLines[1].NetEntitlement.Should().Be(253.13m);
            response.PayLines[1].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[2].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[2].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[2].TaxDeducted.Should().Be(67.74m);
            response.PayLines[2].NIDeducted.Should().Be(17.85m);
            response.PayLines[2].NetEntitlement.Should().Be(253.13m);
            response.PayLines[2].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[3].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[3].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[3].TaxDeducted.Should().Be(67.74m);
            response.PayLines[3].NIDeducted.Should().Be(17.85m);
            response.PayLines[3].NetEntitlement.Should().Be(253.13m);
            response.PayLines[3].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[4].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[4].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[4].TaxDeducted.Should().Be(67.74m);
            response.PayLines[4].NIDeducted.Should().Be(17.85m);
            response.PayLines[4].NetEntitlement.Should().Be(253.13m);
            response.PayLines[4].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[5].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[5].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[5].TaxDeducted.Should().Be(67.74m);
            response.PayLines[5].NIDeducted.Should().Be(17.85m);
            response.PayLines[5].NetEntitlement.Should().Be(253.13m);
            response.PayLines[5].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[5].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[5].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[6].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[6].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[6].TaxDeducted.Should().Be(67.74m);
            response.PayLines[6].NIDeducted.Should().Be(17.85m);
            response.PayLines[6].NetEntitlement.Should().Be(253.13m);
            response.PayLines[6].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[6].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[6].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[7].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[7].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[7].TaxDeducted.Should().Be(67.74m);
            response.PayLines[7].NIDeducted.Should().Be(17.85m);
            response.PayLines[7].NetEntitlement.Should().Be(253.13m);
            response.PayLines[7].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[7].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[7].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[8].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[8].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[8].TaxDeducted.Should().Be(67.74m);
            response.PayLines[8].NIDeducted.Should().Be(17.85m);
            response.PayLines[8].NetEntitlement.Should().Be(253.13m);
            response.PayLines[8].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[8].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[8].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[9].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[9].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[9].TaxDeducted.Should().Be(67.74m);
            response.PayLines[9].NIDeducted.Should().Be(17.85m);
            response.PayLines[9].NetEntitlement.Should().Be(253.13m);
            response.PayLines[9].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[9].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[9].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[10].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[10].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[10].TaxDeducted.Should().Be(67.74m);
            response.PayLines[10].NIDeducted.Should().Be(17.85m);
            response.PayLines[10].NetEntitlement.Should().Be(253.13m);
            response.PayLines[10].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[10].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[10].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[11].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[11].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[11].TaxDeducted.Should().Be(67.74m);
            response.PayLines[11].NIDeducted.Should().Be(17.85m);
            response.PayLines[11].NetEntitlement.Should().Be(253.13m);
            response.PayLines[11].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[11].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[11].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[12].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[12].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[12].TaxDeducted.Should().Be(67.74m);
            response.PayLines[12].NIDeducted.Should().Be(17.85m);
            response.PayLines[12].NetEntitlement.Should().Be(253.13m);
            response.PayLines[12].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[12].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[12].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[13].BenefitsClaimed.Should().Be(1.41m);
            response.PayLines[13].GrossEntitlement.Should().Be(0m);
            response.PayLines[13].TaxDeducted.Should().Be(0m);
            response.PayLines[13].NIDeducted.Should().Be(0m);
            response.PayLines[13].NetEntitlement.Should().Be(0m);
            response.PayLines[13].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[13].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[13].GrossEntitlementIn4Months.Should().Be(0m);
        }

        //Commented out due to passing of multiple benefits not supported anymore
        //[Fact]
        //[Trait("Category", "UnitTest")]
        //public async Task PerformCalculationAsync_WithMultipleBenefits()
        //{
        //    var request = new ProtectiveAwardCalculationRequestModel
        //    {
        //        EmploymentStartDate = new DateTime(2017, 07, 24),
        //        InsolvencyDate = new DateTime(2017, 08, 03),
        //        DismissalDate = new DateTime(2017, 08, 07),
        //        TribunalAwardDate = new DateTime(2017, 09, 01),
        //        ProtectiveAwardStartDate = new DateTime(2017, 08, 08),
        //        ProtectiveAwardDays = 27,
        //        PayDay = 6,
        //        WeeklyWage = 348.60m,
        //        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
        //        Benefits = new List<ProtectiveAwardBenefit>(){
        //            new ProtectiveAwardBenefit()
        //            {
        //                BenefitStartDate = new DateTime(2017, 08, 13),
        //                BenefitEndDate = new DateTime(2017, 08, 26),
        //                BenefitAmount = 140m
        //            },
        //            new ProtectiveAwardBenefit()
        //            {
        //                BenefitStartDate = new DateTime(2017, 08, 20),
        //                BenefitEndDate = new DateTime(2017, 09, 02),
        //                BenefitAmount = 400m
        //            }
        //        }
        //    };

        //    var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);

        //    response.IsTaxable.Should().BeTrue();
        //    response.PayLines.Count.Should().Be(5);

        //    response.PayLines[0].BenefitsClaimed.Should().Be(0m);
        //    response.PayLines[0].GrossEntitlement.Should().Be(278.88m);
        //    response.PayLines[0].TaxDeducted.Should().Be(55.78m);
        //    response.PayLines[0].NIDeducted.Should().Be(13.55m);
        //    response.PayLines[0].NetEntitlement.Should().Be(209.55m);
        //    response.PayLines[0].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[0].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[0].GrossEntitlementIn4Months.Should().Be(0m);

        //    response.PayLines[1].BenefitsClaimed.Should().Be(70m);
        //    response.PayLines[1].GrossEntitlement.Should().Be(278.60m);
        //    response.PayLines[1].TaxDeducted.Should().Be(55.72m);
        //    response.PayLines[1].NIDeducted.Should().Be(13.51m);
        //    response.PayLines[1].NetEntitlement.Should().Be(209.37m);
        //    response.PayLines[1].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[1].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[1].GrossEntitlementIn4Months.Should().Be(0m);

        //    response.PayLines[2].BenefitsClaimed.Should().Be(270m);
        //    response.PayLines[2].GrossEntitlement.Should().Be(78.60m);
        //    response.PayLines[2].TaxDeducted.Should().Be(15.72m);
        //    response.PayLines[2].NIDeducted.Should().Be(0m);
        //    response.PayLines[2].NetEntitlement.Should().Be(62.88m);
        //    response.PayLines[2].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[2].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[2].GrossEntitlementIn4Months.Should().Be(0m);

        //    response.PayLines[3].BenefitsClaimed.Should().Be(200m);
        //    response.PayLines[3].GrossEntitlement.Should().Be(148.60m);
        //    response.PayLines[3].TaxDeducted.Should().Be(29.72m);
        //    response.PayLines[3].NIDeducted.Should().Be(0m);
        //    response.PayLines[3].NetEntitlement.Should().Be(118.88m);
        //    response.PayLines[3].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[3].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[3].GrossEntitlementIn4Months.Should().Be(0m);

        //    response.PayLines[4].BenefitsClaimed.Should().Be(0m);
        //    response.PayLines[4].GrossEntitlement.Should().Be(0m);
        //    response.PayLines[4].TaxDeducted.Should().Be(0m);
        //    response.PayLines[4].NIDeducted.Should().Be(0m);
        //    response.PayLines[4].NetEntitlement.Should().Be(0m);
        //    response.PayLines[4].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[4].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
        //    response.PayLines[4].GrossEntitlementIn4Months.Should().Be(0m);
        //}

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithBenefitHigherThanWeeklyWage()
        {
            var request = new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2017, 07, 24),
                InsolvencyDate = new DateTime(2017, 08, 03),
                DismissalDate = new DateTime(2017, 08, 07),
                TribunalAwardDate = new DateTime(2017, 09, 01),
                ProtectiveAwardStartDate = new DateTime(2017, 08, 08),
                ProtectiveAwardDays = 27,
                PayDay = 6,
                WeeklyWage = 348.60m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                paBenefitStartDate = new DateTime(2017, 08, 13),
                paBenefitAmount = 500m
                
            };

            var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);

            response.IsTaxable.Should().BeTrue();
            response.PayLines.Count.Should().Be(5);

            response.PayLines[0].BenefitsClaimed.Should().Be(0m);
            response.PayLines[0].GrossEntitlement.Should().Be(278.88m);
            response.PayLines[0].TaxDeducted.Should().Be(55.78m);
            response.PayLines[0].NIDeducted.Should().Be(10.67m);
            response.PayLines[0].NetEntitlement.Should().Be(212.43m);
            response.PayLines[0].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[1].BenefitsClaimed.Should().Be(159.09m);
            response.PayLines[1].GrossEntitlement.Should().Be(189.51m);
            response.PayLines[1].TaxDeducted.Should().Be(37.90m);
            response.PayLines[1].NIDeducted.Should().Be(0.0m);
            response.PayLines[1].NetEntitlement.Should().Be(151.61m);
            response.PayLines[1].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[2].BenefitsClaimed.Should().Be(159.09m);
            response.PayLines[2].GrossEntitlement.Should().Be(189.51m);
            response.PayLines[2].TaxDeducted.Should().Be(37.90m);
            response.PayLines[2].NIDeducted.Should().Be(0.0m);
            response.PayLines[2].NetEntitlement.Should().Be(151.61m);
            response.PayLines[2].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[3].BenefitsClaimed.Should().Be(159.09m);
            response.PayLines[3].GrossEntitlement.Should().Be(189.51m);
            response.PayLines[3].TaxDeducted.Should().Be(37.90m);
            response.PayLines[3].NIDeducted.Should().Be(0.0m);
            response.PayLines[3].NetEntitlement.Should().Be(151.61m);
            response.PayLines[3].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[4].BenefitsClaimed.Should().Be(22.73m);
            response.PayLines[4].GrossEntitlement.Should().Be(0m);
            response.PayLines[4].TaxDeducted.Should().Be(0m);
            response.PayLines[4].NIDeducted.Should().Be(0m);
            response.PayLines[4].NetEntitlement.Should().Be(0m);
            response.PayLines[4].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].GrossEntitlementIn4Months.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithTribunalDateInADifferentYear()
        {
            var request = new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2017, 07, 24),
                InsolvencyDate = new DateTime(2017, 08, 03),
                DismissalDate = new DateTime(2017, 08, 07),
                TribunalAwardDate = new DateTime(2018, 04, 06),
                ProtectiveAwardStartDate = new DateTime(2017, 08, 08),
                ProtectiveAwardDays = 90,
                PayDay = 6,
                WeeklyWage = 348.60m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                paBenefitStartDate = new DateTime(2017, 08, 13),
                paBenefitAmount = 120m
                
            };

            var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);

            response.IsTaxable.Should().BeFalse();
            response.PayLines.Count.Should().Be(14);

            response.PayLines[0].BenefitsClaimed.Should().Be(0m);
            response.PayLines[0].GrossEntitlement.Should().Be(278.88m);
            response.PayLines[0].TaxDeducted.Should().Be(0);
            response.PayLines[0].NIDeducted.Should().Be(10.67m);
            response.PayLines[0].NetEntitlement.Should().Be(268.21m);
            response.PayLines[0].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[1].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[1].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[1].TaxDeducted.Should().Be(0);
            response.PayLines[1].NIDeducted.Should().Be(17.85m);
            response.PayLines[1].NetEntitlement.Should().Be(320.87m);
            response.PayLines[1].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[2].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[2].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[2].TaxDeducted.Should().Be(0);
            response.PayLines[1].NIDeducted.Should().Be(17.85m);
            response.PayLines[1].NetEntitlement.Should().Be(320.87m);
            response.PayLines[2].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[3].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[3].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[3].TaxDeducted.Should().Be(0);
            response.PayLines[1].NIDeducted.Should().Be(17.85m);
            response.PayLines[1].NetEntitlement.Should().Be(320.87m);
            response.PayLines[3].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[4].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[4].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[4].TaxDeducted.Should().Be(0);
            response.PayLines[4].NIDeducted.Should().Be(17.85m);
            response.PayLines[4].NetEntitlement.Should().Be(320.87m);
            response.PayLines[4].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[4].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[5].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[5].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[5].TaxDeducted.Should().Be(0);
            response.PayLines[5].NIDeducted.Should().Be(17.85m);
            response.PayLines[5].NetEntitlement.Should().Be(320.87m);
            response.PayLines[5].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[5].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[5].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[6].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[6].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[6].TaxDeducted.Should().Be(0);
            response.PayLines[1].NIDeducted.Should().Be(17.85m);
            response.PayLines[1].NetEntitlement.Should().Be(320.87m);
            response.PayLines[6].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[6].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[6].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[7].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[7].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[7].TaxDeducted.Should().Be(0);
            response.PayLines[7].NIDeducted.Should().Be(17.85m);
            response.PayLines[7].NetEntitlement.Should().Be(320.87m);
            response.PayLines[7].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[7].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[7].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[8].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[8].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[8].TaxDeducted.Should().Be(0);
            response.PayLines[8].NIDeducted.Should().Be(17.85m);
            response.PayLines[8].NetEntitlement.Should().Be(320.87m);
            response.PayLines[8].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[8].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[8].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[9].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[9].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[9].TaxDeducted.Should().Be(0);
            response.PayLines[9].NIDeducted.Should().Be(17.85m);
            response.PayLines[9].NetEntitlement.Should().Be(320.87m);
            response.PayLines[9].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[9].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[9].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[10].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[10].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[10].TaxDeducted.Should().Be(0);
            response.PayLines[10].NIDeducted.Should().Be(17.85m);
            response.PayLines[10].NetEntitlement.Should().Be(320.87m);
            response.PayLines[10].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[10].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[10].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[11].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[11].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[11].TaxDeducted.Should().Be(0);
            response.PayLines[11].NIDeducted.Should().Be(17.85m);
            response.PayLines[11].NetEntitlement.Should().Be(320.87m);
            response.PayLines[11].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[11].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[11].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[12].BenefitsClaimed.Should().Be(9.88m);
            response.PayLines[12].GrossEntitlement.Should().Be(338.72m);
            response.PayLines[12].TaxDeducted.Should().Be(0);
            response.PayLines[12].NIDeducted.Should().Be(17.85m);
            response.PayLines[12].NetEntitlement.Should().Be(320.87m);
            response.PayLines[12].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[12].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[12].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[13].BenefitsClaimed.Should().Be(1.41m);
            response.PayLines[13].GrossEntitlement.Should().Be(0m);
            response.PayLines[13].TaxDeducted.Should().Be(0);
            response.PayLines[13].NIDeducted.Should().Be(0m);
            response.PayLines[13].NetEntitlement.Should().Be(0m);
            response.PayLines[13].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[13].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[13].GrossEntitlementIn4Months.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithClaimIn4MonthsPriorToInsolvencyDate()
        {
            var request = new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2017, 07, 24),
                InsolvencyDate = new DateTime(2017, 08, 03),
                DismissalDate = new DateTime(2017, 08, 07),
                TribunalAwardDate = new DateTime(2017, 04, 06),
                ProtectiveAwardStartDate = new DateTime(2017, 06, 01),
                ProtectiveAwardDays = 20,
                PayDay = 6,
                WeeklyWage = 348.60m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                paBenefitStartDate = new DateTime(2017, 08, 13),               
                paBenefitAmount = 120m
                
            };

            var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);

            response.IsTaxable.Should().BeTrue();
            response.PayLines.Count.Should().Be(4);

            response.PayLines[0].BenefitsClaimed.Should().Be(0m);
            response.PayLines[0].GrossEntitlement.Should().Be(139.44m);
            response.PayLines[0].TaxDeducted.Should().Be(27.89m);
            response.PayLines[0].NIDeducted.Should().Be(0);
            response.PayLines[0].NetEntitlement.Should().Be(111.55m);
            response.PayLines[0].MaximumEntitlementIn4MonthPeriod.Should().Be(209.57m);
            response.PayLines[0].EmployerEntitlementIn4MonthPeriod.Should().Be(139.44m);
            response.PayLines[0].GrossEntitlementIn4Months.Should().Be(139.44m);

            response.PayLines[1].BenefitsClaimed.Should().Be(0m);
            response.PayLines[1].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[1].TaxDeducted.Should().Be(69.72m);
            response.PayLines[1].NIDeducted.Should().Be(19.03m);
            response.PayLines[1].NetEntitlement.Should().Be(259.85m);
            response.PayLines[1].MaximumEntitlementIn4MonthPeriod.Should().Be(489m);
            response.PayLines[1].EmployerEntitlementIn4MonthPeriod.Should().Be(348.60m);
            response.PayLines[1].GrossEntitlementIn4Months.Should().Be(348.60m);

            response.PayLines[2].BenefitsClaimed.Should().Be(0m);
            response.PayLines[2].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[2].TaxDeducted.Should().Be(69.72m);
            response.PayLines[1].NIDeducted.Should().Be(19.03m);
            response.PayLines[1].NetEntitlement.Should().Be(259.85m);
            response.PayLines[2].MaximumEntitlementIn4MonthPeriod.Should().Be(489m);
            response.PayLines[2].EmployerEntitlementIn4MonthPeriod.Should().Be(348.60m);
            response.PayLines[2].GrossEntitlementIn4Months.Should().Be(348.60m);

            response.PayLines[3].BenefitsClaimed.Should().Be(0m);
            response.PayLines[3].GrossEntitlement.Should().Be(139.44m);
            response.PayLines[3].TaxDeducted.Should().Be(27.89m);
            response.PayLines[3].NIDeducted.Should().Be(0m);
            response.PayLines[3].NetEntitlement.Should().Be(111.55m);
            response.PayLines[3].MaximumEntitlementIn4MonthPeriod.Should().Be(209.57m);
            response.PayLines[3].EmployerEntitlementIn4MonthPeriod.Should().Be(139.44m);
            response.PayLines[3].GrossEntitlementIn4Months.Should().Be(139.44m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_WithClaimBefore4MonthsPriorToInsolvencyDate()
        {
            var request = new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2017, 07, 24),
                InsolvencyDate = new DateTime(2017, 08, 03),
                DismissalDate = new DateTime(2017, 08, 07),
                TribunalAwardDate = new DateTime(2017, 04, 06),
                ProtectiveAwardStartDate = new DateTime(2017, 03, 01),
                ProtectiveAwardDays = 21,
                PayDay = 6,
                WeeklyWage = 348.60m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                paBenefitStartDate = new DateTime(2017, 08, 13),
                paBenefitAmount = 120m
                
            };

            var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);

            response.IsTaxable.Should().BeTrue();
            response.PayLines.Count.Should().Be(4);

            response.PayLines[0].BenefitsClaimed.Should().Be(0m);
            response.PayLines[0].GrossEntitlement.Should().Be(209.16m);
            response.PayLines[0].TaxDeducted.Should().Be(41.83m);
            response.PayLines[0].NIDeducted.Should().Be(2.30m);
            response.PayLines[0].NetEntitlement.Should().Be(165.03m);
            response.PayLines[0].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[0].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[1].BenefitsClaimed.Should().Be(0m);
            response.PayLines[1].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[1].TaxDeducted.Should().Be(69.72m);
            response.PayLines[1].NIDeducted.Should().Be(19.03m);
            response.PayLines[1].NetEntitlement.Should().Be(259.85m);
            response.PayLines[1].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[1].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[2].BenefitsClaimed.Should().Be(0m);
            response.PayLines[2].GrossEntitlement.Should().Be(348.60m);
            response.PayLines[2].TaxDeducted.Should().Be(69.72m);
            response.PayLines[1].NIDeducted.Should().Be(19.03m);
            response.PayLines[1].NetEntitlement.Should().Be(259.85m);
            response.PayLines[2].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[2].GrossEntitlementIn4Months.Should().Be(0m);

            response.PayLines[3].BenefitsClaimed.Should().Be(0m);
            response.PayLines[3].GrossEntitlement.Should().Be(139.44m);
            response.PayLines[3].TaxDeducted.Should().Be(27.89m);
            response.PayLines[3].NIDeducted.Should().Be(0m);
            response.PayLines[3].NetEntitlement.Should().Be(111.55m);
            response.PayLines[3].MaximumEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].EmployerEntitlementIn4MonthPeriod.Should().Be(0m);
            response.PayLines[3].GrossEntitlementIn4Months.Should().Be(0m);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_EnsureStatMaxUsesTheLatestDate()
        {
            var request = new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 07, 24),
                InsolvencyDate = new DateTime(2017, 08, 07),
                DismissalDate = new DateTime(2017, 08, 07),
                TribunalAwardDate = new DateTime(2017, 08, 07),
                ProtectiveAwardStartDate = new DateTime(2018, 04, 8),
                ProtectiveAwardDays = 21,
                PayDay = 6,
                WeeklyWage = 9000m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
            };

            var response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);
            response.PayLines[0].GrossEntitlement.Should().Be(489m);

            request.InsolvencyDate = new DateTime(2017, 08, 07);
            request.DismissalDate = new DateTime(2018, 4, 6);
            request.TribunalAwardDate = new DateTime(2017, 08, 07);
            response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);
            response.PayLines[0].GrossEntitlement.Should().Be(508m);

            request.InsolvencyDate = new DateTime(2018, 04, 6);
            request.DismissalDate = new DateTime(2017, 8, 7);
            request.TribunalAwardDate = new DateTime(2017, 08, 07);
            response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);
            response.PayLines[0].GrossEntitlement.Should().Be(508m);

            request.InsolvencyDate = new DateTime(2017, 08, 07);
            request.DismissalDate = new DateTime(2017, 8, 7);
            request.TribunalAwardDate = new DateTime(2018, 4, 6);
            response = await _service.PerformProtectiveAwardCalculationAsync(request, _options);
            response.PayLines[0].GrossEntitlement.Should().Be(508m);
        }
    }
}



