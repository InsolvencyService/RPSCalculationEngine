using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class APPAValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "Bad payload" };
            yield return new object[] {
                new APPACalculationRequestModel()
                {
                    Ap = null,
                    Pa = null
                },
                "Neither Arrears Of Pay nor Protective Award data has been provided" };
            yield return new object[] {
                new APPACalculationRequestModel()
                {
                    Ap = new List<ArrearsOfPayCalculationRequestModel>(),
                    Pa = null
                },
                "Neither Arrears Of Pay nor Protective Award data has been provided" };
            yield return new object[] {
                new APPACalculationRequestModel()
                {
                    Ap = new List<ArrearsOfPayCalculationRequestModel>()
                    {
                        new ArrearsOfPayCalculationRequestModel
                        {
                            InputSource = InputSource.Rp1,
                            InsolvencyDate = new DateTime(2017, 03, 22),
                            EmploymentStartDate = new DateTime(2016, 12, 19),
                            DismissalDate = new DateTime(2017, 01, 03),
                            DateNoticeGiven = new DateTime(2016, 12, 19),
                            UnpaidPeriodFrom = new DateTime(2016, 12, 19),
                            UnpaidPeriodTo = new DateTime(2017, 01, 03),
                            ApClaimAmount = 150,
                            IsTaxable = true,
                            PayDay = (int)DayOfWeek.Saturday,
                            ShiftPattern = new List<string> { "2", "3", "4", "5", "6" },
                            WeeklyWage = 243.25m
                        },
                        new ArrearsOfPayCalculationRequestModel
                        {
                            InputSource = InputSource.Rp1,
                            InsolvencyDate = new DateTime(2017, 03, 22),
                            EmploymentStartDate = new DateTime(2016, 12, 19),
                            DismissalDate = new DateTime(2017, 01, 03),
                            DateNoticeGiven = new DateTime(2016, 12, 19),
                            UnpaidPeriodFrom = new DateTime(2017, 1, 3),
                            UnpaidPeriodTo = new DateTime(2017, 01, 4),
                            ApClaimAmount = 150,
                            IsTaxable = true,
                            PayDay = (int)DayOfWeek.Saturday,
                            ShiftPattern = new List<string> { "2", "3", "4", "5", "6" },
                            WeeklyWage = 243.25m
                        }
                    },
                    Pa = null
                },
                "The same day appears in more than one Arrears Of Pay period" };
            yield return new object[] {
                new APPACalculationRequestModel()
                {
                    Ap = new List<ArrearsOfPayCalculationRequestModel>()
                    {
                        new ArrearsOfPayCalculationRequestModel
                        {
                            InputSource = InputSource.Rp14a,
                            InsolvencyDate = new DateTime(2017, 03, 22),
                            EmploymentStartDate = new DateTime(2016, 12, 19),
                            DismissalDate = new DateTime(2017, 01, 03),
                            DateNoticeGiven = new DateTime(2016, 12, 19),
                            UnpaidPeriodFrom = new DateTime(2016, 12, 19),
                            UnpaidPeriodTo = new DateTime(2017, 01, 03),
                            ApClaimAmount = 150,
                            IsTaxable = true,
                            PayDay = (int)DayOfWeek.Saturday,
                            ShiftPattern = new List<string> { "2", "3", "4", "5", "6" },
                            WeeklyWage = 243.25m
                        },
                        new ArrearsOfPayCalculationRequestModel
                        {
                            InputSource = InputSource.Rp14a,
                            InsolvencyDate = new DateTime(2017, 03, 22),
                            EmploymentStartDate = new DateTime(2016, 12, 19),
                            DismissalDate = new DateTime(2017, 01, 03),
                            DateNoticeGiven = new DateTime(2016, 12, 19),
                            UnpaidPeriodFrom = new DateTime(2017, 1, 3),
                            UnpaidPeriodTo = new DateTime(2017, 01, 4),
                            ApClaimAmount = 150,
                            IsTaxable = true,
                            PayDay = (int)DayOfWeek.Saturday,
                            ShiftPattern = new List<string> { "2", "3", "4", "5", "6" },
                            WeeklyWage = 243.25m
                        }
                    },
                    Pa = null
                },
                "The same day appears in more than one Arrears Of Pay period" };
            yield return new object[]
            {
                new APPACalculationRequestModel
                {
                    Rp14aNotRequired = false,
                    Ap = new List<ArrearsOfPayCalculationRequestModel>()
                {
                    new ArrearsOfPayCalculationRequestModel()
                    {
                        InputSource = InputSource.Rp14a,
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
                    Pa = null
                },
                "No Arrears Of Pay RP1 data has been not provided"
            };
            yield return new object[]
            {
                new APPACalculationRequestModel
                {
                    Rp14aNotRequired = false,
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
                        ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                        WeeklyWage = 400m
                    }
                },
                    Pa = null
                },
                "No Arrears Of Pay RP14a data has been not provided"
            };
        }
    
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}



