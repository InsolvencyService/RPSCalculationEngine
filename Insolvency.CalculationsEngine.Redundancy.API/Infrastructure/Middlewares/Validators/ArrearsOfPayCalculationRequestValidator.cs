using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class ArrearsOfPayCalculationRequestValidator : AbstractValidator<ArrearsOfPayCalculationRequestModel>
    {
        public ArrearsOfPayCalculationRequestValidator()
        {
            RuleFor(req => req.InputSource)
                .Must(CommonValidation.BeValidInputSource)
                .WithMessage($"'Input Source' is not valid, correct values are 'rp1' or 'rp14a'");

            RuleFor(req => req.InsolvencyDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Insolvency Date' is not provided or it is an invalid date");

            RuleFor(req => req.EmploymentStartDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Employment Start Date' is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Dismissal Date' is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
                .WithMessage($"'Dismissal Date' can not be before the 'Employment Start Date'");

            RuleFor(req => req.UnpaidPeriodFrom.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Unpaid Period From' Date is not provided or it is an invalid date");

            RuleFor(req => req.UnpaidPeriodFrom.Date).LessThanOrEqualTo(model => model.DismissalDate.Date)
                .WithMessage($"'Unpaid Period From' Date can not be after the 'Dismissal Date'");

            RuleFor(req => req.UnpaidPeriodTo.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Unpaid Period To' Date is not provided or it is an invalid date");

            RuleFor(req => req.DateNoticeGiven.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Date Notice Given' is not provided or it is an invalid date");

            RuleFor(req => req.UnpaidPeriodFrom.Date).LessThanOrEqualTo(model => model.InsolvencyDate.Date)
                .WithMessage($"'Unpaid Period From' Date can not be after the Insolvency Date");

            RuleFor(req => req.DateNoticeGiven.Date).LessThanOrEqualTo(model => model.DismissalDate.Date)
                .WithMessage($"'Date Notice Given' can not be after the 'Dismissal Date'");

            RuleFor(req => req.DateNoticeGiven.Date).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
               .WithMessage($"'Date Notice Given' can not be before 'Employment Start Date'");

            RuleFor(req => req.UnpaidPeriodFrom.Date).LessThanOrEqualTo(model => model.UnpaidPeriodTo.Date)
                .WithMessage($"'Unpaid Period From' Date can not be after the 'Unpaid Period To'");

            RuleFor(req => req.UnpaidPeriodTo.Date).LessThanOrEqualTo(model => model.UnpaidPeriodFrom.Date.AddMonths(7))
                .WithMessage($"'Unpaid Period To' Date can not be moe than 7 months after the 'Unpaid Period From'");

            RuleFor(req => req.WeeklyWage)
                .GreaterThan(0)
                .WithMessage($"'Weekly wage' is invalid; value must not be 0 or negative");

            RuleFor(req => req.ShiftPattern).Must(CommonValidation.BeValidShiftPattern)
                .WithMessage(
                    $"Invalid shift pattern  correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");

            RuleFor(req => req.PayDay).Must(CommonValidation.BeValidPayDay)
                .WithMessage(
                    $"'Pay day' is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");
        }
    }
}