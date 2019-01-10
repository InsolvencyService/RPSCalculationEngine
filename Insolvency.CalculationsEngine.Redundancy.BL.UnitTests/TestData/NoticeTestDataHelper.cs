using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using System;
using System.Collections;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData
{
    public class NoticeTestDataHelper
    {
        public class NoticeRequestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    
                    //CNP not null, NWNP null
                    new NoticePayCompositeCalculationRequestModel()
                    {
                        Cnp = CnpRequestData(),Nwnp = null
                    },
                    new NoticePayCompositeCalculationResponseDTO(){
                        Cnp = CnpResponseData(),
                        Nwnp = new NoticeWorkedNotPaidCompositeOutput()//null
                    },

                    //CNP null, NWNP not null
                    new NoticePayCompositeCalculationRequestModel()
                    {
                        Cnp = null, Nwnp = NwnpRequestData()
                    },
                     new NoticePayCompositeCalculationResponseDTO(){
                        Cnp = new CompensatoryNoticePayCalculationResponseDTO(),//null
                        Nwnp = NwnpResponseData(),
                    },

                      //CNP & NWNP not null
                    new NoticePayCompositeCalculationRequestModel()
                    {
                        Cnp = CnpRequestData(), Nwnp = NwnpRequestData()
                    },
                     new NoticePayCompositeCalculationResponseDTO(){
                        Cnp = CnpResponseData(),
                        Nwnp = NwnpResponseData()
                    },
                };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private static CompensatoryNoticePayCalculationRequestModel CnpRequestData()
        {
            return new CompensatoryNoticePayCalculationRequestModel()
            {
                InsolvencyEmploymentStartDate = new DateTime(2014, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 22),
                DateNoticeGiven = new DateTime(2018, 06, 11),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                Benefits = new List<CompensatoryNoticePayBenefit>()
                                    {
                                        new CompensatoryNoticePayBenefit()
                                        {
                                            BenefitStartDate = new DateTime(2018, 06, 05),
                                            BenefitEndDate = new DateTime(2018, 06, 10),
                                            BenefitAmount = 123.45m
                                        }
                                    },
                NewEmployments = new List<CompensatoryNoticePayNewEmployment>()
                                    {
                                        new CompensatoryNoticePayNewEmployment()
                                        {
                                            NewEmploymentStartDate = new DateTime(2018, 06, 11),
                                            NewEmploymentEndDate = new DateTime(2018, 07, 10),
                                            NewEmploymentWeeklyWage = 120m,
                                            NewEmploymentWage = 256.56m
                                        }
                                    },
                WageIncreases = new List<CompensatoryNoticePayWageIncrease>()
                                    {
                                        new CompensatoryNoticePayWageIncrease()
                                        {
                                            WageIncreaseStartDate = new DateTime(2018, 07, 11),
                                            WageIncreaseEndDate = new DateTime(2018, 08, 31),
                                            WageIncreaseAmount = 312.50m
                                        }
                                    }
            };
        }

        private static List<NoticeWorkedNotPaidCalculationRequestModel> NwnpRequestData()
        {
            return new List<NoticeWorkedNotPaidCalculationRequestModel>()
                        {
                            new NoticeWorkedNotPaidCalculationRequestModel()
                            {
                                InputSource = InputSource.Rp14a,
                                EmploymentStartDate = new DateTime(2014, 02, 01),
                                InsolvencyDate = new DateTime(2018, 6, 1),
                                DateNoticeGiven = new DateTime(2018, 6, 11),
                                DismissalDate = new DateTime(2018, 06, 22),
                                UnpaidPeriodFrom = new DateTime(2018, 6, 11),
                                UnpaidPeriodTo = new DateTime(2018, 6, 15),
                                WeeklyWage = 330.25m,
                                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                                PayDay = 6,
                                IsTaxable = true
                            },
                            new NoticeWorkedNotPaidCalculationRequestModel()
                            {
                                InputSource = InputSource.Rp14a,
                                EmploymentStartDate = new DateTime(2014, 02, 01),
                                InsolvencyDate = new DateTime(2018, 6, 1),
                                DateNoticeGiven = new DateTime(2018, 06, 11),
                                DismissalDate = new DateTime(2018, 06, 22),
                                UnpaidPeriodFrom = new DateTime(2018, 6, 18),
                                UnpaidPeriodTo = new DateTime(2018, 6, 22),
                                WeeklyWage = 330.25m,
                                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                                PayDay = 6,
                                IsTaxable = true
                            }
                        };
        }

        private static CompensatoryNoticePayCalculationResponseDTO CnpResponseData()
        {
            return new CompensatoryNoticePayCalculationResponseDTO() {
                NoticeWeeksDue = 3,
                NoticeStartDate = new DateTime(2018, 06, 12),
                ProjectedNoticeDate = new DateTime(2018, 07, 09),
                CompensationEndDate = new DateTime(2018, 07, 09),
                DaysInClaim = 13,
                WeeklyResults = new List<CompensatoryNoticePayResult>()
                {
                    new CompensatoryNoticePayResult()
                    {
                        WeekNumber = 0,
                        EmployerEntitlement = 198.15m,
                        BenefitsDeducted = 0,
                        NewEmploymentDeducted = 88m,
                        WageIncreaseDeducted = 0,
                        NotionalBenefitDeducted = 0,
                        GrossEntitlement = 110.15m,
                        IsTaxable = true,
                        NotionalTaxDeducted = 0,
                        TaxDeducted = 22.03m,
                        NIDeducted = 0,
                        NetEntitlement = 88.12m,
                        PreferentialClaim = 0,
                        NonPreferentialClaim = 110.15m,
                        IsSelected = false
                    },
                    new CompensatoryNoticePayResult()
                    {
                        WeekNumber = 1,
                        EmployerEntitlement = 330.25m,
                        BenefitsDeducted = 0,
                        NewEmploymentDeducted = 123.2m,
                        WageIncreaseDeducted = 0,
                        NotionalBenefitDeducted = 0,
                        GrossEntitlement = 207.05m,
                        IsTaxable = true,
                        NotionalTaxDeducted = 0,
                        TaxDeducted = 41.41m,
                        NIDeducted = 5.41m,
                        NetEntitlement = 160.23m,
                        PreferentialClaim = 0,
                        NonPreferentialClaim = 207.05m,
                        IsSelected = false
                    },
                    new CompensatoryNoticePayResult()
                    {
                        WeekNumber = 2,
                        EmployerEntitlement = 330.25m,
                        BenefitsDeducted = 0,
                        NewEmploymentDeducted = 123.2m,
                        WageIncreaseDeducted = 0,
                        NotionalBenefitDeducted = 0,
                        GrossEntitlement = 207.05m,
                        IsTaxable = true,
                        NotionalTaxDeducted = 0,
                        TaxDeducted = 41.41m,
                        NIDeducted = 5.41m,
                        NetEntitlement = 160.23m,
                        PreferentialClaim = 0,
                        NonPreferentialClaim = 207.05m,
                        IsSelected = false
                    }
                }
            };
        }
       
        private static NoticeWorkedNotPaidCompositeOutput NwnpResponseData()
        {
           return new NoticeWorkedNotPaidCompositeOutput()
                {
                    SelectedInputSource = "rp14a",
                    rp1Results = null,
                    rp14aResults = new NoticeWorkedNotPaidResponseDTO("rp14a", 508m, new List<NoticeWorkedNotPaidWeeklyResult>(){
                                    new NoticeWorkedNotPaidWeeklyResult()
                                    {
                                        WeekNumber = 0,
                                        PayDate = new DateTime(2018, 6, 18),
                                        MaximumEntitlement = 508m,
                                        EmployerEntitlement = 264.20m,
                                        GrossEntitlement = 264.20m,
                                        IsTaxable = true,
                                        TaxDeducted = 52.84m,
                                        NiDeducted = 12.26m,
                                        NetEntitlement = 199.10m,
                                        MaximumDays = 4,
                                        EmploymentDays = 4,
                                        MaximumEntitlementIn4MonthPeriod = 0m,
                                        EmployerEntitlementIn4MonthPeriod = 0m,
                                        GrossEntitlementIn4Months = 0m,
                                        IsSelected = false
                                    },
                                    new NoticeWorkedNotPaidWeeklyResult()
                                    {
                                        WeekNumber = 1,
                                        PayDate = new DateTime(2018, 6, 25),
                                        MaximumEntitlement = 508m,
                                        EmployerEntitlement = 264.20m,
                                        GrossEntitlement = 264.20m,
                                        IsTaxable = true,
                                        TaxDeducted = 52.84m,
                                        NiDeducted = 12.26m,
                                        NetEntitlement = 199.10m,
                                        MaximumDays = 4,
                                        EmploymentDays = 4,
                                        MaximumEntitlementIn4MonthPeriod = 0m,
                                        EmployerEntitlementIn4MonthPeriod = 0m,
                                        GrossEntitlementIn4Months = 0m,
                                        IsSelected = false
                                    }
                            })
                        
                
            };
        }
    }
    
}
