namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class NoticePayCompositeCalculationResponseDTO
    {
        public NoticePayCompositeCalculationResponseDTO()
        {
           
        }

        public NoticeWorkedNotPaidCompositeOutput Nwnp { get; set; }

        public CompensatoryNoticePayCalculationResponseDTO Cnp { get; set; }
    }
}
