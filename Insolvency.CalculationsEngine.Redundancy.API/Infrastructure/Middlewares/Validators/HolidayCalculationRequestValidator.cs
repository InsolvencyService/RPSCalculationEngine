using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class HolidayCalculationRequestValidator : AbstractValidator<HolidayCalculationRequestModel>
    {
        public HolidayCalculationRequestValidator()
        {
            RuleFor(req => req.Hpa)
                .SetValidator(new HolidayPayAccruedCalculationRequestValidator())
                .When(req => req.Hpa != null);

            RuleForEach(req => req.Htnp)
                .SetValidator(new HolidayTakenNotPaidCalculationRequestValidator());

            RuleFor(req => req.Htnp)
               .NotNull()
               .WithMessage($"Neither Hpa nor any Htnp data has been provided")
               .NotEmpty()
               .WithMessage($"Neither Hpa nor any Htnp data has been provided")
               .When(req => req.Hpa == null);

            RuleFor(req => req.Htnp)
               .Must(NoOverlappingPeriodsForRp1OrRp14a)
               .WithMessage($"The same day appears in more than one Holiday Taken Not Paid period")
               .When(req => req.Htnp != null);
        }

        private bool NoOverlappingPeriodsForRp1OrRp14a(List<HolidayTakenNotPaidCalculationRequestModel> list)
        {
            return NoOverlappingPeriods(list, InputSource.Rp1) &&
                    NoOverlappingPeriods(list, InputSource.Rp14a);
        }

        private bool NoOverlappingPeriods(List<HolidayTakenNotPaidCalculationRequestModel> fullList, string inputSource)
        {
            var list = fullList.Where(r => r.InputSource == inputSource).ToArray();

            // test Rp1/Rp14a seprately
            for (int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list.Count(); j++)
                {
                    if (i != j && list[i].UnpaidPeriodFrom.Date.DoRangesIntersect(
                            list[i].UnpaidPeriodTo.Date,
                            list[j].UnpaidPeriodFrom.Date,
                            list[j].UnpaidPeriodTo.Date).Result)
                        return false;
                }
            }
            return true;
        }
    }
}