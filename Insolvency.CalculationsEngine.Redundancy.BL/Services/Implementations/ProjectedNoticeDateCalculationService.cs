using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.DTO;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations
{
    public class ProjectedNoticeDateCalculationService : IProjectedNoticeDateCalculationService
    {
        public async Task<ProjectedNoticeDateResponseDTO> PerformProjectedNoticeDateCalculationAsync(ProjectedNoticeDateCalculationRequestModel request, IOptions<ConfigLookupRoot> options)
        {
            var result = new ProjectedNoticeDateResponseDTO();

            var relNoticeDate = (request.DateNoticeGiven < request.DismissalDate) ? request.DateNoticeGiven : request.DismissalDate;

            // number of weeks entitle (based on years served) max 12 
            var noticeEntitlementWeeks = await request.EmploymentStartDate.GetServiceYearsAsync(relNoticeDate);
            noticeEntitlementWeeks = Math.Max(Math.Min(noticeEntitlementWeeks, 12), 1);

            result.ProjectedNoticeDate = relNoticeDate.AddDays(noticeEntitlementWeeks * 7);
            return await Task.FromResult(result);
        }
    }
}