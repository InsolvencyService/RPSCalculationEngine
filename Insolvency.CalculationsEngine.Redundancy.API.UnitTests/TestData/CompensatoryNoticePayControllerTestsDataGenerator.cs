using System;
using System.Collections.Generic;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    public static class CompensatoryNoticePayControllerTestsDataGenerator
    {
        public static CompensatoryNoticePayCalculationRequestModel GetValidRequest()
        {
            return new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
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
                },
                NotionalBenefitOverrides = new List<CompensatoryNoticePayNotionalBenefitOverride>()
                {
                    new CompensatoryNoticePayNotionalBenefitOverride()
                    {
                        NotionalBenefitOverrideStartDate = new DateTime(2018, 07, 11),
                        NotionalBenefitOverrideEndDate = new DateTime(2018, 08, 31)
                    }
                }
            };
        }

        public static CompensatoryNoticePayCalculationResponseDTO GetResponse()
        {
            return new CompensatoryNoticePayCalculationResponseDTO
            {
                WeeklyResults = new List<CompensatoryNoticePayResult>()
                {
                    new CompensatoryNoticePayResult()
                    {
                        PreferentialClaim = 0m,
                        NonPreferentialClaim = 247.950m,
                        BenefitsDeducted = 82.300m,
                        EmployerEntitlement = 330.25m,
                        GrossEntitlement = 247.950m,
                        IsTaxable = true,
                        NIDeducted = 10.31400m,
                        NetEntitlement = 188.04600m,
                        NewEmploymentDeducted = 0m,
                        NotionalBenefitDeducted = 0m,
                        NotionalTaxDeducted = 49.59000m,
                        WageIncreaseDeducted = 0m,
                        WeekNumber = 0
                    },
                    new CompensatoryNoticePayResult()
                    {
                        PreferentialClaim = 0m,
                        NonPreferentialClaim = 246.340m,
                        BenefitsDeducted = 41.150m,
                        EmployerEntitlement = 330.25m,
                        GrossEntitlement = 246.340m,
                        IsTaxable = true,
                        NIDeducted = 10.12080m,
                        NetEntitlement = 186.95120m,
                        NewEmploymentDeducted = 42.760m,
                        NotionalBenefitDeducted = 0m,
                        NotionalTaxDeducted = 49.26800m,
                        WageIncreaseDeducted = 0m,
                        WeekNumber = 1
                    },
                    new CompensatoryNoticePayResult()
                    {
                        PreferentialClaim = 0m,
                        NonPreferentialClaim = 270.386m,
                        BenefitsDeducted = 0m,
                        EmployerEntitlement = 330.25m,
                        GrossEntitlement = 270.386m,
                        IsTaxable = true,
                        NIDeducted = 13.00632m,
                        NetEntitlement = 203.30248m,
                        NewEmploymentDeducted = 59.864m,
                        NotionalBenefitDeducted = 0m,
                        NotionalTaxDeducted = 54.07720m,
                        WageIncreaseDeducted = 0m,
                        WeekNumber = 2
                    }
                },
                CompensationEndDate = new DateTime(2018, 6, 26),
                DaysInClaim = 17,
                NoticeStartDate = new DateTime(2018, 6, 2),
                NoticeWeeksDue = 3,
                ProjectedNoticeDate = new DateTime(2018, 6, 19),
            };
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithDismissalDateBefore5thApril2018()
        {
            return new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 3, 1),
                DismissalDate = new DateTime(2018, 03, 05),
                DateNoticeGiven = new DateTime(2018, 03, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                Benefits = new List<CompensatoryNoticePayBenefit>()
                {
                    new CompensatoryNoticePayBenefit()
                    {
                        BenefitStartDate = new DateTime(2018, 03, 05),
                        BenefitEndDate = new DateTime(2018, 03, 10),
                        BenefitAmount = 123.45m
                    }
                },
                NewEmployments = new List<CompensatoryNoticePayNewEmployment>()
                {
                    new CompensatoryNoticePayNewEmployment()
                    {
                        NewEmploymentStartDate = new DateTime(2018, 03, 11),
                        NewEmploymentEndDate = new DateTime(2018, 04, 10),
                        NewEmploymentWeeklyWage = 120m,
                        NewEmploymentWage = 256.56m
                    }
                },
                WageIncreases = new List<CompensatoryNoticePayWageIncrease>()
                {
                    new CompensatoryNoticePayWageIncrease()
                    {
                        WageIncreaseStartDate = new DateTime(2018, 04, 11),
                        WageIncreaseEndDate = new DateTime(2018, 05, 31),
                        WageIncreaseAmount = 312.50m
                    }
                }
            };
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithBenefitHigherThanWeeklyWage()
        {
            return new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
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
                        BenefitEndDate = new DateTime(2018, 06, 08),
                        BenefitAmount = 500m
                    }
                }
            };
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNewEmploymentHigherThanWeeklyWage()
        {
            return new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                NewEmployments = new List<CompensatoryNoticePayNewEmployment>()
                {
                    new CompensatoryNoticePayNewEmployment()
                    {
                        NewEmploymentStartDate = new DateTime(2018, 06, 05),
                        NewEmploymentEndDate = new DateTime(2018, 06, 08),
                        NewEmploymentWeeklyWage = 400m,
                        NewEmploymentWage = 500m
                    }
                }
            };
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithWageIncreaseHigherThanWeeklyWage()
        {
            return new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2016, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                DeceasedDate = null,
                WageIncreases = new List<CompensatoryNoticePayWageIncrease>()
                {
                    new CompensatoryNoticePayWageIncrease()
                    {
                        WageIncreaseStartDate = new DateTime(2018, 06, 05),
                        WageIncreaseEndDate = new DateTime(2018, 06, 08),
                        WageIncreaseAmount = 500m
                    }
                }
            };
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithDeceasedDate()
        {
            return new CompensatoryNoticePayCalculationRequestModel
            {
                InsolvencyEmploymentStartDate = new DateTime(2000, 02, 01),
                InsolvencyDate = new DateTime(2018, 6, 1),
                DismissalDate = new DateTime(2018, 06, 05),
                DateNoticeGiven = new DateTime(2018, 06, 01),
                WeeklyWage = 330.25m,
                ShiftPattern = new List<string> { "1", "2", "3", "4", "5" },
                IsTaxable = true,
                DateOfBirth = new DateTime(1980, 1, 1),
                DeceasedDate = new DateTime(2018, 06, 11)
            };
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidInsolvencyEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.InsolvencyEmploymentStartDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInsolvencyEmploymentStartDateBeforeDateOfBirth()
        {
            var request = GetValidRequest();
            request.InsolvencyEmploymentStartDate = new DateTime(1989, 1, 1);
            request.DateOfBirth = new DateTime(1990, 1, 1);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidInsolvencyDate()
        {
            var request = GetValidRequest();
            request.InsolvencyDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidDismissalDate()
        {
            var request = GetValidRequest();
            request.DismissalDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithDismissalDateBeforeInsolvencyEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.DismissalDate = new DateTime(2018, 06, 02);
            request.InsolvencyEmploymentStartDate = new DateTime(2018, 06, 03);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithDismissalDateBeforeDateNoticeGiven()
        {
            var request = GetValidRequest();
            request.DismissalDate = new DateTime(2018, 06, 02);
            request.DateNoticeGiven = new DateTime(2018, 06, 03);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithDismissalDateLessThat1MonthAfterInsolvencyEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.DismissalDate = new DateTime(2018, 06, 04);
            request.InsolvencyEmploymentStartDate = new DateTime(2018, 06, 03);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidDateNoticeGiven()
        {
            var request = GetValidRequest();
            request.DateNoticeGiven = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithDateNoticeGivenBeforeInsolvencyEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.DateNoticeGiven = new DateTime(2018, 04, 02);
            request.InsolvencyEmploymentStartDate = new DateTime(2018, 04, 03);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithZeroWeeklyWage()
        {
            var request = GetValidRequest();
            request.WeeklyWage = 0;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNegativeWeeklyWage()
        {
            var request = GetValidRequest();
            request.WeeklyWage = -1m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNullShiftPattern()
        {
            var request = GetValidRequest();
            request.ShiftPattern = null;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidShiftPattern()
        {
            var request = GetValidRequest();
            request.ShiftPattern = new List<string>();
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidDateOfBirth()
        {
            var request = GetValidRequest();
            request.DateOfBirth = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidDeceasedDate()
        {
            var request = GetValidRequest();
            request.DeceasedDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithDeceasedDateBeforeDateOfBirth()
        {
            var request = GetValidRequest();
            request.DateOfBirth = new DateTime(1990, 01, 01);
            request.DeceasedDate= new DateTime(1989, 01, 01);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidBenefitStartDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitStartDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidBenefitEndDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitEndDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithBenefitEndDateBeforeBenefitStartDate()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitStartDate = new DateTime(2018, 01, 01);
            request.Benefits[0].BenefitEndDate = new DateTime(2017, 01, 01);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithZeroBenefitAmount()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitAmount  = 0;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNegativeBenefitAmount()
        {
            var request = GetValidRequest();
            request.Benefits[0].BenefitAmount = -1m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidNewEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentStartDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidNewEmploymentEndDate()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentEndDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNewEmploymentEndDateBeforeNewEmploymentStartDate()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentStartDate = new DateTime(2018, 01, 01);
            request.NewEmployments[0].NewEmploymentEndDate = new DateTime(2017, 01, 01);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNonZeroNewEmploymentWageAndZeroNewEmploymentWeeklyWage()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentWeeklyWage = 0;
            request.NewEmployments[0].NewEmploymentWage = 10m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNonZeroNewEmploymentWeeklyWageAndZeroNewEmploymentWage()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentWage = 0;
            request.NewEmployments[0].NewEmploymentWeeklyWage = 10m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNegativeNewEmploymentWage()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentWage = -1m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithZeroNewEmploymentWage()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentWage = 0m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNegativeNewEmploymentWeeklyWage()
        {
            var request = GetValidRequest();
            request.NewEmployments[0].NewEmploymentWeeklyWage = -1m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidWageIncreaseStartDate()
        {
            var request = GetValidRequest();
            request.WageIncreases[0].WageIncreaseStartDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidWageIncreaseEndDate()
        {
            var request = GetValidRequest();
            request.WageIncreases[0].WageIncreaseEndDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithWageIncreaseEndDateBeforeWageIncreaseStartDate()
        {
            var request = GetValidRequest();
            request.WageIncreases[0].WageIncreaseStartDate = new DateTime(2018, 01, 01);
            request.WageIncreases[0].WageIncreaseEndDate = new DateTime(2017, 01, 01);
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithZeroWageIncreaseAmount()
        {
            var request = GetValidRequest();
            request.WageIncreases[0].WageIncreaseAmount = 0;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNegativeWageIncreaseAmount()
        {
            var request = GetValidRequest();
            request.WageIncreases[0].WageIncreaseAmount = -1m;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidNotionalBenefitOverrideStartDate()
        {
            var request = GetValidRequest();
            request.NotionalBenefitOverrides[0].NotionalBenefitOverrideStartDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithInvalidNotionalBenefitOverrideEndDate()
        {
            var request = GetValidRequest();
            request.NotionalBenefitOverrides[0].NotionalBenefitOverrideEndDate = DateTime.MinValue;
            return request;
        }

        public static CompensatoryNoticePayCalculationRequestModel GetRequestWithNotionalBenefitOverrideEndDateBeforeNotionalBenefitOverrideStartDate()
        {
            var request = GetValidRequest();
            request.NotionalBenefitOverrides[0].NotionalBenefitOverrideStartDate = new DateTime(2018, 01, 01);
            request.NotionalBenefitOverrides[0].NotionalBenefitOverrideEndDate = new DateTime(2017, 01, 01);
            return request;
        }

    }
}

