using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment
{
    public class RedundancyPaymentResponseDto
    {
        public RedundancyPaymentResponseDto()
        {
        }

        public RedundancyPaymentResponseDto(DateTime adjStartDate,int noticeWeeks, DateTime relNoticeDate, int upto21, int from22to41, int over41,decimal redWeeks, decimal gross, decimal empPartPayment, decimal net, decimal statutoryMaximum)
        {
            AdjEmploymentStartDate = adjStartDate;
            NoticeEntitlementWeeks = noticeWeeks;
            NoticeDateForRedundancyPay = relNoticeDate;
            YearsOfServiceUpto21 = upto21;
            YearsOfServiceFrom22To41 = from22to41;
            YearsServiceOver41 = over41;
            RedundancyPayWeeks = redWeeks;
            GrossEntitlement = gross;
            EmployerPartPayment = empPartPayment;
            NetEntitlement = net;
            StatutoryMax = statutoryMaximum;
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();
        public DateTime AdjEmploymentStartDate { get; set; }
        public int NoticeEntitlementWeeks { get; set; }
        public DateTime NoticeDateForRedundancyPay { get; set; }
        public int YearsOfServiceUpto21 { get; set; }
        public int YearsOfServiceFrom22To41 { get; set; }
        public int YearsServiceOver41 { get; set; }
        public decimal RedundancyPayWeeks { get; set; }
        public decimal GrossEntitlement { get; set; }
        public decimal EmployerPartPayment { get; set; }
        public decimal NetEntitlement { get; set; }
        public decimal PreferentialClaim { get; set; }
        public decimal NonPreferentialClaim { get; set; }
        public decimal StatutoryMax { get; set; }
    }
}