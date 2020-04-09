using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class ArrearsOfPayTestsDataGenerator
    {
        public static ArrearsOfPayCalculationRequestModel GetValidRequestData()
        {
            return new ArrearsOfPayCalculationRequestModel
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
            };
        }
        
        public static ArrearsOfPayResponseDTO GetAoPCalcResult_ForEmploymentNotStartedInFirstWeek()
        {
            var arrearsOfPayWeeklyResultsList = new List<ArrearsOfPayWeeklyResult>
            {
                new ArrearsOfPayWeeklyResult
                {
                    WeekNumber = 1,
                    PayDate = new DateTime(2017, 09, 02),
                    ApPayRate = 259.5m,
                    MaximumEntitlement = 489,
                    EmployerEntitlement = 103.8m,
                    GrossEntitlement = 103.8m,
                    IsTaxable = true,
                    TaxDeducted = 20.76m,
                    NIDeducted = 0,
                    NetEntitlement = 83.04m,
                    MaximumDays = 7,
                    EmploymentDays = 2,
                    MaximumEntitlementIn4MonthPeriod = 489,
                    EmployerEntitlementIn4MonthPeriod = 103.8m,
                    GrossEntitlementIn4Months = 103.8m
                },
                new ArrearsOfPayWeeklyResult
                {
                    WeekNumber = 2,
                    PayDate = new DateTime(2017, 09, 09),
                    ApPayRate = 259.5m,
                    MaximumEntitlement = 489,
                    EmployerEntitlement = 259.5m,
                    GrossEntitlement = 259.5m,
                    IsTaxable = true,
                    TaxDeducted = 51.9m,
                    NIDeducted = 9.18m,
                    NetEntitlement = 198.42m,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 489,
                    EmployerEntitlementIn4MonthPeriod = 259.5m,
                    GrossEntitlementIn4Months = 259.5m
                },
                new ArrearsOfPayWeeklyResult
                {
                    WeekNumber = 3,
                    PayDate = new DateTime(2017, 09, 16),
                    ApPayRate = 259.5m,
                    MaximumEntitlement = 489,
                    EmployerEntitlement = 259.5m,
                    GrossEntitlement = 259.5m,
                    IsTaxable = true,
                    TaxDeducted = 51.9m,
                    NIDeducted = 9.18m,
                    NetEntitlement = 198.42m,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 489,
                    EmployerEntitlementIn4MonthPeriod = 259.5m,
                    GrossEntitlementIn4Months = 259.5m
                },
                new ArrearsOfPayWeeklyResult
                {
                    WeekNumber = 4,
                    PayDate = new DateTime(2017, 09, 23),
                    ApPayRate = 259.5m,
                    MaximumEntitlement = 489,
                    EmployerEntitlement = 259.5m,
                    GrossEntitlement = 259.5m,
                    IsTaxable = true,
                    TaxDeducted = 51.9m,
                    NIDeducted = 9.18m,
                    NetEntitlement = 198.42m,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 489,
                    EmployerEntitlementIn4MonthPeriod = 259.5m,
                    GrossEntitlementIn4Months = 259.5m
                },
                new ArrearsOfPayWeeklyResult
                {
                    WeekNumber = 5,
                    PayDate = new DateTime(2017, 09, 30),
                    ApPayRate = 259.5m,
                    MaximumEntitlement = 489,
                    EmployerEntitlement = 259.5m,
                    GrossEntitlement = 259.5m,
                    IsTaxable = true,
                    TaxDeducted = 51.9m,
                    NIDeducted = 9.18m,
                    NetEntitlement = 198.42m,
                    MaximumDays = 7,
                    EmploymentDays = 5,
                    MaximumEntitlementIn4MonthPeriod = 489,
                    EmployerEntitlementIn4MonthPeriod = 259.5m,
                    GrossEntitlementIn4Months = 259.5m
                }
            };
            return new ArrearsOfPayResponseDTO
            {
                InputSource = InputSource.Rp1,
                StatutoryMax = 489,
                WeeklyResult = arrearsOfPayWeeklyResultsList
            };
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithNullInputSource()
        {
            var request = GetValidRequestData();
            request.InputSource = null;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithEmptyInputSource()
        {
            var request = GetValidRequestData();
            request.InputSource = string.Empty;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidInputSource()
        {
            var request = GetValidRequestData();
            request.InputSource = "Wibble";
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidInsolvencyDate()
        {
            var request = GetValidRequestData();
            request.InsolvencyDate = DateTime.MinValue;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidEmploymentStartDate()
        {
            var request = GetValidRequestData();
            request.EmploymentStartDate = DateTime.MinValue;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidDismissalDate()
        {
            var request = GetValidRequestData();
            request.DismissalDate = DateTime.MinValue;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithEmploymentStartDateAfterDismissalDate()
        {
            var request = GetValidRequestData();
            request.EmploymentStartDate = new DateTime(2017, 01, 04);
            request.DismissalDate = new DateTime(2017, 01, 03);
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidUnpaidPeriodFrom()
        {
            var request = GetValidRequestData();
            request.UnpaidPeriodFrom = DateTime.MinValue;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithUnpaidPeriodFromAfterDismissalDate()
        {
            var request = GetValidRequestData();
            request.UnpaidPeriodFrom = new DateTime(2017, 01, 04);
            request.DismissalDate = new DateTime(2017, 01, 03);
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidUnpaidPeriodTo()
        {
            var request = GetValidRequestData();
            request.UnpaidPeriodTo = DateTime.MinValue;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidDateNoticeGiven()
        {
            var request = GetValidRequestData();
            request.DateNoticeGiven = DateTime.MinValue;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithUnpaidPeriodFromAfterInsolvencyDate()
        {
            var request = GetValidRequestData();
            request.UnpaidPeriodFrom = new DateTime(2017, 01, 04);
            request.InsolvencyDate = new DateTime(2017, 01, 03);
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithUnpaidPeriodFromAfterUnpaidPeriodTo()
        {
            var request = GetValidRequestData();
            request.UnpaidPeriodFrom = new DateTime(2017, 01, 04);
            request.UnpaidPeriodTo = new DateTime(2017, 01, 03);
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithDateNoticeGivenAfterDismissalDate()
        {
            var request = GetValidRequestData();
            request.DismissalDate = new DateTime(2017, 01, 03);
            request.DateNoticeGiven = new DateTime(2017, 1, 4);
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithDateNoticeGivenBeforeEmploymentStartDate()
        {
            var request = GetValidRequestData();
            request.EmploymentStartDate = new DateTime(2017, 01, 05);
            request.DateNoticeGiven = new DateTime(2017, 1, 4);
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithUnpaidPeriodToMoreThan7MOnthsAfterUnpaidPeriodFrom()
        {
            var request = GetValidRequestData();
            request.UnpaidPeriodFrom = new DateTime(2017, 01, 04);
            request.UnpaidPeriodTo = new DateTime(2017, 08, 05);
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithZeroWeeklyWage()
        {
            var request = GetValidRequestData();
            request.WeeklyWage = 0M;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithNegativeWeeklyWage()
        {
            var request = GetValidRequestData();
            request.WeeklyWage = -1M;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithNullShiftPattern()
        {
            var request = GetValidRequestData();
            request.ShiftPattern = null;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidShiftPattern()
        {
            var request = GetValidRequestData();
            request.ShiftPattern = new List<string>();
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithNullPayDay()
        {
            var request = GetValidRequestData();
            request.PayDay = null;
            return request;
        }

        public static ArrearsOfPayCalculationRequestModel GetRequestWithInvalidPayDay()
        {
            var request = GetValidRequestData();
            request.PayDay = 9;
            return request;
        }
    }
}