using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA{
    public class APPACalculationRequestModel
    {
        public APPACalculationRequestModel()
        {
            Ap = new List<ArrearsOfPayCalculationRequestModel>();
        }

        public bool Rp14aNotRequired { get; set; }

        public List<ArrearsOfPayCalculationRequestModel> Ap { get; set; }

        public ProtectiveAwardCalculationRequestModel Pa { get; set; }
    }
}
