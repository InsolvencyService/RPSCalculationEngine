using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday
{
    public class IrregularHolidayPayAccruedCalculationRequestModel : HolidayPayAccruedCalculationRequestModel
    {

        /// <summary>
        /// HolidayAccruedDaysCore for irregular hours workers
        /// </summary>
        public decimal? HolidayAccruedDaysCore { get; set; } 

        /// <summary>
        /// for irregular hours workers
        /// HolidaysCarriedOverCoreSource - this is RP1 / RP14a / Override etc.
        /// (Need this to check that DaysCForward is not capped unnecessarily if the Source is “Override”
        /// </summary>
        public string HolidaysCarriedOverCoreSource { get; set; } 

        /// <summary>
        /// IrregularHoursWorker flag to identify the Regular Hour Worker or Irregular Hour Worker
        /// </summary>
        public bool IrregularHoursWorker { get; set; }
    }
}
