using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate
{
    public class ProjectedNoticeDateResponseDTO
    {
        public ProjectedNoticeDateResponseDTO()
        {
        }

        public ProjectedNoticeDateResponseDTO(DateTime projectedNoticeDate)
        {
            ProjectedNoticeDate = projectedNoticeDate;
        }

        public string TraceInfo { get; set; } = TraceInfoSerializer.GetTraceDetails();

        public DateTime ProjectedNoticeDate { get; set; }

        public DateTime NoticeStartDate { get; set; }
    }
}
