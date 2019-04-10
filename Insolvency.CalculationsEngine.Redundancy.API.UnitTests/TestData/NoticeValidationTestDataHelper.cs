using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class NoticeValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "Bad payload" };
            yield return new object[] {
                new NoticePayCompositeCalculationRequestModel()
                {
                    Cnp = null,
                    Nwnp = null
                },
                "Neither NWNP nor CNP data has been provided" };
            yield return new object[] {
                new NoticePayCompositeCalculationRequestModel()
                {
                    Cnp = null,
                    Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>()
                },
                "Neither NWNP nor CNP data has been provided" };
            yield return new object[] {
                new NoticePayCompositeCalculationRequestModel()
                {
                    Cnp = null,
                    Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>()
                    {
                        new NoticeWorkedNotPaidCalculationRequestModel()
                        {
                             InputSource = InputSource.Rp1,
                             EmploymentStartDate = new DateTime(2015, 8, 2),
                             InsolvencyDate = new DateTime(2018, 7, 20),
                             DateNoticeGiven = new DateTime(2018, 7, 20),
                             DismissalDate = new DateTime(2018, 7, 20),
                             UnpaidPeriodFrom = new DateTime(2018, 7, 1),
                             UnpaidPeriodTo = new DateTime(2018, 7, 8),
                             WeeklyWage = 320,
                             ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                             PayDay = 6,
                             IsTaxable = true
                         },
                        new NoticeWorkedNotPaidCalculationRequestModel()
                        {
                             InputSource = InputSource.Rp1,
                             EmploymentStartDate = new DateTime(2015, 8, 2),
                             InsolvencyDate = new DateTime(2018, 7, 20),
                             DateNoticeGiven = new DateTime(2018, 7, 20),
                             DismissalDate = new DateTime(2018, 7, 20),
                             UnpaidPeriodFrom = new DateTime(2018, 7, 8),
                             UnpaidPeriodTo = new DateTime(2018, 7, 10),
                             WeeklyWage = 320,
                             ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                             PayDay = 6,
                             IsTaxable = true
                         }
                    }
                },
                "The same day appears in more than one Notice Worked Not Paid period" };
            yield return new object[] {
                new NoticePayCompositeCalculationRequestModel()
                {
                    Cnp = null,
                    Nwnp = new List<NoticeWorkedNotPaidCalculationRequestModel>()
                    {
                        new NoticeWorkedNotPaidCalculationRequestModel()
                        {
                             InputSource = InputSource.Rp14a,
                             EmploymentStartDate = new DateTime(2015, 8, 2),
                             InsolvencyDate = new DateTime(2018, 7, 20),
                             DateNoticeGiven = new DateTime(2018, 7, 20),
                             DismissalDate = new DateTime(2018, 7, 20),
                             UnpaidPeriodFrom = new DateTime(2018, 7, 1),
                             UnpaidPeriodTo = new DateTime(2018, 7, 8),
                             WeeklyWage = 320,
                             ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                             PayDay = 6,
                             IsTaxable = true
                         },
                        new NoticeWorkedNotPaidCalculationRequestModel()
                        {
                             InputSource = InputSource.Rp14a,
                             EmploymentStartDate = new DateTime(2015, 8, 2),
                             InsolvencyDate = new DateTime(2018, 7, 20),
                             DateNoticeGiven = new DateTime(2018, 7, 20),
                             DismissalDate = new DateTime(2018, 7, 20),
                             UnpaidPeriodFrom = new DateTime(2018, 7, 8),
                             UnpaidPeriodTo = new DateTime(2018, 7, 10),
                             WeeklyWage = 320,
                             ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                             PayDay = 6,
                             IsTaxable = true
                         }
                    }
                },
                "The same day appears in more than one Notice Worked Not Paid period" };
            yield return new object[] {
                new NoticePayCompositeCalculationRequestModel()
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
                },
                "Notice Worked Not Paid RP1 data has been not provided" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}



