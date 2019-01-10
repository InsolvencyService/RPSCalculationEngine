using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class NoticeWorkedNotPaidCalculationRequestValidator:AbstractValidator<NoticeWorkedNotPaidCalculationRequestModel>
    {
        public NoticeWorkedNotPaidCalculationRequestValidator()
        {
            RuleFor(req => req.InputSource)
               .Must(CommonValidation.BeValidInputSource)
               .WithMessage($"Input Source is not valid, correct values are 'rp1' or 'rp14a'");

            RuleFor(req => req.EmploymentStartDate.Date).Must(CommonValidation.BeValidDate)
               .WithMessage($"Employement Start Date is not provided or it is an invalid date");

            RuleFor(req => req.InsolvencyDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"Insolvency Date is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"Dismissal Date is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date.AddMonths(1))
                .WithMessage($"'Dismissal Date' must be at least 1 calendar month after the Employment Start Date");

            RuleFor(req => req.DateNoticeGiven.Date).Must(CommonValidation.BeValidDate)
               .WithMessage($"'Date Notice Given' is not provided or it is an invalid date");

            RuleFor(req => req.DateNoticeGiven.Date).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
                .WithMessage($"'Date Notice Given' can not be before the Employment Start Date");

            RuleFor(req => req.DateNoticeGiven.Date).LessThanOrEqualTo(model => model.DismissalDate.Date)
                .WithMessage($"'Date Notice Given' can not be after the Dismissal Date");

            RuleFor(req => req.UnpaidPeriodFrom.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"Unpaid Period From Date is not provided or it is invalid");

            RuleFor(req => req.UnpaidPeriodFrom.Date).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
               .WithMessage($"'Unpaid Period From Date' can not be before the Employment Start Date");

            //RuleFor(req => req.UnpaidPeriodFrom.Date).GreaterThan(model => model.DateNoticeGiven)
            //    .WithMessage($"'Unpaid Period From' Date can not be before or equal to the Date Notice Given");

            RuleFor(req => req.UnpaidPeriodFrom.Date).LessThanOrEqualTo(model => model.DismissalDate)
                .WithMessage($"'Unpaid Period From' Date can not be after the 'Dismissal Date'");

            RuleFor(req => req.UnpaidPeriodFrom.Date).LessThanOrEqualTo(model => model.UnpaidPeriodTo.Date)
                .WithMessage($"'Unpaid Period From' Date cannot be after the 'Unpaid Period To'");

            RuleFor(req => req.UnpaidPeriodTo.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"Unpaid Period To is not provided or it is invalid");

            //RuleFor(req => req.UnpaidPeriodTo.Date).LessThanOrEqualTo(model => model.DismissalDate.Date)
            //    .WithMessage($"'Unpaid Period To' Date cannot be after the Dismissal Date");

            RuleFor(req => req.WeeklyWage)
                .GreaterThanOrEqualTo(0)
                .WithMessage($"Weekly wage is invalid; value must not be 0 or negative");

            RuleFor(req => req.ShiftPattern).Must(CommonValidation.BeValidShiftPattern)
                .WithMessage(
                    $"Invalid shift pattern  correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat] ");

            RuleFor(req => req.PayDay).Must(CommonValidation.BeValidPayDay)
                .WithMessage(
                    $"Pay day is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat] ");

        }
    }
}
       