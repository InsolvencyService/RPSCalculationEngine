using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class HolidayTakenNotPaidControllerTestsDataGenerator
    {
        public static HolidayTakenNotPaidCalculationRequestModel GetValidRp1RequestData()
        {
            var holidayTakenNotPaidCalculationRequestModel = new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1,
                new DateTime(2018, 01, 10), new DateTime(2018, 01, 03), new DateTime(2017, 12, 20)
                , new DateTime(2017, 12, 29), 306.85m, new List<string> { "1", "2", "3", "4", "5" }, 6, true);
            return holidayTakenNotPaidCalculationRequestModel;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetValidRp14aRequestData()
        {
            var holidayTakenNotPaidCalculationRequestModel = new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a,
                new DateTime(2018, 01, 10), new DateTime(2018, 01, 03), new DateTime(2017, 12, 12)
                , new DateTime(2017, 12, 16), 306.85m, new List<string> { "1", "2", "3", "4", "5" }, 6, true);
            return holidayTakenNotPaidCalculationRequestModel;
        }

        public static HolidayTakenNotPaidResponseDTO GetValidRp1ResponseData()
        {
            return new HolidayTakenNotPaidResponseDTO(InputSource.Rp1, 489m, 
                new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(0, new DateTime(2017, 12, 16), 489m, 245.48m, 245.48m, true, 49.096m, 10.0176m, 186.3664m, 7, 4, 0m, 0m, 0m, false),
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2017, 12, 23), 489m, 306.85m, 306.85m, true, 61.37m, 17.382m, 228.098m, 7, 5, 0m, 0m, 0m, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2017, 12, 30), 489m, 245.48m, 245.48m, true, 49.096m, 10.0176m, 186.3664m, 7, 4, 0m, 0m, 0m, false)
                });
        }

        public static HolidayTakenNotPaidResponseDTO GetValidRp14aResponseData()
        {
            return new HolidayTakenNotPaidResponseDTO(InputSource.Rp14a, 489m,
                new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(0, new DateTime(2017, 12, 16), 489m, 245.48m, 245.48m, true, 49.096m, 10.0176m, 186.3664m, 7, 4, 0m, 0m, 0m, false),
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2017, 12, 23), 489m, 306.85m, 306.85m, true, 61.37m, 17.382m, 228.098m, 7, 5, 0m, 0m, 0m, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2017, 12, 30), 489m, 245.48m, 245.48m, true, 49.096m, 10.0176m, 186.3664m, 7, 4, 0m, 0m, 0m, false)
                });
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithNullInputSource()
        {
            var request = GetValidRp1RequestData();
            request.InputSource = null;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithEmptyInputSource()
        {
            var request = GetValidRp1RequestData();
            request.InputSource = string.Empty;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithInvalidInputSource()
        {
            var request = GetValidRp1RequestData();
            request.InputSource = "Wibble";
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithInvalidInsolvencyDate()
        {
            var request = GetValidRp1RequestData();
            request.InsolvencyDate = DateTime.MinValue;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithInvalidDismissalDate()
        {
            var request = GetValidRp1RequestData();
            request.DismissalDate = DateTime.MinValue;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithInvalidUnpaidPeriodFrom()
        {
            var request = GetValidRp1RequestData();
            request.UnpaidPeriodFrom = DateTime.MinValue;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithInvalidUnpaidPeriodTo()
        {
            var request = GetValidRp1RequestData();
            request.UnpaidPeriodTo = DateTime.MinValue;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithUnpaidFromDateAfterDismissalDate()
        {
            var request = GetValidRp1RequestData();
            request.DismissalDate = new DateTime(2018, 01, 03);
            request.UnpaidPeriodFrom = new DateTime(2018, 01, 05);
            request.UnpaidPeriodTo = new DateTime(2018, 01, 10);
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithUnpaidToDateAfterDismissalDate()
        {
            var request = GetValidRp1RequestData();
            request.DismissalDate = new DateTime(2018, 01, 03);
            request.UnpaidPeriodFrom = new DateTime(2018, 01, 05);
            request.UnpaidPeriodTo = new DateTime(2018, 01, 10);
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithUnpaidFromDateAfterInsolvencyDate()
        {
            var request = GetValidRp1RequestData();
            request.InsolvencyDate = new DateTime(2018, 01, 03);
            request.UnpaidPeriodFrom = new DateTime(2018, 01, 05);
            request.UnpaidPeriodTo = new DateTime(2018, 01, 10);
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithUnpaidToDateAfterInsolvencyDate()
        {
            var request = GetValidRp1RequestData();
            request.InsolvencyDate = new DateTime(2018, 01, 03);
            request.UnpaidPeriodFrom = new DateTime(2018, 01, 05);
            request.UnpaidPeriodTo = new DateTime(2018, 01, 10);
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithUnpaidToDateBeforeUnpaidFromDate()
        {
            var request = GetValidRp1RequestData();
            request.UnpaidPeriodFrom = new DateTime(2017, 12, 20);
            request.UnpaidPeriodTo = new DateTime(2017, 12, 10);
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithZeroWeeklyWage()
        {
            var request = GetValidRp1RequestData();
            request.WeeklyWage = 0m;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithNegativeWeeklyWage()
        {
            var request = GetValidRp1RequestData();
            request.WeeklyWage = -1m;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithNullShiftPattern()
        {
            var request = GetValidRp1RequestData();
            request.ShiftPattern = null;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithInvalidShiftPattern()
        {
            var request = GetValidRp1RequestData();
            request.ShiftPattern = new List<string>();
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithNullPayDay()
        {
            var request = GetValidRp1RequestData();
            request.PayDay = null;
            return request;
        }

        public static HolidayTakenNotPaidCalculationRequestModel GetRequestWithInvalidPayDay()
        {
            var request = GetValidRp1RequestData();
            request.PayDay = 9;
            return request;
        }
    }
}
