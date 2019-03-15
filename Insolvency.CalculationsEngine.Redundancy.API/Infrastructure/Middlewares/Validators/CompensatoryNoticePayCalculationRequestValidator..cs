using System;
using System.Collections.Generic;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class CompensatoryNoticePayCalculationRequestValidator : AbstractValidator<CompensatoryNoticePayCalculationRequestModel>
    {
        public CompensatoryNoticePayCalculationRequestValidator()
        {
            RuleFor(req => req.InsolvencyEmploymentStartDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Insolvency Employment Start Date' is not provided or it is an invalid date");

            RuleFor(req => req.InsolvencyEmploymentStartDate.Date).GreaterThanOrEqualTo(model => model.DateOfBirth.Date)
                .WithMessage($"'Insolvency Employment Start Date' cannot be before the Date of Birth");

            RuleFor(req => req.InsolvencyDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Insolvency Date' is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Dismissal Date' is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).GreaterThanOrEqualTo(model => model.InsolvencyEmploymentStartDate.Date.AddMonths(1))
                .WithMessage($"'Dismissal Date' must be at least 1 calendar month after the Insolvency Employment Start Date");

            RuleFor(req => req.DismissalDate.Date).GreaterThanOrEqualTo(model => model.DateNoticeGiven.Date)
                .WithMessage($"'Dismissal Date' cannot be before the Date Notice Given");

            RuleFor(req => req.DateNoticeGiven.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Date Notice Given' is not provided or it is an invalid date");

            RuleFor(req => req.DateNoticeGiven.Date).GreaterThanOrEqualTo(model => model.InsolvencyEmploymentStartDate.Date)
                .WithMessage($"'Date Notice Given' cannot be before the Insolvency Employment Start Date");

            RuleFor(req => req.WeeklyWage)
                .NotNull()
                .WithMessage($"'Weekly Wage' is not provided")
                .GreaterThan(0)
                .WithMessage($"'Weekly Wage' is invalid; value must not be 0 or negative");

            RuleFor(req => req.ShiftPattern)
               .NotNull()
               .WithMessage($"Shift pattern is not provided")
               .Must(CommonValidation.BeValidShiftPattern)
               .WithMessage($"Invalid shift pattern correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");

            RuleFor(req => req.IsTaxable)
                .NotNull()
                .WithMessage($"'Is Taxable' value is not provided or is not valid ('true' or 'false')");

            RuleFor(req => req.DateOfBirth.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Date Of Birth' is not provided or it is an invalid date");

            RuleFor(req => req.DeceasedDate)
                .Must(CommonValidation.BeValidDateIfPresent)
                .WithMessage($"'Deceased Date' is an invalid date");

            RuleFor(req => req.DeceasedDate)
                .GreaterThanOrEqualTo(model => model.DateOfBirth.Date)
                .When(HasDeceasedDate)
                .WithMessage($"'Decreased Date' can not be before the Date of Birth");

            RuleForEach(x => x.Benefits).SetValidator(new CompensatoryNoticePayBenefitValidator());
            RuleForEach(x => x.NewEmployments).SetValidator(new CompensatoryNoticePayNewEmploymentValidator());
            RuleForEach(x => x.WageIncreases).SetValidator(new CompensatoryNoticePayWageIncreaseValidator());
            RuleForEach(x => x.NotionalBenefitOverrides).SetValidator(new CompensatoryNoticePayNotionalBenefitOverrideValidator());
        }

        private bool HasDeceasedDate(CompensatoryNoticePayCalculationRequestModel model)
        {
            return model.DeceasedDate.HasValue && CommonValidation.BeValidDate(model.DeceasedDate.Value);
        }


        private class CompensatoryNoticePayBenefitValidator : AbstractValidator<CompensatoryNoticePayBenefit>
        {
            public CompensatoryNoticePayBenefitValidator()
            {
                RuleFor(req => req.BenefitStartDate.Date).Must(CommonValidation.BeValidDate)
                    .WithMessage($"'Benefit Start Date' is not provided or it is an invalid date");

                RuleFor(req => req.BenefitEndDate.Value.Date)
                    .GreaterThanOrEqualTo(model => model.BenefitStartDate.Date)
                    .WithMessage($"'Benefit End Date' cannot be before the Benefit Start Date")
                    .When(req => req.BenefitEndDate.HasValue);

                RuleFor(req => req.BenefitAmount)
                    .NotNull()
                    .WithMessage($"'Benefit Amount' is not provided")
                    .GreaterThan(0)
                    .WithMessage($"'Benefit Amount' is invalid; value must not be 0 or negative");

            }
        }

        private class CompensatoryNoticePayNewEmploymentValidator : AbstractValidator<CompensatoryNoticePayNewEmployment>
        {
            public CompensatoryNoticePayNewEmploymentValidator()
            {
                RuleFor(req => req.NewEmploymentStartDate.Date).Must(CommonValidation.BeValidDate)
                    .WithMessage($"'New Employment Start Date' is not provided or it is an invalid date");

                RuleFor(req => req.NewEmploymentEndDate.Value.Date)
                    .GreaterThanOrEqualTo(model => model.NewEmploymentStartDate.Date)
                    .WithMessage($"'New Employment End Date' cannot be before the New Employment Start Date")
                    .When(req => req.NewEmploymentEndDate.HasValue);

                RuleFor(req => req.NewEmploymentWage)
                    .NotNull()
                    .WithMessage($"'New Employment Wage' is not provided")
                    .GreaterThan(0)
                    .WithMessage($"'New Employment Wage' is invalid; value must not be negative or zero");                

                RuleFor(req => req.NewEmploymentWeeklyWage)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage($"'New Employment Weekly Wage' is invalid; value must not be negative")
                    .When(req => req.NewEmploymentWeeklyWage.HasValue);

                RuleFor(req => req.NewEmploymentWeeklyWage)
                    .Equal(0m)
                    .WithMessage($"'New Employment Weekly Wage' must be zero if New Employment Wage is zero")
                    .When(req => req.NewEmploymentWage == 0m && req.NewEmploymentWeeklyWage.HasValue);

            }
        }

        private class CompensatoryNoticePayWageIncreaseValidator : AbstractValidator<CompensatoryNoticePayWageIncrease>
        {
            public CompensatoryNoticePayWageIncreaseValidator()
            {
                RuleFor(req => req.WageIncreaseStartDate.Date).Must(CommonValidation.BeValidDate)
                    .WithMessage($"'Wage Increase Start Date' is not provided or it is an invalid date");

                RuleFor(req => req.WageIncreaseEndDate.Value.Date)
                    .GreaterThanOrEqualTo(model => model.WageIncreaseStartDate.Date)
                    .WithMessage($"'Wage Increase End Date' cannot be before the Wage Increase Start Date")
                    .When(req => req.WageIncreaseEndDate.HasValue);

                RuleFor(req => req.WageIncreaseAmount)
                    .NotNull()
                    .WithMessage($"'Wage Increase Amount' is not provided")
                    .GreaterThan(0)
                    .WithMessage($"'Wage Increase Amount' is invalid; value must not be 0 or negative");

            }
        }

        private class CompensatoryNoticePayNotionalBenefitOverrideValidator : AbstractValidator<CompensatoryNoticePayNotionalBenefitOverride>
        {
            public CompensatoryNoticePayNotionalBenefitOverrideValidator()
            {
                RuleFor(req => req.NotionalBenefitOverrideStartDate.Date).Must(CommonValidation.BeValidDate)
                    .WithMessage($"'Notional Benefit Override Start Date' is not provided or it is an invalid date");

                RuleFor(req => req.NotionalBenefitOverrideEndDate.Date)
                    .Must(CommonValidation.BeValidDate)
                    .WithMessage($"'Notional Benefit Override End Date' is not provided or it is an invalid date")
                    .GreaterThanOrEqualTo(model => model.NotionalBenefitOverrideStartDate.Date)
                    .WithMessage($"'Notional Benefit Override End Date' cannot be before the Notional Benefit Override Start Date");
            }
        }
    }
}
