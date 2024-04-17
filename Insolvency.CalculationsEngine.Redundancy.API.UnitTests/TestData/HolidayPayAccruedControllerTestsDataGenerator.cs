using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class HolidayPayAccruedTestsDataGenerator
    {
        public static HolidayPayAccruedCalculationRequestModel GetValidRequestData()
        {
            return new HolidayPayAccruedCalculationRequestModel
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
            };
        }
        public static HolidayPayAccruedCalculationRequestModel GetRequestDataWithIpConfirmedDays()
        {
            return new HolidayPayAccruedCalculationRequestModel
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
                IpConfirmedDays = 5
            };
        }

        public static HolidayPayAccruedResponseDTO GetValidResponseData()
        {
            return new HolidayPayAccruedResponseDTO
            {
                StatutoryMax = 0.00m,
                HolidaysOwed = 0.00m,
                BusinessDaysInClaim = 261,
                WorkingDaysInClaim = 0.00m,
                ProRataAccruedDays = 0.00m,
                WeeklyResults = new List<HolidayPayAccruedWeeklyResult>(),
            };
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithInvalidInsolvencyDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = DateTime.MinValue;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithInsolvencyDateBeforeEmpStartDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = new DateTime(2017, 3, 22);
            request.EmpStartDate = new DateTime(2017, 3, 23);
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithInvalidEmpStartDate()
        {
            var request = GetValidRequestData();
            request.EmpStartDate = DateTime.MinValue;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithInvalidDismissalDate()
        {
            var request = GetValidRequestData();
            request.DismissalDate = DateTime.MinValue;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithDismissalDateBeforeEmpStartDate()
        {
            var request = GetValidRequestData();
            request.DismissalDate = new DateTime(2017, 3, 22);
            request.EmpStartDate = new DateTime(2017, 3, 23);
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithDismissalDateMoreThan1YearPriorToInsolvencyDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = new DateTime(2017, 3, 22);
            request.DismissalDate = new DateTime(2016, 3, 21);
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithInvalidHolidayYearStart()
        {
            var request = GetValidRequestData();
            request.HolidayYearStart = DateTime.MinValue;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStartAfterDismissalDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = new DateTime(2017, 03, 22);
            request.DismissalDate = new DateTime(2017, 2, 20);
            request.HolidayYearStart = new DateTime(2017, 3, 1);
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStartAfterInsolvencyDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = new DateTime(2017, 03, 22);
            request.DismissalDate = new DateTime(2017, 3, 24);
            request.HolidayYearStart = new DateTime(2017, 3, 23);
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStart12MonthsBeforeDismissalDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = new DateTime(2017, 03, 22);
            request.DismissalDate = new DateTime(2017, 2, 20);
            request.HolidayYearStart = new DateTime(2016, 2, 20);
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStart12MonthsBeforeInsolvencyDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = new DateTime(2017, 02, 20);
            request.DismissalDate = new DateTime(2017, 3, 20);
            request.HolidayYearStart = new DateTime(2016, 2, 20);
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullContractedHolEntitlement()
        {
            var request = GetValidRequestData();
            request.ContractedHolEntitlement = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNegativeContractedHolEntitlement()
        {
            var request = GetValidRequestData();
            request.ContractedHolEntitlement = -1m;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullIsTaxable()
        {
            var request = GetValidRequestData();
            request.IsTaxable = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullPayDay()
        {
            var request = GetValidRequestData();
            request.PayDay = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithInvalidPayDay()
        {
            var request = GetValidRequestData();
            request.PayDay = 7;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullShiftPattern()
        {
            var request = GetValidRequestData();
            request.ShiftPattern = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithInvalidShiftPattern()
        {
            var request = GetValidRequestData();
            request.ShiftPattern = new List<string> { "1", "2", "7", "4", "5" };
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullWeeklyWage()
        {
            var request = GetValidRequestData();
            request.WeeklyWage = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithZeroWeeklyWage()
        {
            var request = GetValidRequestData();
            request.WeeklyWage = 0m;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNegativeWeeklyWage()
        {
            var request = GetValidRequestData();
            request.WeeklyWage = -1m;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullDaysCFwd()
        {
            var request = GetValidRequestData();
            request.DaysCFwd = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNegativeDaysCFwd()
        {
            var request = GetValidRequestData();
            request.DaysCFwd = -1m;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullDaysTaken()
        {
            var request = GetValidRequestData();
            request.DaysTaken = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNegativeDaysTaken()
        {
            var request = GetValidRequestData();
            request.DaysTaken = -1m;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNullIpConfirmedDays()
        {
            var request = GetValidRequestData();
            request.IpConfirmedDays = null;
            return request;
        }

        public static HolidayPayAccruedCalculationRequestModel GetRequestWithNegativeIpConfirmedDays()
        {
            var request = GetValidRequestData();
            request.IpConfirmedDays = -1m;
            return request;
        }
    }
}