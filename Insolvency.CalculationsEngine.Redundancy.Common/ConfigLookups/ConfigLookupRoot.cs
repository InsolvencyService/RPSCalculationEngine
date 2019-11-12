using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class ConfigLookupRoot
    {
        public List<StatMaxLookup> StatMaxLookup { get; set; }
        public List<NotionalBenefitLookup> NotionalBenefitsMonthlyRateUnder25 { get; set; }
        public List<NotionalBenefitLookup> NotionalBenefitsMonthlyRate25AndOver { get; set; }
        public List<NotionalBenefitLookup> NotionalBenefitsWeeklyRateUnder25 { get; set; }
        public List<NotionalBenefitLookup> NotionalBenefitsWeeklyRate25AndOver { get; set; }
        public List<BenefitWaitingDaysLookup> BenefitWaitingDaysLookup { get; set; }
        public List<TaxRateLookup> TaxRateLookup { get; set; }
        public List<NIRateLookup> NIRateLookup { get; set; }
        public List<NIRateLookup> NIUpperRateLookup { get; set; }
        public List<NIThresholdLookup> NIThresholdLookup { get; set; }
        public List<NIThresholdLookup> NIUpperThresholdLookup { get; set; }
        public List<PreferentialLimitLookup> PrefentialLimitLookup { get; set; }
    }
}