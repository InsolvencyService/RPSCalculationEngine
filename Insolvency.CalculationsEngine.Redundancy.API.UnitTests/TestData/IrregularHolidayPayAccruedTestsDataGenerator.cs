using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using System;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class IrregularHolidayPayAccruedTestsDataGenerator
    {
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

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithInvalidInsolvencyDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = DateTime.MinValue;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithInsolvencyDateBeforeEmpStartDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = new DateTime(2017, 3, 22);
            request.EmpStartDate = new DateTime(2017, 3, 23);
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithInvalidEmpStartDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.EmpStartDate = DateTime.MinValue;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithInvalidDismissalDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.DismissalDate = DateTime.MinValue;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithDismissalDateBeforeEmpStartDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.DismissalDate = new DateTime(2017, 3, 22);
            request.EmpStartDate = new DateTime(2017, 3, 23);
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithDismissalDateMoreThan1YearPriorToInsolvencyDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = new DateTime(2017, 3, 22);
            request.DismissalDate = new DateTime(2016, 3, 21);
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithInvalidHolidayYearStart()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.HolidayYearStart = DateTime.MinValue;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStartAfterDismissalDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = new DateTime(2017, 03, 22);
            request.DismissalDate = new DateTime(2017, 2, 20);
            request.HolidayYearStart = new DateTime(2017, 3, 1);
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStartAfterInsolvencyDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = new DateTime(2017, 03, 22);
            request.DismissalDate = new DateTime(2017, 3, 24);
            request.HolidayYearStart = new DateTime(2017, 3, 23);
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStart12MonthsBeforeDismissalDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = new DateTime(2017, 03, 22);
            request.DismissalDate = new DateTime(2017, 2, 20);
            request.HolidayYearStart = new DateTime(2016, 2, 20);
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStart12MonthsBeforeInsolvencyDate()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = new DateTime(2017, 02, 20);
            request.DismissalDate = new DateTime(2017, 3, 20);
            request.HolidayYearStart = new DateTime(2016, 2, 20);
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithHolidayYearStartBefore1stApril2024()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.InsolvencyDate = new DateTime(2022, 06, 20);
            request.DismissalDate = new DateTime(2022,06, 20);
            request.HolidayYearStart = new DateTime(2022, 2, 20);
            return request;
        }
        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullContractedHolEntitlement()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.ContractedHolEntitlement = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNegativeContractedHolEntitlement()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.ContractedHolEntitlement = -1m;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullIsTaxable()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.IsTaxable = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullPayDay()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.PayDay = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithInvalidPayDay()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.PayDay = 7;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullShiftPattern()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.ShiftPattern = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithInvalidShiftPattern()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.ShiftPattern = new List<string> { "1", "2", "7", "4", "5" };
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullWeeklyWage()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.WeeklyWage = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithZeroWeeklyWage()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.WeeklyWage = 0m;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNegativeWeeklyWage()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.WeeklyWage = -1m;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullDaysCFwd()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.DaysCFwd = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNegativeDaysCFwd()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.DaysCFwd = -1m;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullDaysTaken()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.DaysTaken = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNegativeDaysTaken()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.DaysTaken = -1m;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullIpConfirmedDays()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.IpConfirmedDays = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNegativeIpConfirmedDays()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.IpConfirmedDays = -1m;
            return request;
        }


        public static IrregularHolidayPayAccruedCalculationRequestModel GetValidRequestForIrregularHourWorkerData()
        {
            return new IrregularHolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2024, 06, 30),
                EmpStartDate = new DateTime(2021, 04, 01),
                DismissalDate = new DateTime(2024, 06, 30),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2024, 04, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Saturday,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 243.25m,
                DaysCFwd = 6m,
                DaysTaken = 8,
                IrregularHoursWorker = true,
                HolidayAccruedDaysCore = 20,
                HolidaysCarriedOverCoreSource = "Rp1"
            };
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetValidRequestForIrregularHourWorkerDataWithSource_Override()
        {
            return new IrregularHolidayPayAccruedCalculationRequestModel
            {
                InsolvencyDate = new DateTime(2024, 06, 30),
                EmpStartDate = new DateTime(2021, 04, 01),
                DismissalDate = new DateTime(2024, 06, 30),
                ContractedHolEntitlement = 28,
                HolidayYearStart = new DateTime(2024, 04, 01),
                IsTaxable = true,
                PayDay = (int)DayOfWeek.Saturday,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                WeeklyWage = 243.25m,
                DaysCFwd = 12m,
                DaysTaken = 8,
                IrregularHoursWorker = true,
                HolidayAccruedDaysCore = 12,
                HolidaysCarriedOverCoreSource = "Override"
            };
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNegativeHolidayAccuredCoreDays()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.HolidayAccruedDaysCore = -3m;
            request.HolidaysCarriedOverCoreSource = "rp14a";
            request.ShiftPattern = new List<string> { "3", "4", "5" };
            request.InsolvencyDate = new DateTime(2024, 05, 1);
            request.EmpStartDate = new DateTime(2021, 04, 01);
            request.DismissalDate = new DateTime(2024, 05, 01);
            request.DaysTaken = 1;
            request.DaysCFwd = 6;
            return request;
        }


        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithNullHolidayAccuredCoreDays()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.HolidayAccruedDaysCore = null;
            return request;
        }

        public static IrregularHolidayPayAccruedCalculationRequestModel GetRequestWithBlankHolidaysCarriedOverCoreSource()
        {
            var request = GetValidRequestForIrregularHourWorkerData();
            request.HolidaysCarriedOverCoreSource = "";
            return request;
        }
    }
}
