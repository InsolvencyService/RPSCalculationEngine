using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class IrregularHolidayValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "Bad payload" };
            yield return new object[] {
                new IrregularHolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = null
                },
                "Neither Hpa nor any Htnp data has been provided" };
            yield return new object[] {
                new IrregularHolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                },
                "Neither Hpa nor any Htnp data has been provided" };
            yield return new object[] {
                new IrregularHolidayCalculationRequestModel()
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
                new IrregularHolidayCalculationRequestModel()
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
                new IrregularHolidayCalculationRequestModel
                {
                    Hpa = new IrregularHolidayPayAccruedCalculationRequestModel
                    {
                        InsolvencyDate = new DateTime(2024, 06, 22),
                        EmpStartDate = new DateTime(2021, 12, 19),
                        DismissalDate = new DateTime(2024, 06, 22),
                        ContractedHolEntitlement = 25,
                        HolidayYearStart = new DateTime(2014, 04, 01),
                        IsTaxable = true,
                        PayDay = (int)DayOfWeek.Saturday,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 243.25m,
                        DaysCFwd = 5.5m,
                        DaysTaken = 3.5m,
                       IrregularHoursWorker =true,
                       HolidaysCarriedOverCoreSource ="rp1",
                       HolidayAccruedDaysCore =6
                    },
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                    {
                        new HolidayTakenNotPaidCalculationRequestModel()
                        {
                            InputSource = InputSource.Rp14a,
                            InsolvencyDate = new DateTime(2024, 06, 22),
                            DismissalDate = new DateTime(2024, 06, 22),
                            UnpaidPeriodFrom = new DateTime(2024, 01, 12),
                            UnpaidPeriodTo = new DateTime(2024, 06, 22),
                            WeeklyWage = 306.85m,
                            ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                            PayDay = 6,
                            IsTaxable = true
                        },
                    }
                },
                "Holiday Taken Not Paid RP1 data has not been provided" };
            yield return new object[] {
                new IrregularHolidayCalculationRequestModel
                {
                    Rp14aNotRequired = false,
                    Hpa = new IrregularHolidayPayAccruedCalculationRequestModel
                    {
                        InsolvencyDate = new DateTime(2024, 06, 22),
                        EmpStartDate = new DateTime(2016, 12, 19),
                        DismissalDate = new DateTime(2024, 06, 22),
                        ContractedHolEntitlement = 25,
                        HolidayYearStart = new DateTime(2024, 04, 01),
                        IsTaxable = true,
                        PayDay = (int)DayOfWeek.Saturday,
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 243.25m,
                        DaysCFwd = 5.5m,
                        DaysTaken = 3.5m,
                        HolidayAccruedDaysCore =10,
                        HolidaysCarriedOverCoreSource ="Override",
                        IrregularHoursWorker =true
                    },
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                    {
                        new HolidayTakenNotPaidCalculationRequestModel()
                        {
                            InputSource = InputSource.Rp1,
                            InsolvencyDate = new DateTime(2021, 06, 22),
                            DismissalDate = new DateTime(2024, 06, 22),
                            UnpaidPeriodFrom = new DateTime(2024, 01, 01),
                            UnpaidPeriodTo = new DateTime(2024, 06, 22),
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
