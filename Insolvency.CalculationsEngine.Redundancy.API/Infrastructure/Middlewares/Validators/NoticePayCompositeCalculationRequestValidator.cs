using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.Common.Extensions;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    public class NoticePayCompositeCalculationRequestValidator : AbstractValidator<NoticePayCompositeCalculationRequestModel>
    {
        public NoticePayCompositeCalculationRequestValidator()
        {
            RuleFor(req => req.Cnp)
                .SetValidator(new CompensatoryNoticePayCalculationRequestValidator())
                .When(req => req.Cnp != null);

            RuleForEach(req => req.Nwnp)
                .SetValidator(new NoticeWorkedNotPaidCalculationRequestValidator());

            RuleFor(req => req.Nwnp)
               .NotNull()
               .WithMessage($"Neither NWNP nor CNP data has been provided")
               .NotEmpty()
               .WithMessage($"Neither NWNP nor CNP data has been provided")
               .When(req => req.Cnp == null);

            RuleFor(req => req.Nwnp)
             .Must(NoOverlappingPeriodsForRp1OrRp14a)
             .WithMessage($"The same day appears in more than one Notice Worked Not Paid period")
             .When(req => req.Nwnp != null);

            RuleFor(req => req)
               .Must(RP1DataPresent)
               .WithMessage($"No Notice Worked Not Paid RP1 data has been not provided")
               .When(req => req.Nwnp != null);
        }

        private bool NoOverlappingPeriodsForRp1OrRp14a(List<NoticeWorkedNotPaidCalculationRequestModel> list)
        {
            return NoOverlappingPeriods(list, InputSource.Rp1) &&
                    NoOverlappingPeriods(list, InputSource.Rp14a);
        }

        private bool NoOverlappingPeriods(List<NoticeWorkedNotPaidCalculationRequestModel> fullList, string inputSource)
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

        private bool RP1DataPresent(NoticePayCompositeCalculationRequestModel data)
        {
            return data.Nwnp.Count(x => x.InputSource == InputSource.Rp14a) == 0 ||
                data.Nwnp.Count(x => x.InputSource == InputSource.Rp1) > 0;
        }
    }
}