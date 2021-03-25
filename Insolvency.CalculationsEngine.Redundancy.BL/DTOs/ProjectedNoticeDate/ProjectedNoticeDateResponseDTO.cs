using Insolvency.CalculationsEngine.Redundancy.BL.Serializer.Extensions;
using System;


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

        public string TraceInfo { get; set; } = TraceInfoSerializer.ConvertToJson();

        public DateTime ProjectedNoticeDate { get; set; }

        public DateTime NoticeStartDate { get; set; }
    }
}
