using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class HolidayCalculationRequestModel
    {
        public HolidayCalculationRequestModel()
        {
            Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>();
        }

        public HolidayPayAccruedCalculationRequestModel Hpa { get; set; }

        public List<HolidayTakenNotPaidCalculationRequestModel> Htnp { get; set; }
    }
}
