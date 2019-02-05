using System;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.RedundancyPaymentCalculation.Extensions;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Microsoft.Extensions.Options;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class RedundancyPaymentCalculationsService : IRedundanyPayCalculationsService
    {
        public async Task<RedundancyPaymentResponseDto> PerformRedundancyPayCalculationAsync(
            RedundancyPaymentCalculationRequestModel data, IOptions<ConfigLookupRoot> options)
        {
            var response = new ResponseDto<RedundancyPaymentResponseDto>();
            var calculationResult = new RedundancyPaymentResponseDto();
            
            var adjStartDate = await data.EmploymentStartDate.GetAdjustedEmploymentStartDate(data.EmploymentBreaks);
            var relevantNoticeDate = await data.DateNoticeGiven.GetRelevantNoticeDate(data.DismissalDate);
            var noticeEntitlementWeeks = await adjStartDate.GetNoticeEntitlementWeeks(relevantNoticeDate);

            var YearsOfService = await adjStartDate.GetServiceYearsAsync(data.DismissalDate);
            
            var projectedNoticeDate = await relevantNoticeDate.GetProjectedNoticeDate(noticeEntitlementWeeks);

            var relevantDismissalDate = await data.DismissalDate.GetRelevantDismissalDate(projectedNoticeDate);

            var statutoryMax = ConfigValueLookupHelper.GetStatutoryMax(options, relevantDismissalDate);

            var totalYearsOfService = await adjStartDate.GetServiceYearsAsync(relevantDismissalDate);
            YearsOfService = Math.Max(YearsOfService, totalYearsOfService);

            //limit maximum service years to 20
            if(totalYearsOfService > 20)
            {
                totalYearsOfService = 20;
            }
           
            var appliedRateOfPay = Math.Min(statutoryMax, data.WeeklyWage);

            int yearsOfServiceOver41 = 0, yearsOfService22To41 = 0, yearsOfServiceUpto21 = 0;

            yearsOfServiceOver41 = await data.DateOfBirth.GetYearsOfServiceOver41(adjStartDate, relevantDismissalDate);

            if(yearsOfServiceOver41 < totalYearsOfService)
            {
                yearsOfService22To41 = await data.DateOfBirth.GetYearsOfServiceFrom22To41(adjStartDate, relevantDismissalDate);
                if (yearsOfServiceOver41 + yearsOfService22To41 > totalYearsOfService)
                {
                    yearsOfService22To41 = totalYearsOfService - yearsOfServiceOver41;
                }
            }
            if (yearsOfServiceOver41 + yearsOfService22To41 < totalYearsOfService)
            {
                yearsOfServiceUpto21 = totalYearsOfService - (yearsOfService22To41 + yearsOfServiceOver41);
            }
           
            var redundancyPayWeeks = decimal.Multiply(yearsOfServiceUpto21 , 0.5m) + yearsOfService22To41 + decimal.Multiply(yearsOfServiceOver41, 1.5m);
            var grossEntitlement = redundancyPayWeeks * appliedRateOfPay;

            calculationResult.AdjEmploymentStartDate = adjStartDate;
            calculationResult.NoticeDateForRedundancyPay = relevantDismissalDate;
            calculationResult.NoticeEntitlementWeeks = noticeEntitlementWeeks;
            calculationResult.RedundancyPayWeeks = Math.Round(redundancyPayWeeks, 4);
            calculationResult.YearsOfServiceUpto21 = yearsOfServiceUpto21;
            calculationResult.YearsOfServiceFrom22To41 = yearsOfService22To41;
            calculationResult.YearsServiceOver41 = yearsOfServiceOver41;
            calculationResult.GrossEntitlement = Math.Max(0m, Math.Round(grossEntitlement, 2) - Math.Round(data.EmployerPartPayment, 2));
            calculationResult.EmployerPartPayment = Math.Round(data.EmployerPartPayment, 2);
            calculationResult.NetEntitlement = Math.Max(0m, Math.Round(grossEntitlement, 2) - Math.Round(data.EmployerPartPayment, 2));
            calculationResult.PreferentialClaim = 0m;
            calculationResult.NonPreferentialClaim = Math.Round(grossEntitlement, 2);
            return calculationResult;
        }
    }
}