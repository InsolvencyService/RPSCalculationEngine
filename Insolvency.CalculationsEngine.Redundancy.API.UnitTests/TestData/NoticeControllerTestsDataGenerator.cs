using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class NoticeControllerTestsDataGenerator
    {
        public static NoticePayCompositeCalculationRequestModel GetValidRequestData()
        {
            return new NoticePayCompositeCalculationRequestModel()
            {
                Cnp = new CompensatoryNoticePayCalculationRequestModel
                {
                    InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                    InsolvencyDate = new DateTime(2018, 6, 1),
                    DismissalDate = new DateTime(2018, 06, 05),
                    DateNoticeGiven = new DateTime(2018, 06, 01),
                    WeeklyWage = 330.25m,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    IsTaxable = true,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    DeceasedDate = null
                },
                Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>
                {
                    new NoticeWorkedNotPaidCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp14a,
                        EmploymentStartDate = new DateTime(2015, 8, 2),
                        InsolvencyDate = new DateTime(2018, 7, 27),
                        DateNoticeGiven = new DateTime(2018, 7, 20),
                        DismissalDate = new DateTime(2018, 8, 8),
                        UnpaidPeriodFrom = new DateTime(2018, 7, 21),
                        UnpaidPeriodTo = new DateTime(2018, 8, 8),
                        WeeklyWage = 320,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        PayDay = 6,
                        IsTaxable = true
                    }
                }
            };
        }

        public static NoticePayCompositeCalculationResponseDTO GetValidResponseData()
        {
            return new NoticePayCompositeCalculationResponseDTO()
            {
                Cnp = new CompensatoryNoticePayCalculationResponseDTO(),
                Nwnp = new NoticeWorkedNotPaidCompositeOutput()
            };
        }
    }
}