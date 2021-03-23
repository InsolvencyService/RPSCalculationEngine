using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;
using System;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class CompensatoryNoticePayCalculationResponseDTO
    {
        public CompensatoryNoticePayCalculationResponseDTO()
        {
            WeeklyResults = new List<CompensatoryNoticePayResult>();
        }
        public string TraceInfo { get; set; } = TraceInfoSerializer.ConvertToJson();

        public int NoticeWeeksDue { get; set; }

        public decimal StatutoryMax { get; set; }

        public decimal MaxCNPEntitlement { get; set; }

        public DateTime NoticeStartDate { get; set; }

        public DateTime ProjectedNoticeDate { get; set; }

        public DateTime CompensationEndDate { get; set; }

        public int DaysInClaim { get; set; }

        public List <CompensatoryNoticePayResult> WeeklyResults { get; set; }
       
    }
}
