using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.APPA;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;
using System.Collections;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class APPACalculationRequestValidator : AbstractValidator<APPACalculationRequestModel>
    {
        public APPACalculationRequestValidator()
        {
            RuleForEach(req => req.Ap)
                .SetValidator(new ArrearsOfPayCalculationRequestValidator());

            RuleFor(req => req.Pa)
               .SetValidator(new ProtectiveAwardCalculationRequestValidator())
               .When(req => req.Pa != null);

            RuleFor(req => req.Ap)
               .NotNull()
               .WithMessage($"Neither Arrears Of Pay nor Protective Award data has been provided")
               .NotEmpty()
               .WithMessage($"Neither Arrears Of Pay nor Protective Award data has been provided")
               .When(req => req.Pa == null);

            RuleFor(req => req.Ap)
                .Must(NoOverlappingPeriodsForRp1OrRp14a)
                .WithMessage($"The same day appears in more than one Arrears Of Pay period")
                .When(req => req.Ap != null);
        }

        private bool NoOverlappingPeriodsForRp1OrRp14a(List<ArrearsOfPayCalculationRequestModel> apList)
        {
            return NoOverlappingPeriods(apList, InputSource.Rp1) &&
                    NoOverlappingPeriods(apList, InputSource.Rp14a);
        }

        private bool NoOverlappingPeriods(List<ArrearsOfPayCalculationRequestModel> fullList, string inputSource)
        {
            var apList = fullList.Where(r => r.InputSource == inputSource).ToArray();

            // test Rp1/Rp14a seprately
            for (int i = 0; i < apList.Count(); i++)
            {
                for (int j = 0; j < apList.Count(); j++)
                {
                    if (i != j && apList[i].UnpaidPeriodFrom.Date.DoRangesIntersect(
                            apList[i].UnpaidPeriodTo.Date,
                            apList[j].UnpaidPeriodFrom.Date,
                            apList[j].UnpaidPeriodTo.Date).Result)
                        return false;
                }
            }
            return true;
        }
    }
}