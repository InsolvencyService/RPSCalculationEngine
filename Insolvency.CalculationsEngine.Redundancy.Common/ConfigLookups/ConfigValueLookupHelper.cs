using System;
using System.Linq;
using Insolvency.CalculationsEngine.Redundancy.Common.Exceptions;
using Microsoft.Extensions.Options;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public static class ConfigValueLookupHelper
    {
        public static decimal GetStatutoryMax(IOptions<ConfigLookupRoot> options, DateTime insolvencyDate)
        {
            var statutoryMax = options.Value.StatMaxLookup.Where(x => x.StartDate.Date <= insolvencyDate.Date
                                                          && x.EndDate.Date >= insolvencyDate.Date)
                .Select(x => x.StatMax).FirstOrDefault();

            if (statutoryMax == 0m)
                throw new MissingConfigurationException("unable to determine the statutory max weekly pay");

            return statutoryMax;
        }

        public static decimal GetTaxRate(IOptions<ConfigLookupRoot> options, DateTime date)
        {
            var taxRate = options.Value.TaxRateLookup.Where(x => x.StartDate.Date <= date.Date
                                                          && x.EndDate.Date >= date.Date)
                .Select(x => x.TaxRate).FirstOrDefault();

            if (taxRate == 0m)
                throw new MissingConfigurationException("unable to determine the tax rate");

            return taxRate;
        }

        public static decimal GetNIRate(IOptions<ConfigLookupRoot> options, DateTime date)
        {
            var niRate = options.Value.NIRateLookup.Where(x => x.StartDate.Date <= date.Date
                                                          && x.EndDate.Date >= date.Date)
                .Select(x => x.NIRate).FirstOrDefault();

            if (niRate == 0m)
                throw new MissingConfigurationException("unable to determine the NI rate");

            return niRate;
        }

        public static decimal GetNIUpperRate(IOptions<ConfigLookupRoot> options, DateTime date)
        {
            var niUpperRate = options.Value.NIUpperRateLookup.Where(x => x.StartDate.Date <= date.Date
                                                          && x.EndDate.Date >= date.Date)
                .Select(x => x.NIRate).FirstOrDefault();

            if (niUpperRate == 0m)
                throw new MissingConfigurationException("unable to determine the NI upper rate");

            return niUpperRate;
        }

        public static decimal GetNIThreshold(IOptions<ConfigLookupRoot> options, DateTime date)
        {
            var niThreshold = options.Value.NIThresholdLookup.Where(x => x.StartDate.Date <= date.Date
                                                          && x.EndDate.Date >= date.Date)
                .Select(x => x.NIThreshold).FirstOrDefault();

            if (niThreshold == 0m)
                throw new MissingConfigurationException("unable to determine the NI threshold");

            return niThreshold;
        }

        public static decimal GetNIUpperThreshold(IOptions<ConfigLookupRoot> options, DateTime date)
        {
            var niUpperThreshold = options.Value.NIUpperThresholdLookup.Where(x => x.StartDate.Date <= date.Date
                                                          && x.EndDate.Date >= date.Date)
                .Select(x => x.NIThreshold).FirstOrDefault();

            if (niUpperThreshold == 0m)
                throw new MissingConfigurationException("unable to determine the NI upper threshold");

            return niUpperThreshold;
        }

        public static decimal GetPreferentialLimit(IOptions<ConfigLookupRoot> options, DateTime date)
        {
            var preferentialLimit = options.Value.PrefentialLimitLookup.Where(x => x.StartDate.Date <= date.Date
                                                          && x.EndDate.Date >= date.Date)
                .Select(x => x.PreferentialLimit).FirstOrDefault();

            if (preferentialLimit == 0m)
                throw new MissingConfigurationException("unable to determine the preferential limit");

            return preferentialLimit;
        }

        public static decimal GetBenefitsWaitingDays(IOptions<ConfigLookupRoot> options, DateTime benefitsStartDate)
        {
            var waitingDays = options.Value.BenefitWaitingDaysLookup.Where(x => x.StartDate.Date <= benefitsStartDate.Date
                                                                     && x.EndDate.Date >= benefitsStartDate.Date)
                .Select(x => x.BenefitWaitingDays).FirstOrDefault();
            if (waitingDays == 0m)
                throw new MissingConfigurationException("unable to determine the Notional Benefits Wating Days");

            return waitingDays;
        }

        public static decimal GetNotionalBenefitsMonthlyRate(IOptions<ConfigLookupRoot> options, DateTime date, int ageInYears)
        {
            var lookups = (ageInYears < 25) ? options.Value.NotionalBenefitsMonthlyRateUnder25 : options.Value.NotionalBenefitsMonthlyRate25AndOver;

            var notionalBenefitMonthlyRate = lookups.Where(x => x.StartDate.Date <= date.Date && x.EndDate.Date >= date.Date)
                    .Select(x => x.Amount)
                    .FirstOrDefault();

            if (notionalBenefitMonthlyRate == 0m)
                throw new MissingConfigurationException("unable to determine the Notional Benefits Monthly rate");

            return notionalBenefitMonthlyRate;
        }

        public static decimal GetNotionalBenefitsWeeklyRate(IOptions<ConfigLookupRoot> options, DateTime date, int ageInYears)
        {
            var lookups = (ageInYears < 25) ? options.Value.NotionalBenefitsWeeklyRateUnder25 : options.Value.NotionalBenefitsWeeklyRate25AndOver;

            var notionalBenefitWeeklyRate = lookups.Where(x => x.StartDate.Date <= date.Date && x.EndDate.Date >= date.Date)
                    .Select(x => x.Amount)
                    .FirstOrDefault();

            if (notionalBenefitWeeklyRate == 0m)
                throw new MissingConfigurationException("unable to determine the Notional Benefits Weekly rate");

            return notionalBenefitWeeklyRate;
        }
    }
}