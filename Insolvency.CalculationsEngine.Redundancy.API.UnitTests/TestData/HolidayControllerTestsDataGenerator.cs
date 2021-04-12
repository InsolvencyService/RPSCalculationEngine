using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class HolidayControllerTestsDataGenerator
    {
        public static HolidayCalculationRequestModel GetValidRequestData()
        {
            return new HolidayCalculationRequestModel()
            {
                Hpa = new HolidayPayAccruedCalculationRequestModel
                {
                    InsolvencyDate = new DateTime(2017, 03, 22),
                    EmpStartDate = new DateTime(2016, 12, 19),
                    DismissalDate = new DateTime(2017, 03, 20),
                    ContractedHolEntitlement = 25,
                    HolidayYearStart = new DateTime(2017, 01, 01),
                    IsTaxable = true,
                    PayDay = (int)DayOfWeek.Saturday,
                    ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                    WeeklyWage = 243.25m,
                    DaysCFwd = 5.5m,
                    DaysTaken = 3.5m,
                    IpConfirmedDays = 25
                },
                Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                {
                    new HolidayTakenNotPaidCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp1,
                        InsolvencyDate = new DateTime(2018, 01, 10),
                        DismissalDate = new DateTime(2018, 01, 03),
                        UnpaidPeriodFrom = new DateTime(2017, 12, 12),
                        UnpaidPeriodTo = new DateTime(2017, 12, 29),
                        WeeklyWage = 306.85m,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        PayDay = 6,
                        IsTaxable = true
                    },
                    new HolidayTakenNotPaidCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp14a,
                        InsolvencyDate = new DateTime(2018, 01, 10),
                        DismissalDate = new DateTime(2018, 01, 03),
                        UnpaidPeriodFrom = new DateTime(2017, 12, 12),
                        UnpaidPeriodTo = new DateTime(2017, 12, 29),
                        WeeklyWage = 306.85m,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        PayDay = 6,
                        IsTaxable = true
                    }
                }
            };
        }

        public static HolidayCalculationResponseDTO GetValidResponseData()
        {
            return new HolidayCalculationResponseDTO()
            {
                Hpa = new HolidayPayAccruedResponseDTO(),
                Htnp = new HolidayTakenNotPaidAggregateOutput()
            };
        }
    }
}