using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Apportionment;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class ApportionmentCalculationRequestValidator : AbstractValidator<ApportionmentCalculationRequestModel>
    {
        public ApportionmentCalculationRequestValidator()
        {
            RuleFor(req => req.GrossEntitlement)
                .NotNull()
                .WithMessage($"'Gross Entitlement Amount' is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"'Gross Entitlement Amount' is invalid; value must not be negative");

            RuleFor(req => req.TotalClaimedInFourMonth)
                .NotNull()
                .WithMessage($"'Total Claimed Amount In Four Month' is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"'Total Claimed Amount In Four Month' is invalid; value must not be negative");
            RuleFor(req => req.GrossPaidInFourMonth)
                .NotNull()
                .WithMessage($"'Gross Paid In Four Months Amount' is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"'Gross Paid In Four Months Amount' is invalid; value must not be negative");
            RuleFor(req => req.TupeStatus)
                .NotNull()
                .WithMessage($"'Tupe Status' value is not provided or is not valid ('true' or 'false')");
        }
    }
}
