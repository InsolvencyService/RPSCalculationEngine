using System;
using System.Collections.Generic;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.ProjectedNoticeDate;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class ProjectedNoticeDateCalculationRequestValidator : AbstractValidator<ProjectedNoticeDateCalculationRequestModel>
    {
        public ProjectedNoticeDateCalculationRequestValidator()
        {
            RuleFor(req => req.EmploymentStartDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Employment Start Date' is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Dismissal Date' is not provided or it is an invalid date");

            RuleFor(req => req.DateNoticeGiven.Date).Must(CommonValidation.BeValidDate)
                .WithMessage($"'Date Notice Given is not provided or it is an invalid date");

            RuleFor(req => req.DismissalDate.Date).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
                .WithMessage($"'Dismissal Date' can not be before the Employment Start Date");

            RuleFor(req => req.DateNoticeGiven.Date).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date)
                .WithMessage($"'Dismissal Date' can not be before the Employment Start Date");

            RuleFor(req => req.DismissalDate).GreaterThanOrEqualTo(model => model.EmploymentStartDate.Date.AddMonths(1))
                .WithMessage($"'Dismissal Date' must be at least 1 month later than 'Employment Start Date'");
        }
    }
}
