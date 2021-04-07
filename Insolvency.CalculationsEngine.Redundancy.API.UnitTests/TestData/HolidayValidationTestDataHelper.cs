using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class HolidayValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "Bad payload" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = null
                },
                "Neither Hpa nor any Htnp data has been provided" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                },
                "Neither Hpa nor any Htnp data has been provided" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                    {
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 10), new DateTime(2018, 9, 18), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 18), new DateTime(2018, 9, 20), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                    }
                },
                "The same day appears in more than one Holiday Taken Not Paid period" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                    {
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 10), new DateTime(2018, 9, 18), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 18), new DateTime(2018, 9, 20), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                    }
                },
                "The same day appears in more than one Holiday Taken Not Paid period" };
            yield return new object[] {
                new HolidayCalculationRequestModel
                {
                    Rp14aNotRequired = true,
                    Rp1NotRequired = false,
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
                            InputSource = InputSource.Rp14a,
                            InsolvencyDate = new DateTime(2018, 01, 10),
                            DismissalDate = new DateTime(2018, 01, 03),
                            UnpaidPeriodFrom = new DateTime(2017, 12, 12),
                            UnpaidPeriodTo = new DateTime(2017, 12, 29),
                            WeeklyWage = 306.85m,
                            ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                            PayDay = 6,
                            IsTaxable = true
                        },
                    }
                },
                "Holiday Taken Not Paid RP1 data has not been provided" };
            yield return new object[] {
                new HolidayCalculationRequestModel
                {
                    Rp14aNotRequired = false,
                    Rp1NotRequired = true,
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
                    }
                },
                "Holiday Taken Not Paid RP14a data has not been provided" };
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}



