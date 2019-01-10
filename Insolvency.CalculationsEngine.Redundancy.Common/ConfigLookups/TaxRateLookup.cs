using System;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class TaxRateLookup
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TaxRate { get; set; }
    }
}