using System;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class NIRateLookup
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal NIRate { get; set; }
    }
}