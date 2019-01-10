using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayCalculationResponseDTO
    {
        public HolidayPayAccruedResponseDTO Hpa { get; set; }

        public HolidayTakenNotPaidAggregateOutput Htnp { get; set; }
    }
}
