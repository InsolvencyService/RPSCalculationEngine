using System;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups
{
    public class TestConfigLookupDataHelper
    {
        public ConfigLookupRoot PopulateConfigLookupRoot()
        {
            var configLookupRoot = new ConfigLookupRoot();
            PopulateStatutoryMaxLookup(configLookupRoot);
            PopulateNotionalBenefitLookup(configLookupRoot);
            PopulateBenefitWaitingLookup(configLookupRoot);
            PopulateTaxRateLookup(configLookupRoot);
            PopulateNIRateLookup(configLookupRoot);
            PopulateNIUpperRateLookup(configLookupRoot);
            PopulateNIThresholdLookup(configLookupRoot);
            PopulateNIUpperThresholdLookup(configLookupRoot);
            PopulatePreferentialLimitLookup(configLookupRoot);
            return configLookupRoot;
        }

        private void PopulatePreferentialLimitLookup(ConfigLookupRoot configLookupRoot)
        {
            var preferentialLimitLookupList =
                configLookupRoot.PrefentialLimitLookup = new List<PreferentialLimitLookup>();
            preferentialLimitLookupList.Add(new PreferentialLimitLookup
            {
                StartDate = new DateTime(1900, 01, 01),
                EndDate = new DateTime(9999, 12, 31),
                PreferentialLimit = 800
            });
        }

        private void PopulateTaxRateLookup(ConfigLookupRoot configLookupRoot)
        {
            var taxRateLookupList =
                configLookupRoot.TaxRateLookup = new List<TaxRateLookup>();
            taxRateLookupList.Add(new TaxRateLookup
            {
                StartDate = new DateTime(1900, 01, 01),
                EndDate = new DateTime(9999, 12, 31),
                TaxRate = 0.20m
            });
        }

        private void PopulateNIRateLookup(ConfigLookupRoot configLookupRoot)
        {
            configLookupRoot.NIRateLookup = new List<NIRateLookup>()
            {
                new NIRateLookup
                {
                    StartDate = new DateTime(1900, 01, 01),
                    EndDate = new DateTime(9999, 12, 31),
                    NIRate = 0.12m
                }
            };
        }

        private void PopulateNIUpperRateLookup(ConfigLookupRoot configLookupRoot)
        {
            configLookupRoot.NIUpperRateLookup = new List<NIRateLookup>()
            {
                new NIRateLookup
                {
                    StartDate = new DateTime(1900, 01, 01),
                    EndDate = new DateTime(9999, 12, 31),
                    NIRate = 0.02m
                }
            };
        }

        private void PopulateNIThresholdLookup(ConfigLookupRoot configLookupRoot)
        {
            configLookupRoot.NIThresholdLookup = new List<NIThresholdLookup>()
            {
                new NIThresholdLookup
                {
                    StartDate = new DateTime(1900, 01, 01),
                    EndDate = new DateTime(9999, 12, 31),
                    NIThreshold = 166m
                }
            };
        }

        private void PopulateNIUpperThresholdLookup(ConfigLookupRoot configLookupRoot)
        {
            configLookupRoot.NIUpperThresholdLookup = new List<NIThresholdLookup>()
            {
                new NIThresholdLookup
                {
                    StartDate = new DateTime(1900, 01, 01),
                    EndDate = new DateTime(9999, 12, 31),
                    NIThreshold = 962m
                }
            };
        }

        private void PopulateBenefitWaitingLookup(ConfigLookupRoot configLookupRoot)
        {
            var benefitWaitingDaysLookupList =
                configLookupRoot.BenefitWaitingDaysLookup = new List<BenefitWaitingDaysLookup>();
            benefitWaitingDaysLookupList.Add(new BenefitWaitingDaysLookup
            {
                StartDate = new DateTime(2011, 04, 06),
                EndDate = new DateTime(9999, 12, 31),
                BenefitWaitingDays = 7
            });
        }

        private void PopulateNotionalBenefitLookup(ConfigLookupRoot configLookupRoot)
        {
            configLookupRoot.NotionalBenefitsMonthlyRateUnder25 =
                new List<NotionalBenefitLookup>()
                {
                    new NotionalBenefitLookup()
                    {
                        StartDate =  new DateTime(1900, 1, 1),
                        EndDate = new DateTime(9999, 12, 31),
                        Amount = 251.77m
                    }
                };
            configLookupRoot.NotionalBenefitsMonthlyRate25AndOver = 
                new List<NotionalBenefitLookup>()
                {
                    new NotionalBenefitLookup()
                    {
                        StartDate =  new DateTime(1900, 1, 1),
                        EndDate = new DateTime(9999, 12, 31),
                        Amount = 317.82m
                    }
                };
        }

        private void PopulateStatutoryMaxLookup(ConfigLookupRoot configLookupRoot)
        {
            var statMaxList = configLookupRoot.StatMaxLookup = new List<StatMaxLookup>();
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2011, 02, 01),
                EndDate = new DateTime(2012, 01, 31),
                StatMax = 400
            });

            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2011, 02, 01),
                EndDate = new DateTime(2012, 01, 31),
                StatMax = 400
            });
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2011, 02, 01),
                EndDate = new DateTime(2013, 01, 31),
                StatMax = 430
            });
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2013, 02, 01),
                EndDate = new DateTime(2014, 04, 05),
                StatMax = 450
            });
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2014, 04, 06),
                EndDate = new DateTime(2015, 04, 05),
                StatMax = 464
            });
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2015, 04, 06),
                EndDate = new DateTime(2016, 04, 05),
                StatMax = 475
            });
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2016, 04, 06),
                EndDate = new DateTime(2017, 04, 05),
                StatMax = 479
            });
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2017, 04, 06),
                EndDate = new DateTime(2018, 04, 05),
                StatMax = 489
            });
            statMaxList.Add(new StatMaxLookup
            {
                StartDate = new DateTime(2018, 04, 06),
                EndDate = new DateTime(9999, 12, 31),
                StatMax = 508
            });
        }
    }
}