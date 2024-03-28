using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class IrregularHolidayCalculationRequestModel 
    {
        public IrregularHolidayCalculationRequestModel()
        {
            Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>();
        }

        public bool Rp1NotRequired { get; set; }

        public bool Rp14aNotRequired { get; set; }

        public IrregularHolidayPayAccruedCalculationRequestModel Hpa { get; set; }

        public List<HolidayTakenNotPaidCalculationRequestModel> Htnp { get; set; }
    }
}
