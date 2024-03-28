using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using System;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators.IrregularHourWorkerHPA
{
    public class IrregularHolidayPayAccruedCalculationRequestValidator : AbstractValidator<IrregularHolidayPayAccruedCalculationRequestModel>
    {
            public IrregularHolidayPayAccruedCalculationRequestValidator()
            {
                RuleFor(req => req.InsolvencyDate)
                    .GreaterThan(DateTime.MinValue)
                        .WithMessage($"Insolvency date is not provided or is not a valid date")
                    .GreaterThan(r => r.EmpStartDate)
                        .WithMessage($"Insolvency date must be greater than the Employee Start date");

                RuleFor(req => req.EmpStartDate.Date)
                    .GreaterThan(DateTime.MinValue)
                    .WithMessage($"Employee start date is not provided or is not a valid date");

                RuleFor(req => req.DismissalDate)
                    .GreaterThan(DateTime.MinValue)
                        .WithMessage($"Dismissal date is not provided or is not a valid date")
                    .GreaterThan(r => r.EmpStartDate)
                        .WithMessage($"Dismissal date must be greater than the Employee Start date");

                RuleFor(req => req.DismissalDate)
                    .GreaterThan(r => r.InsolvencyDate.AddYears(-1))
                        .WithMessage($"Dismissal date must be no more than a year prior to the insolvency date")
                        .When(r => r.InsolvencyDate != DateTime.MinValue && r.DismissalDate != DateTime.MinValue);

                RuleFor(req => req.ContractedHolEntitlement)
                    .NotNull()
                    .WithMessage($"Contracted Holiday Entitlement is not provided")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage($"Contracted Hoiday Entitlement must be greated than or equal to 0");

                RuleFor(req => req.HolidayYearStart)
                     .GreaterThan(DateTime.MinValue)
                     .WithMessage($"Holiday year start date is not provided or is not a valid date");

                RuleFor(req => req.HolidayYearStart.Date)
                    .LessThanOrEqualTo(r => r.InsolvencyDate.Date)
                    .WithMessage($"Holiday year start date cannot be after Insolvency date")
                    .LessThanOrEqualTo(r => r.DismissalDate.Date)
                    .WithMessage($"Holiday year start date cannot be after DismissalDate date");

                RuleFor(req => req.HolidayYearStart.Date)
                    .GreaterThan(r => GetMinFromDismissalDateAndInsolvencyDate(r).AddYears(-1))
                    .WithMessage($"Holiday year start date must be no more than a year prior to the dismissal date/insolvency date")
                    .When(r => r.DismissalDate != DateTime.MinValue && r.InsolvencyDate != DateTime.MinValue && r.HolidayYearStart != DateTime.MinValue);

                RuleFor(req => req.IsTaxable)
                    .NotNull()
                    .WithMessage($"IsTaxable value is not provided or is not valid ('true' or 'false')");

                RuleFor(req => req.PayDay)
                    .NotNull()
                    .WithMessage($"Pay day is not provided")
                    .Must(CommonValidation.BeValidPayDay)
                    .WithMessage($"Pay day is not valid correct values are [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");

                RuleFor(req => req.ShiftPattern)
                    .NotNull()
                    .WithMessage($"Shift pattern is not provided")
                    .Must(CommonValidation.BeValidShiftPattern)
                    .WithMessage($"Invalid shift pattern correct values are 0,1,2,3,4,5,6 Note: [0 = Sunday, 1 = Mon, 2 = Tues, 3 = Wed, 4 = Thurs, 5 = Fri, 6 = Sat]");

                RuleFor(req => req.WeeklyWage)
                    .NotNull()
                    .WithMessage($"Weekly wage is not provided")
                    .GreaterThan(0)
                    .WithMessage($"Weekly wage is invalid; value must not be 0 or negative");

                RuleFor(req => req.DaysCFwd)
                    .NotNull()
                    .WithMessage($"Days carried forward is not provided")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage($"Days carried forward must be 0 or greater");

                RuleFor(req => req.DaysTaken)
                    .NotNull()
                    .WithMessage($"Days taken is not provided")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage($"Days taken must be 0 or greater");

                RuleFor(req => req.IpConfirmedDays)
                    .NotNull()
                    .WithMessage($"Ip Confirmed Days is not provided")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage($"Ip Confirmed Days must be greater than or equal to 0")
                    .When(req => req.IrregularHoursWorker == false);

                ///Addition of validation for Irregular Hour Worker
                ///changes
                RuleFor(req => req.HolidayAccruedDaysCore)
                    .NotNull()
                    .WithMessage($"Holiday Accrued Days Core is not provided")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage($"Holiday Accrued Days Core must be greater than or equal to 0")
                    .When(req => req.IrregularHoursWorker == true);

                RuleFor(req => req.HolidaysCarriedOverCoreSource)
                .NotNull()
                .WithMessage($"Holidays Carried Over Core Source is not provided")
                .When(req => req.IrregularHoursWorker == true);
            }

            private DateTime GetMinFromDismissalDateAndInsolvencyDate(IrregularHolidayPayAccruedCalculationRequestModel req)
            {
                return req.DismissalDate.Date < req.InsolvencyDate.Date ? req.DismissalDate.Date : req.InsolvencyDate.Date;
            }
        }
    }
