using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class NoticePayCompositeCalculationRequestModel
    {
        public NoticePayCompositeCalculationRequestModel()
        {
           
        }

        public bool Rp1NotRequired { get; set; }

        public List<NoticeWorkedNotPaidCalculationRequestModel> Nwnp { get; set; }

        public CompensatoryNoticePayCalculationRequestModel Cnp { get; set; }

    }
}
