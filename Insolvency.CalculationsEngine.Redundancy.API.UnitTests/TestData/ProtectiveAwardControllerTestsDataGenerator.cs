using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class ProtectiveAwardControllerTestsDataGenerator
    {
        public static ProtectiveAwardCalculationRequestModel GetValidRequest()
        {
            return new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2017, 08, 01),
                DismissalDate = new DateTime(2017, 08, 08),
                TribunalAwardDate = new DateTime(2017, 09, 01),
                ProtectiveAwardStartDate = new DateTime(2018, 08, 10),
                ProtectiveAwardDays = 7,
                PayDay = 6,
                WeeklyWage = 320m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                Benefits = new List<ProtectiveAwardBenefit>(){
                    new ProtectiveAwardBenefit()
                    {
                        BenefitStartDate = new DateTime(2018, 08, 13),
                        BenefitEndDate = new DateTime(2018, 10, 07),
                        BenefitAmount = 120m
                    }
                }
            };
        }

        public static ProtectiveAwardCalculationRequestModel GetValidRequestWithoutBenefit()
        {
            return new ProtectiveAwardCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 08, 01),
                DismissalDate = new DateTime(2018, 08, 08),
                TribunalAwardDate = new DateTime(2018, 09, 01),
                ProtectiveAwardStartDate = new DateTime(2018, 08, 10),
                ProtectiveAwardDays = 7,
                PayDay = 6,
                WeeklyWage = 320m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
            };
        }

        public static ProtectiveAwardResponseDTO GetValidResponse()
        {
            return new ProtectiveAwardResponseDTO();
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidInsolvencyDate()
        {
            var request = GetValidRequest();
            request.InsolvencyDate = DateTime.MinValue;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithFutureInsolvencyDate()
        {
            var request = GetValidRequest();
            request.InsolvencyDate = DateTime.Now.AddDays(1);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInsolvencyDateBeforeEmployeeStartDate()
        {
            var request = GetValidRequest();
            request.EmploymentStartDate = new DateTime(2018, 02, 01);
            request.InsolvencyDate = new DateTime(2018, 01, 01);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.EmploymentStartDate = DateTime.MinValue;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithFutureEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.EmploymentStartDate = DateTime.Now.AddDays(1);
            request.InsolvencyDate = DateTime.Now.AddDays(2);
            request.DismissalDate = DateTime.Now.AddDays(2);
            return request;
        }

        public static object GetRequestWithProtectiveAwardDaysGreaterThanNinetyDaysLimit()
        {
            var request = GetValidRequest();
            request.ProtectiveAwardDays = 97;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidDismissalDate()
        {
            var request = GetValidRequest();
            request.DismissalDate = DateTime.MinValue;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithFutureDismissalDate()
        {
            var request = GetValidRequest();
            request.DismissalDate = DateTime.Now.AddDays(1);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithDismissalDateBeforeEmployeeStartDate()
        {
            var request = GetValidRequest();
            request.EmploymentStartDate = new DateTime(2018, 02, 01);
            request.DismissalDate = new DateTime(2018, 01, 01);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithLessThan3MonthsOfService()
        {
            var request = GetValidRequest();
            request.EmploymentStartDate = new DateTime(2018, 02, 01);
            request.DismissalDate = new DateTime(2018, 4, 30);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidTribunalAwardDate()
        {
            var request = GetValidRequest();
            request.TribunalAwardDate = DateTime.MinValue;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithFutureTribunalAwardDate()
        {
            var request = GetValidRequest();
            request.TribunalAwardDate = DateTime.Now.AddDays(1);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWitheTribunalAwardDateBeforeDismissalDate()
        {
            var request = GetValidRequest();
            request.TribunalAwardDate = new DateTime(2018, 01, 01);
            request.DismissalDate = new DateTime(2018, 02, 01);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidProtectiveAwardStartDate()
        {
            var request = GetValidRequest();
            request.ProtectiveAwardStartDate = DateTime.MinValue;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithFutureProtectiveAwardStartDate()
        {
            var request = GetValidRequest();
            request.ProtectiveAwardStartDate = DateTime.Now.AddDays(1);
            return request;
        }
        public static ProtectiveAwardCalculationRequestModel GetRequestWithNullProtectiveAwardDays()
        {
            var request = GetValidRequest();
            request.ProtectiveAwardDays = null;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithZeroProtectiveAwardDays()
        {
            var request = GetValidRequest();
            request.ProtectiveAwardDays = 0;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithNegativeProtectiveAwardDays()
        {
            var request = GetValidRequest();
            request.ProtectiveAwardDays = -1;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithNullPayDay()
        {
            var request = GetValidRequest();
            request.PayDay = null;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidPayDay()
        {
            var request = GetValidRequest();
            request.PayDay = 9;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithZeroWeeklyWage()
        {
            var request = GetValidRequest();
            request.WeeklyWage = 0;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithNegativeWeeklyWage()
        {
            var request = GetValidRequest();
            request.WeeklyWage = -1m;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithNullShiftPattern()
        {
            var request = GetValidRequest();
            request.ShiftPattern = null;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidShiftPattern()
        {
            var request = GetValidRequest();
            request.ShiftPattern = new List<string>();
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidBenefitStartDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitStartDate = DateTime.MinValue;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithFutureBenefitStartDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitStartDate = DateTime.Now.AddDays(1);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithInvalidBenefitEndDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitEndDate = DateTime.MinValue;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithFutureBenefitEndDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitEndDate = DateTime.Now.AddDays(1);
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithBenefitStartDateBeforeDimissalDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitStartDate = new DateTime(2016, 08, 08);
            request.Benefits[0].BenefitEndDate = new DateTime(2017, 01, 01);
            return request;
        }
        public static ProtectiveAwardCalculationRequestModel GetRequestWithBenefitEndDateBeforeDismissalDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitStartDate = new DateTime(2016, 01, 01);
            request.Benefits[0].BenefitEndDate = new DateTime(2016, 08, 08);
            return request;
        }


        public static ProtectiveAwardCalculationRequestModel GetRequestWithZeroBenefitAmount()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitAmount = 0;
            return request;
        }

        public static ProtectiveAwardCalculationRequestModel GetRequestWithNegativeBenefitAmount()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitAmount = -1m;
            return request;
        }
    }
}


