using System;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class ProjectedNoticeDateTestsDataGenerator
    {
        public static ProjectedNoticeDateCalculationRequestModel GetValidRequestData()
        {
            return new ProjectedNoticeDateCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2016, 02, 01),
                DismissalDate = new DateTime(2018, 01, 01),
                DateNoticeGiven = new DateTime(2018, 01, 02)
            };
        }

        public static ProjectedNoticeDateResponseDTO GetValidResponseData()
        {
            return new ProjectedNoticeDateResponseDTO
            {
                ProjectedNoticeDate = new DateTime(2018, 01, 08)
            };
        }

        public static ProjectedNoticeDateCalculationRequestModel GetInvalidRequestData_DismissalDataBeforeEmployeeStartDate()
        {
            return new ProjectedNoticeDateCalculationRequestModel
            {
                EmploymentStartDate = new DateTime(2018, 02, 01),
                DismissalDate = new DateTime(2018, 01, 01),
                DateNoticeGiven = new DateTime(2018, 06, 01)
            };
        }

        public static ProjectedNoticeDateCalculationRequestModel GetInvalidRequestData_InvalidEmployeeStartDate()
        {
            return new ProjectedNoticeDateCalculationRequestModel
            {
                EmploymentStartDate = DateTime.MinValue,
                DismissalDate = new DateTime(2018, 01, 01),
                DateNoticeGiven = new DateTime(2018, 06, 01)
            };
        }
    }
}