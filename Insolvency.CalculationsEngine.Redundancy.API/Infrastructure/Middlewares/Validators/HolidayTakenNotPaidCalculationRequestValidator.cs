using System;
using System.Collections.Generic;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class HolidayTakenNotPaidCalculationRequestValidator:AbstractValidator<HolidayTakenNotPaidCalculationRequestModel>
    {
        public HolidayTakenNotPaidCalculationRequestValidator()
        {
            RuleFor(req => req.InputSource)
               .Must(CommonValidation.BeValidInputSource)
               .WithMessage($"'Input Source' is not valid, correct values are 'rp1' or 'rp14a'");

            RuleFor(req => req.InsolvencyDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Insolvency Date' is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Dismissal Date' is not provided or it is an invalid date");

            RuleFor(req => req.UnpaidPeriodFrom.Date)
                .Must(CommonValidation.BeValidDate)
                .WithMessage($"'Unpaid Period From' Date is not provided or it is invalid")
                .LessThanOrEqualTo(model => model.DismissalDate.Date)
                .WithMessage($"'Unpaid Period From' Date can not be after the 'Dismissal Date'")
                .LessThanOrEqualTo(model => model.InsolvencyDate.Date)
                .WithMessage($"'Unpaid Period From' Date can not be after the 'Insolvency Date'");

            RuleFor(req => req.UnpaidPeriodTo.Date)
                .Must(CommonValidation.BeValidDate)
                .WithMessage($"'Unpaid Period To' Date is not provided or it is invalid")
                .LessThanOrEqualTo(model => model.DismissalDate.Date)
                .WithMessage($"'Unpaid Period To' Date can not be after the 'Dismissal Date'")
                .LessThanOrEqualTo(model => model.InsolvencyDate.Date)
                .WithMessage($"'Unpaid Period To' Date can not be after the 'Insolvency Date'");

            RuleFor(req => req.UnpaidPeriodFrom.Date).LessThanOrEqualTo(model => model.UnpaidPeriodTo.Date)
                .WithMessage($"'Unpaid Period From' Date cannot be after the 'Unpaid Period To'");

            RuleFor(req => req.WeeklyWage).GreaterThan(0)
                .WithMessage($"'Weekly Wage' is invalid; value must not be 0 or negative");

            RuleFor(req => req.PayDay)
                .NotNull()
                .WithMessage($"'Pay Day' is not provided")
                .Must(CommonValidation.BeValidPayDay)
                .WithMessage($"'Pay Day' is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");

            RuleFor(req => req.ShiftPattern)
                .NotNull()
                .WithMessage($"'Shift Pattern' is not provided")
                .Must(CommonValidation.BeValidShiftPattern)
                .WithMessage($"Invalid 'shift pattern' correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");
        }
    }
}
