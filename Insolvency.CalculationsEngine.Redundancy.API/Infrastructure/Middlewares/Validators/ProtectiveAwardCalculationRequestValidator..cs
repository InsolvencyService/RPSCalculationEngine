using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using System;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class ProtectiveAwardCalculationRequestValidator : AbstractValidator<ProtectiveAwardCalculationRequestModel>
    {
        public ProtectiveAwardCalculationRequestValidator()
        {

            RuleFor(req => req.InsolvencyDate.Date)
                .Must(CommonValidation.BeValidDate)
                .WithMessage($"'Insolvency Date' is not provided or it is an invalid date")
                .Must(CommonValidation.NotBeInTheFuture)
                .WithMessage($"'Insolvency Date' can not be in the future")
                .GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
                .WithMessage($"'Insolvency Date' can not be before the Employment Start Date");

            RuleFor(req => req.EmploymentStartDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Employment Start Date' is not provided or it is an invalid date")
                .Must(CommonValidation.NotBeInTheFuture)
                .WithMessage($"'Employment Start Date' can not be in the future");

            RuleFor(req => req.DismissalDate.Date)
                .Must(CommonValidation.BeValidDate)
                    .WithMessage($"'Dismissal Date' is not provided or it is an invalid date")
                .Must(CommonValidation.NotBeInTheFuture)
                    .WithMessage($"'Dismissal Date' can not be in the future")
                .GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
                    .WithMessage($"'Dismissal Date' can not be before the Employment Start Date");

            RuleFor(req => req.TribunalAwardDate.Date)
                .Must(CommonValidation.BeValidDate)
                .WithMessage($"'Tribunal Award Date' is not provided or it is an invalid date")
                .Must(CommonValidation.NotBeInTheFuture)
                .WithMessage($"'Tribunal Award Date' can not be in the future");

            RuleFor(req => req.ProtectiveAwardStartDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Protective Award Start Date' is not provided or it is an invalid date")
                .Must(CommonValidation.NotBeInTheFuture)
                .WithMessage($"'Protective Award Start Date' can not be in the future");

            RuleFor(req => req.ProtectiveAwardDays)
                .NotNull()
                .WithMessage($"'Protective Award Days' is not provided or it is not in the format of an integer")
                .GreaterThanOrEqualTo(1)
                .WithMessage($"'Protective Award Days' is invalid; value must be 1 or greater")
                .LessThanOrEqualTo(90)
                .WithMessage($"'Protective Award Days' is invalid; value must be 90 or less");

            RuleFor(req => req.PayDay)
                .NotNull()
                .WithMessage($"'Pay day' is not provided or it is not in the format of an integer")
                .Must(CommonValidation.BeValidPayDay)
                .WithMessage(
                    $"'Pay day' is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");

            RuleFor(req => req.WeeklyWage)
                .NotNull()
                .WithMessage($"'Weekly Wage' is not provided")
                .GreaterThan(0)
                .WithMessage($"'Weekly Wage' is invalid; value must not be 0 or negative");

            RuleFor(req => req.ShiftPattern)
                .NotNull()
                .WithMessage($"Shift pattern is not provided")
                .Must(CommonValidation.BeValidShiftPattern)
                .WithMessage(
                    $"Invalid shift pattern correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");

            RuleFor(req => req.paBenefitStartDate.Date)
                    .Must(CommonValidation.NotBeInTheFuture)
                    .WithMessage($"'Benefit Start Date' can not be in the future");

            RuleFor(req => req.paBenefitAmount)
                    .NotNull()
                    .WithMessage($"'Benefit Amount' is not provided")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage($"'Benefit Amount' is invalid; value must not be negative");

            RuleFor(x => x)
                .Must(BenefitStartDateBeAfterDismissalDate)
                .WithMessage("'Benefit Start Date' can not be before Date of Dismissal");
        }

        private bool BenefitStartDateBeAfterDismissalDate(ProtectiveAwardCalculationRequestModel model)
        {
            if (model.paBenefitAmount > 0)
            {
                    if (model.paBenefitStartDate.Date < model.DismissalDate)
                        return false;
            }
            return true;
        }

        
    }
}