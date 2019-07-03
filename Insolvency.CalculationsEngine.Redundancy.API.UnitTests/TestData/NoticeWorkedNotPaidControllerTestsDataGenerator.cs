using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class NoticeWorkedNotPaidControllerTestsDataGenerator
    {
        public static NoticeWorkedNotPaidCalculationRequestModel GetValidRP14aRequest()
        {
            //voilates model validation weekly wage is negative amount
            return new NoticeWorkedNotPaidCalculationRequestModel()
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
                IsTaxable = true,
                ApClaimAmount = 100
            };
        }

        public static NoticeWorkedNotPaidCalculationRequestModel GetValidRP1Request()
        {
            //voilates model validation weekly wage is negative amount
            return new NoticeWorkedNotPaidCalculationRequestModel()
            {
                InputSource = InputSource.Rp1,
                EmploymentStartDate = new DateTime(2015, 8, 2),
                InsolvencyDate = new DateTime(2018, 7, 27),
                DateNoticeGiven = new DateTime(2018, 7, 20),
                DismissalDate = new DateTime(2018, 8, 8),
                UnpaidPeriodFrom = new DateTime(2018, 7, 21),
                UnpaidPeriodTo = new DateTime(2018, 8, 8),
                WeeklyWage = 320,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                PayDay = 6,
                IsTaxable = true,
                ApClaimAmount = 100
            };
        }

        public static NoticeWorkedNotPaidCalculationRequestModel GetBadRequest()
        {
            //voilates model validation weekly wage is negative amount
            return new NoticeWorkedNotPaidCalculationRequestModel()
            {
                InputSource = InputSource.Rp14a,
                EmploymentStartDate = new DateTime(2007, 01, 1),
                InsolvencyDate = new DateTime(2009, 03, 29),
                DismissalDate = new DateTime(2009, 03, 30),
                DateNoticeGiven = new DateTime(2009, 03, 30),
                UnpaidPeriodFrom = new DateTime(2009, 1, 20),
                UnpaidPeriodTo = new DateTime(2009, 01, 19),
                WeeklyWage = 306.85m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                PayDay = 6,
                IsTaxable = true
            };
        }

        public static NoticeWorkedNotPaidCalculationRequestModel GetRequestWithNullInputSource()
        {
            var request = GetValidRP14aRequest();
            request.InputSource = null;
            return request;
        }

        public static NoticeWorkedNotPaidCalculationRequestModel GetRequestWithEmptyInputSource()
        {
            var request = GetValidRP14aRequest();
            request.InputSource = string.Empty;
            return request;
        }

        public static NoticeWorkedNotPaidCalculationRequestModel GetRequestWithInvalidInputSource()
        {
            var request = GetValidRP14aRequest();
            request.InputSource = "Wibble";
            return request;
        }
    }
}