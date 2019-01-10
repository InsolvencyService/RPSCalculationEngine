using System;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.RefundOfNotionalTax;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class RefundOfNotionalTaxCalculationRequestValidator : AbstractValidator<RefundOfNotionalTaxCalculationRequestModel>
    {
        public RefundOfNotionalTaxCalculationRequestValidator()
        {
            RuleFor(req => req.TaxableEarnings)
                .NotNull()
                .WithMessage($"Taxable earnings is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"Taxable earnings must be greater than or equal to 0");

            RuleFor(req => req.TaxAllowance)
                .NotNull()
                .WithMessage($"Tax Allowance is not provided");
                
            RuleFor(req => req.MaximumCNPEntitlement)
                .NotNull()
                .WithMessage($"Maximum CNP Entitlement is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"Maximum CNP Entitlement must be greater than or equal to 0");

            RuleFor(req => req.CnpPaid)
                .NotNull()
                .WithMessage($"CNP Paid is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"CNP Paid must be greater than or equal to 0");

            RuleFor(req => req.CnpTaxDeducted)
                .NotNull()
                .WithMessage($"CNP tax deducted is not provided")
                .GreaterThanOrEqualTo(0)
                .WithMessage($"CNP tax deducted must be greater than or equal to 0");
        }
    }
}