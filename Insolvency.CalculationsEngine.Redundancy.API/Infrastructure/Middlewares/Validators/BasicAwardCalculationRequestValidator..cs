using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.BasicAward;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class BasicAwardCalculationRequestValidator : AbstractValidator<BasicAwardCalculationRequestModel>
    {
        public BasicAwardCalculationRequestValidator()
        {
            RuleFor(req => req.BasicAwardAmount)
                .NotNull()
                .WithMessage($"'Basic Award Amount' is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"'Basic Award Amount' is invalid; value must not be negative");

            RuleFor(req => req.IsTaxable)
                .NotNull()
                .WithMessage($"'Is Taxable' value is not provided or is not valid ('true' or 'false')");
        }
    }
}
