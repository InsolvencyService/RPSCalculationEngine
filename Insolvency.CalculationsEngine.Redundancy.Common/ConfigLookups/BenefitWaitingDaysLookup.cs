using System;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class BenefitWaitingDaysLookup
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BenefitWaitingDays { get; set; }
    }
}