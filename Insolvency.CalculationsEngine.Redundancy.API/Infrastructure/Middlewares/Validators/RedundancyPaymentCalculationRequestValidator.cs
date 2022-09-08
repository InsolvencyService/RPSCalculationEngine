using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.RedundancyPaymentCalculation.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RedundancyPayment;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class RedundancyPaymentCalculationRequestValidator : AbstractValidator<RedundancyPaymentCalculationRequestModel>
    {
        public RedundancyPaymentCalculationRequestValidator()
        {
            RuleFor(req => req.EmploymentStartDate.Date)
                .NotNull()
                .WithMessage($"Employment start date is not provided")
                .GreaterThan(DateTime.MinValue)
                .WithMessage($"Employment start date is not a valid date");

            RuleFor(req => req.DateNoticeGiven.Date)
                .NotNull()
                .WithMessage($"Notice given date is not provided")
                .GreaterThan(DateTime.MinValue)
                .WithMessage($"Notice given date is not a valid date");

            RuleFor(req => req.DismissalDate.Date)
                 .NotNull()
                .WithMessage($"Dismissal date is not provided")
                .GreaterThan(DateTime.MinValue)
                .WithMessage($"Dismissal date is not a valid date");

            RuleFor(req => req.ClaimReceiptDate.Date)
                .Must(CommonValidation.BeValidDate)
                .WithMessage($"Claim Receipt Date is not provided or it is an invalid date");

            RuleFor(req => req)
                .Must(ClaimReceitDateLessThatDismissalDatePlus6Months)
                .WithName("ClaimReceiptDate")
                .WithMessage($"Claim Receipt Date must be within 6 months of the dismissal date");

            RuleFor(req => req.DateOfBirth.Date)
                .NotNull()
                .WithMessage($"Date of birth is not provided")
                .GreaterThan(DateTime.MinValue)
                .WithMessage($"Date of birth is not a valid date");

            RuleFor(req => req.WeeklyWage)
                .GreaterThan(0)
                .WithMessage($"Weekly wage is invalid; value must be greater than zero");

            RuleFor(req => req.EmployerPartPayment)
                 .GreaterThanOrEqualTo(0)
                 .WithMessage("Employer part payment is invalid; value must be greater than or equal to zero");

            RuleFor(req => req.EmploymentBreaks)
                 .GreaterThanOrEqualTo(0)
                 .WithMessage("Number of days on employment break is invalid; value must be greater than or equal to zero");

            RuleFor(Req => Req)
                .MustAsync(TotalYearsOfServiceGreaterThan2)
                .WithName("EmploymentStartDate")
                .WithMessage("Years of service cannot be less than 2 years")
                .When(x => x.EmploymentStartDate != DateTime.MinValue &&
                            x.DismissalDate != DateTime.MinValue &&
                            x.DateNoticeGiven != DateTime.MinValue);
        }


        private async Task<bool> TotalYearsOfServiceGreaterThan2(RedundancyPaymentCalculationRequestModel data, CancellationToken token)
        {
            var adjStartDate = await data.EmploymentStartDate.GetAdjustedEmploymentStartDate(data.EmploymentBreaks);
            var relevantNoticeDate = await data.DateNoticeGiven.GetRelevantNoticeDate(data.DismissalDate);
            var noticeEntitlementWeeks = await adjStartDate.GetNoticeEntitlementWeeks(relevantNoticeDate);

            var YearsOfService = await adjStartDate.GetServiceYearsAsync(data.DismissalDate);

            var projectedNoticeDate = await relevantNoticeDate.GetProjectedNoticeDate(noticeEntitlementWeeks);

            var relevantDismissalDate = await data.DismissalDate.GetRelevantDismissalDate(projectedNoticeDate);

            var totalYearsOfService = await adjStartDate.GetServiceYearsAsync(relevantDismissalDate);
            var totalMaxYearsOfService = Math.Max(YearsOfService, totalYearsOfService);

            return await Task.FromResult(totalMaxYearsOfService >= 2);
        }

        private bool ClaimReceitDateLessThatDismissalDatePlus6Months(RedundancyPaymentCalculationRequestModel data)
       {
            return data.ClaimReceiptDate.Date <= data.DismissalDate.Date.AddMonths(6);
        }
    }
}