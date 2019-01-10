using System;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class NIThresholdLookup
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal NIThreshold { get; set; }
    }
}