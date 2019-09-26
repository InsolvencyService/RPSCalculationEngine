using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class APPACalculationsServiceTests
    {
        private readonly APPACalculationService _service;
        private readonly Mock<IArrearsOfPayCalculationsService> _apService;
        private readonly Mock<IProtectiveAwardCalculationService> _paService;
        private readonly IOptions<ConfigLookupRoot> _options;

        public APPACalculationsServiceTests()
        {
            _apService = new Mock<IArrearsOfPayCalculationsService>();
            _paService = new Mock<IProtectiveAwardCalculationService>();
            _service = new APPACalculationService(_apService.Object, _paService.Object);

            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_PerformsAPAndPACalculationsWithRp1Selected()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var apRequests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 6),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                    UnpaidPeriodTo = new DateTime(2018, 10, 9),
                    ApClaimAmount = 350M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 200m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp14a,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                }
            };

            var apResponseRP1 = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp1,
                DngApplied = true,
                RunNWNP = false,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),250m, 508M, 350, 350M,true,50M, 30M, 300M,7, 5, 508M, 300M, 300M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),250m, 508M, 20M, 20M,true,4M, 0M, 16M, 7, 2, 508M, 20M, 20M),

                }
            };

            var apResponseRP14a = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp14a,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),428.57M, 508M, 257.14M, 357.14M,true,51.43M, 11.42M, 294.29M, 7, 3, 508M, 357.14M, 357.14M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 28.29M, 7, 4, 508M, 22.86M, 22.86M),
                }
            };

            var paRequest = new ProtectiveAwardCalculationRequestModel()
            {
                InsolvencyDate = new DateTime(2018, 10, 20),
                EmploymentStartDate = new DateTime(2016, 4, 6),
                DismissalDate = new DateTime(2018, 10, 20),
                TribunalAwardDate = new DateTime(2018, 10, 21),
                ProtectiveAwardStartDate = new DateTime(2018, 10, 22),
                ProtectiveAwardDays = 90,
                PayDay = 6,
                WeeklyWage = 400M,
                ShiftPattern = shiftPattern,
            };

            var paResponse = new ProtectiveAwardResponseDTO()
            {
                IsTaxable = true,
                StatutoryMax = 508m,
                PayLines = new List<ProtectiveAwardPayLine>()
                {
                    new ProtectiveAwardPayLine(1, new DateTime(2018, 10, 27), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(2, new DateTime(2018, 11, 3),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(3, new DateTime(2018, 11, 10), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(4, new DateTime(2018, 11, 17), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(5, new DateTime(2018, 11, 24), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(6, new DateTime(2018, 12, 1),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(7, new DateTime(2018, 12, 8),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(8, new DateTime(2018, 12, 15), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(9, new DateTime(2018, 12, 22), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(10, new DateTime(2018, 12, 29), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(11, new DateTime(2019, 1, 5),   0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(12, new DateTime(2019, 1, 12),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(13, new DateTime(2019, 1, 12),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M)
                }
            };

            var request = new APPACalculationRequestModel()
            {
                Ap = apRequests,
                Pa = paRequest
            };

            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp1, _options)).ReturnsAsync(apResponseRP1);
            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp14a, _options)).ReturnsAsync(apResponseRP14a);
            _paService.Setup(m => m.PerformProtectiveAwardCalculationAsync(paRequest, _options)).ReturnsAsync(paResponse);

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().NotBeNull();
            results.Pa.Should().NotBeNull();

            results.Ap.SelectedInputSource.Should().Be(InputSource.Rp1);
            results.Ap.RP1ResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(1);
            results.Ap.RP14aResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(0);
            results.Pa.PayLines.Count(x => x.IsSelected).Should().Be(7);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_PerformsAPAndPACalculationsWithRp14aSelected()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var apRequests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 6),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                    UnpaidPeriodTo = new DateTime(2018, 10, 9),
                    ApClaimAmount = 700M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp14a,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                }
            };

            var apResponseRP1 = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp1,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),500m, 508M, 500M, 500M,true,100M, 40.56M, 359.44M,7, 5, 508M, 500M, 500M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 40M, 40M,true,8M, 0M, 32M, 7, 2, 508M, 40M, 40M),
                }
            };

            var apResponseRP14a = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp14a,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),428.57M, 508M, 357.14M, 357.14M,true,51.43M, 11.42M, 294.29M, 7, 3, 508M, 357.14M, 357.14M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 18.29M, 7, 4, 508M, 22.86M, 22.86M),
                }
            };

            var paRequest = new ProtectiveAwardCalculationRequestModel()
            {
                InsolvencyDate = new DateTime(2018, 10, 20),
                EmploymentStartDate = new DateTime(2016, 4, 6),
                DismissalDate = new DateTime(2018, 10, 20),
                TribunalAwardDate = new DateTime(2018, 10, 21),
                ProtectiveAwardStartDate = new DateTime(2018, 10, 22),
                ProtectiveAwardDays = 90,
                PayDay = 6,
                WeeklyWage = 400M,
                ShiftPattern = shiftPattern,
            };

            var paResponse = new ProtectiveAwardResponseDTO()
            {
                IsTaxable = true,
                StatutoryMax = 508m,
                PayLines = new List<ProtectiveAwardPayLine>()
                {
                    new ProtectiveAwardPayLine(1, new DateTime(2018, 10, 27), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(2, new DateTime(2018, 11, 3),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(3, new DateTime(2018, 11, 10), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(4, new DateTime(2018, 11, 17), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(5, new DateTime(2018, 11, 24), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(6, new DateTime(2018, 12, 1),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(7, new DateTime(2018, 12, 8),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(8, new DateTime(2018, 12, 15), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(9, new DateTime(2018, 12, 22), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(10, new DateTime(2018, 12, 29), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(11, new DateTime(2019, 1, 5),   0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(12, new DateTime(2019, 1, 12),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(13, new DateTime(2019, 1, 12),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M)
                }
            };

            var request = new APPACalculationRequestModel()
            {
                Ap = apRequests,
                Pa = paRequest
            };

            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp1, _options)).ReturnsAsync(apResponseRP1);
            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp14a, _options)).ReturnsAsync(apResponseRP14a);
            _paService.Setup(m => m.PerformProtectiveAwardCalculationAsync(paRequest, _options)).ReturnsAsync(paResponse);

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().NotBeNull();
            results.Pa.Should().NotBeNull();

            results.Ap.SelectedInputSource.Should().Be(InputSource.Rp14a);
            results.Ap.RP1ResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(0);
            results.Ap.RP14aResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(1);
            results.Pa.PayLines.Count(x => x.IsSelected).Should().Be(7);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_PerformsAPAndPACalculationsAndPrioritizesPrefPeriodWeeks()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var apRequests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 6),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                    UnpaidPeriodTo = new DateTime(2018, 10, 9),
                    ApClaimAmount = 700M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp14a,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                }
            };

            var apResponseRP1 = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp1,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),500m, 508M, 500M, 500M,true,100M, 40.56M, 359.44M,7, 5, 508M, 500M, 500M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 40M, 40M,true,8M, 0M, 32M, 7, 2, 508M, 40M, 40M),

                }
            };

            var apResponseRP14a = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp14a,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),428.57M, 508M, 257.14M, 257.14M,true,51.43M, 11.42M, 194.29M, 7, 3, 508M, 257.14M, 257.14M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 18.29M, 7, 4, 508M, 22.86M, 22.86M),
                }
            };

            var paRequest = new ProtectiveAwardCalculationRequestModel()
            {
                InsolvencyDate = new DateTime(2018, 10, 20),
                EmploymentStartDate = new DateTime(2016, 4, 6),
                DismissalDate = new DateTime(2018, 10, 20),
                TribunalAwardDate = new DateTime(2018, 10, 21),
                ProtectiveAwardStartDate = new DateTime(2018, 10, 1),
                ProtectiveAwardDays = 90,
                PayDay = 6,
                WeeklyWage = 400M,
                ShiftPattern = shiftPattern,
                paBenefitStartDate = new DateTime(2018, 10, 1),               
                paBenefitAmount = 200M
            };

            var paResponse = new ProtectiveAwardResponseDTO()
            {
                IsTaxable = true,
                StatutoryMax = 508m,
                PayLines = new List<ProtectiveAwardPayLine>()
                {
                    new ProtectiveAwardPayLine(1, new DateTime(2018, 10, 6), 200M, 200M, 40M, 0M, 180M, 508M, 200M, 200M),
                    new ProtectiveAwardPayLine(2, new DateTime(2018, 10, 13), 0M, 400M, 80M, 28.56M, 291.44M, 508M, 291.44M, 291.44M),
                    new ProtectiveAwardPayLine(3, new DateTime(2018, 10, 20), 0M, 400M, 80M, 28.56M, 291.44M, 508M, 291.44M, 291.44M),
                    new ProtectiveAwardPayLine(4, new DateTime(2018, 10, 27), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(5, new DateTime(2018, 11, 3),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(6, new DateTime(2018, 11, 10), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(7, new DateTime(2018, 11, 17), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(8, new DateTime(2018, 11, 24), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(9, new DateTime(2018, 12, 1),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(10, new DateTime(2018, 12, 8),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(11, new DateTime(2018, 12, 15), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(12, new DateTime(2018, 12, 22), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(13, new DateTime(2018, 12, 29), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                }
            };

            var request = new APPACalculationRequestModel()
            {
                Ap = apRequests,
                Pa = paRequest
            };

            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp1, _options)).ReturnsAsync(apResponseRP1);
            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp14a, _options)).ReturnsAsync(apResponseRP14a);
            _paService.Setup(m => m.PerformProtectiveAwardCalculationAsync(paRequest, _options)).ReturnsAsync(paResponse);

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Pa.PayLines.Count(x => x.IsSelected).Should().Be(8);
            results.Pa.PayLines[1].IsSelected.Should().BeTrue();
            results.Pa.PayLines[2].IsSelected.Should().BeTrue();
            results.Pa.PayLines[3].IsSelected.Should().BeTrue();
            results.Pa.PayLines[4].IsSelected.Should().BeTrue();
            results.Pa.PayLines[5].IsSelected.Should().BeTrue();
            results.Pa.PayLines[6].IsSelected.Should().BeTrue();
            results.Pa.PayLines[7].IsSelected.Should().BeTrue();
            results.Pa.PayLines[8].IsSelected.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_PerformsAPAndPACalculationsWithOnlyAPInput()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var apRequests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 6),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 1),
                    UnpaidPeriodTo = new DateTime(2018, 10, 9),
                    ApClaimAmount = 700M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp14a,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                }
            };

            var apResponseRP1 = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp1,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6),500m, 508M, 500M, 500M,true,100M, 40.56M, 359.44M,7, 5, 508M, 500M, 500M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13),500m, 508M, 40M, 40M,true,8M, 0M, 32M, 7, 2, 508M, 40M, 40M),
                }
            };

            var apResponseRP14a = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp14a,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),428.57M, 508M, 257.14M, 257.14M,true,51.43M, 11.42M, 194.29M, 7, 3, 508M, 257.14M, 257.14M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 18.29M, 7, 4, 508M, 22.86M, 22.86M),
                }
            };

            var request = new APPACalculationRequestModel()
            {
                Ap = apRequests,
                Pa = null
            };

            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp1, _options)).ReturnsAsync(apResponseRP1);
            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp14a, _options)).ReturnsAsync(apResponseRP14a);

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().NotBeNull();
            results.Pa.Should().BeNull();

            results.Ap.SelectedInputSource.Should().Be(InputSource.Rp14a);
            results.Ap.RP1ResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(0);
            results.Ap.RP14aResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_PerformsAPAndPACalculationsWithOnlyPAInput()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var paRequest = new ProtectiveAwardCalculationRequestModel()
            {
                InsolvencyDate = new DateTime(2018, 10, 20),
                EmploymentStartDate = new DateTime(2016, 4, 6),
                DismissalDate = new DateTime(2018, 10, 20),
                TribunalAwardDate = new DateTime(2018, 10, 21),
                ProtectiveAwardStartDate = new DateTime(2018, 10, 22),
                ProtectiveAwardDays = 90,
                PayDay = 6,
                WeeklyWage = 400M,
                ShiftPattern = shiftPattern,
            };

            var paResponse = new ProtectiveAwardResponseDTO()
            {
                IsTaxable = true,
                StatutoryMax = 508m,
                PayLines = new List<ProtectiveAwardPayLine>()
                {
                    new ProtectiveAwardPayLine(1, new DateTime(2018, 10, 27), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(2, new DateTime(2018, 11, 3),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(3, new DateTime(2018, 11, 10), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(4, new DateTime(2018, 11, 17), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(5, new DateTime(2018, 11, 24), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(6, new DateTime(2018, 12, 1),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(7, new DateTime(2018, 12, 8),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(8, new DateTime(2018, 12, 15), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(9, new DateTime(2018, 12, 22), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(10, new DateTime(2018, 12, 29), 0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(11, new DateTime(2019, 1, 5),   0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(12, new DateTime(2019, 1, 12),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M),
                    new ProtectiveAwardPayLine(13, new DateTime(2019, 1, 12),  0M, 400M, 80M, 28.56M, 291.44M, 0M, 0M, 0M)
                }
            };

            var request = new APPACalculationRequestModel()
            {
                Ap = null,
                Pa = paRequest
            };

            _paService.Setup(m => m.PerformProtectiveAwardCalculationAsync(paRequest, _options)).ReturnsAsync(paResponse);

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().BeNull();
            results.Pa.Should().NotBeNull();
            results.Pa.PayLines.Count(x => x.IsSelected).Should().Be(8);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_PerformsNoCalculationsWhenAPAndPAAreNull()
        {
            // Arrange
            var request = new APPACalculationRequestModel()
            {
                Ap = null,
                Pa = null
            };

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().BeNull();
            results.Pa.Should().BeNull();

            _apService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<ArrearsOfPayCalculationRequestModel>>(),
                It.IsAny<string>(),
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
            _paService.Verify(m => m.PerformProtectiveAwardCalculationAsync(
              It.IsAny<ProtectiveAwardCalculationRequestModel>(),
              It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_PerformsNoCalculationsWhenAPIsEmptyAndPAIsNull()
        {
            // Arrange
            var request = new APPACalculationRequestModel()
            {
                Ap = new List<ArrearsOfPayCalculationRequestModel>(),
                Pa = null
            };

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().BeNull();
            results.Pa.Should().BeNull();

            _apService.Verify(m => m.PerformCalculationAsync(
                It.IsAny<List<ArrearsOfPayCalculationRequestModel>>(),
                It.IsAny<string>(),
                It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
            _paService.Verify(m => m.PerformProtectiveAwardCalculationAsync(
              It.IsAny<ProtectiveAwardCalculationRequestModel>(),
              It.IsAny<IOptions<ConfigLookupRoot>>()), Times.Never);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_SelectRp14WhenZero()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var apRequests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp14a,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 0M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                }
            };

            var apResponseRP1 = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp1,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),428.57M, 508M, 257.14M, 257.14M,true,51.43M, 11.42M, 194.29M, 7, 3, 508M, 257.14M, 257.14M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 18.29M, 7, 4, 508M, 22.86M, 22.86M),
                }
            };

            var apResponseRP14a = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp14a,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),0M, 508M, 0M, 0M,true,0M, 0M, 0M, 7, 3, 508M, 0M, 0M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),0M, 508M, 0M, 0M,true,0M, 0M, 0M, 7, 4, 508M, 0M, 0M),
                }
            };

            var request = new APPACalculationRequestModel()
            {
                Ap = apRequests,
                Pa = null
            };

            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp1, _options)).ReturnsAsync(apResponseRP1);
            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp14a, _options)).ReturnsAsync(apResponseRP14a);

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().NotBeNull();
            results.Pa.Should().BeNull();

            results.Ap.SelectedInputSource.Should().Be(InputSource.Rp14a);
            results.Ap.RP1ResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(0);
            results.Ap.RP14aResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(2);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task PerformCalculationAsync_SelectRp1WhenZero()
        {
            // Arrange
            var shiftPattern = new List<string> { "1", "2", "3", "4", "5" };

            var apRequests = new List<ArrearsOfPayCalculationRequestModel>()
            {
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp1,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 0M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                },
                new ArrearsOfPayCalculationRequestModel()
                {
                    InputSource = InputSource.Rp14a,
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 04, 06),
                    DismissalDate = new DateTime(2018, 10, 20),
                    DateNoticeGiven = new DateTime(2018, 10, 14),
                    UnpaidPeriodFrom = new DateTime(2018, 10, 10),
                    UnpaidPeriodTo = new DateTime(2018, 10, 18),
                    ApClaimAmount = 600M,
                    IsTaxable = true,
                    PayDay = 6,
                    ShiftPattern = shiftPattern,
                    WeeklyWage = 400m
                }
            };

            var apResponseRP1 = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp1,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),0M, 508M, 0M, 0M,true,0M, 0M, 0M, 7, 3, 508M, 0M, 0M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),0M, 508M, 0M, 0M,true,0M, 0M, 0M, 7, 4, 508M, 0M, 0M),
                }
            };

            var apResponseRP14a = new ArrearsOfPayResponseDTO()
            {
                StatutoryMax = 508m,
                InputSource = InputSource.Rp14a,
                DngApplied = true,
                RunNWNP = true,
                WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                {
                    new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 13),428.57M, 508M, 257.14M, 257.14M,true,51.43M, 11.42M, 194.29M, 7, 3, 508M, 257.14M, 257.14M),
                    new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 20),428.57M, 508M, 22.86M, 22.86M,true,4.57M, 0M, 18.29M, 7, 4, 508M, 22.86M, 22.86M),
                }
            };

            var request = new APPACalculationRequestModel()
            {
                Ap = apRequests,
                Pa = null
            };

            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp1, _options)).ReturnsAsync(apResponseRP1);
            _apService.Setup(m => m.PerformCalculationAsync(apRequests, InputSource.Rp14a, _options)).ReturnsAsync(apResponseRP14a);

            // Act
            var results = await _service.PerformCalculationAsync(request, _options);

            // Assert
            results.Ap.Should().NotBeNull();
            results.Pa.Should().BeNull();

            results.Ap.SelectedInputSource.Should().Be(InputSource.Rp1);
            results.Ap.RP1ResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(2);
            results.Ap.RP14aResultsList.WeeklyResult.Count(x => x.IsSelected).Should().Be(0);
        }
    }
}



