using System;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public class RedundancyPaymentTestsDataGenerator
    {
        public RedundancyPaymentCalculationRequestModel GetInvalidRequestData()
        {
            return new RedundancyPaymentCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                //invalid dismissal date
                DismissalDate = DateTime.MinValue,
                DateNoticeGiven = new DateTime(2018, 06, 03),
                DateOfBirth = new DateTime(1975, 03, 15),
                WeeklyWage = 400,
                EmployerPartPayment = 0,
                EmploymentBreaks = 0
            };
        }

        public RedundancyPaymentCalculationRequestModel GetInvalidRequestData_WeeklyWage_Zero()
        {
            return new RedundancyPaymentCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                DismissalDate = new DateTime(2018, 07, 03),
                DateNoticeGiven = new DateTime(2018, 06, 03),
                DateOfBirth = new DateTime(1975, 03, 15),
                WeeklyWage = 0,
                EmployerPartPayment = 0,
                EmploymentBreaks = 0
            };
        }


        public RedundancyPaymentCalculationRequestModel
           GetRPRequestData_With_InsolvencyDate_OutOfStatutoryMaxLookupBoundaries()
        {
            return new RedundancyPaymentCalculationRequestModel()
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                DismissalDate = new DateTime(2018, 06, 03),
                DateNoticeGiven = new DateTime(2018, 06, 03),
                DateOfBirth = new DateTime(1975, 03, 15),
                WeeklyWage = 400,
                EmployerPartPayment = 0,
                EmploymentBreaks = 0
            };
        }
        public RedundancyPaymentCalculationRequestModel GetValidRequestData()
        {
            return new RedundancyPaymentCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                DismissalDate = new DateTime(2018, 06, 03),
                DateNoticeGiven = new DateTime(2018, 06, 03),
                DateOfBirth = new DateTime(1975, 03, 15) ,
                WeeklyWage = 400 ,
                EmployerPartPayment = 0,
                EmploymentBreaks = 0
            };
        }

        public RedundancyPaymentCalculationRequestModel GetNullData()
        {
            return null;
        }

        public RedundancyPaymentResponseDto GetValidCalculationResults()
        {
            return new RedundancyPaymentResponseDto
            {
                AdjEmploymentStartDate = new DateTime(2016, 02, 01),
                NoticeEntitlementWeeks = 2,
                NoticeDateForRedundancyPay = new DateTime(2018, 06, 03),
                YearsOfServiceUpto21 = 0,
                YearsOfServiceFrom22To41 = 0,
                YearsServiceOver41 = 1,
                RedundancyPayWeeks = 1.5m,
                GrossEntitlement = 900,
                EmployerPartPayment = 200,
                NetEntitlement = 600,
                PreferentialClaim = 0,
                NonPreferentialClaim = 900m
            };
        }
    }
}