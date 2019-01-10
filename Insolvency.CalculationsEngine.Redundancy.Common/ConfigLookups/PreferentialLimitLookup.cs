using System;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class PreferentialLimitLookup
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PreferentialLimit { get; set; }
    }
}