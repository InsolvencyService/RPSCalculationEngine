using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class APPAControllerTestsDataGenerator
    {
        public static APPACalculationRequestModel GetValidRequestData()
        {
            return new APPACalculationRequestModel
            {
                Ap = new List<ArrearsOfPayCalculationRequestModel>()
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
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
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
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 400m
                    }
                },
                Pa = new ProtectiveAwardCalculationRequestModel()
                {
                    InsolvencyDate = new DateTime(2018, 10, 20),
                    EmploymentStartDate = new DateTime(2016, 4, 6),
                    DismissalDate = new DateTime(2018, 10, 20),
                    TribunalAwardDate = new DateTime(2018, 10, 21),
                    ProtectiveAwardStartDate = new DateTime(2018, 10, 22),
                    ProtectiveAwardDays = 90,
                    PayDay = 6,
                    WeeklyWage = 400M,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    paBenefitStartDate = DateTime.MinValue,
                    paBenefitAmount = decimal.Zero
                }
            };
        }
        public static APPACalculationResponseDTO GetValidResponseData()
        {
            return new APPACalculationResponseDTO()
            {
                Ap = new ArrearsOfPayAggregateOutput()
                {
                    SelectedInputSource = InputSource.Rp1,
                    RP1ResultsList = new ArrearsOfPayResponseDTO()
                    {
                        InputSource = InputSource.Rp1,
                        StatutoryMax = 508M,
                        DngApplied = true,
                        RunNWNP = true,
                        WeeklyResult = new List<ArrearsOfPayWeeklyResult>()
                        {
                            new ArrearsOfPayWeeklyResult(1, new DateTime(2018, 10, 6), 500M, 508M, 500M, 500M, true, 100M, 40.56M, 359.44M, 7, 5, 508, 500, 500, true),
                            new ArrearsOfPayWeeklyResult(2, new DateTime(2018, 10, 13), 500M, 508M, 40M, 40M, true, 8M, 0M, 32M, 7, 2, 508, 40, 40, false)
                        }
                    },
                    RP14aResultsList = null
                },
                Pa = new ProtectiveAwardResponseDTO()
                {
                    IsTaxable = true,
                    PayLines = new List<ProtectiveAwardPayLine>()
                    {
                        new ProtectiveAwardPayLine(1, new DateTime(2018, 10, 27), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(2, new DateTime(2018, 11, 03), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(3, new DateTime(2018, 11, 10), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(4, new DateTime(2018, 11, 17), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(5, new DateTime(2018, 11, 24), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(6, new DateTime(2018, 12, 01), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(7, new DateTime(2018, 12, 08), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(8, new DateTime(2018, 12, 15), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(9, new DateTime(2018, 12, 22), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(10, new DateTime(2018, 12, 29), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(11, new DateTime(2019, 01, 05), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(12, new DateTime(2019, 01, 12), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true),
                        new ProtectiveAwardPayLine(13, new DateTime(2019, 01, 19), 0M, 400M, 80M, 28.56M, 291.44M, 0, 0, 0, true)
                    }
                }
            };
        }
    }
}