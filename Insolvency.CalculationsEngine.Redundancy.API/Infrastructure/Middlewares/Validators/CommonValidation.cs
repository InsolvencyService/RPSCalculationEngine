using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators
{
    internal static class CommonValidation
    {
        internal static bool BeValidDate(DateTime date)
        {
            return !(date <= DateTime.MinValue);
        }

        internal static bool BeValidDate(DateTime? date)
        {
            return date.HasValue && BeValidDate(date.Value);
        }

        internal static bool NotBeInTheFuture(DateTime date)
        {
            return date.Date <= DateTime.Today;
        }

        internal static bool BeValidDateIfPresent(DateTime? date)
        {
            return !date.HasValue || BeValidDate(date.Value);
        }

        internal static bool BeValidPayDay(int payDay)
        {
            return payDay >= 0 && payDay <= 6;
        }

        internal static bool BeValidPayDay(int? payDay)
        {
            return BeValidPayDay(payDay ?? -1);
        }

        internal static bool BeValidShiftPattern(List<string> shiftDaysList)
        {
            return shiftDaysList != null
                && shiftDaysList.Count >= 1 && shiftDaysList.Count <= 7
                && TryParseShiftDays(shiftDaysList);
        }

        internal static bool BeValidInputSource(string inputSource)
        {
            return inputSource == InputSource.Rp1 || inputSource == InputSource.Rp14a;
        }

        private static bool TryParseShiftDays(List<string> shiftDaysList)
        {
            foreach (var day in shiftDaysList)
            {
                var parseAttempt = int.TryParse(day, out int parseResult);
                if (!parseAttempt) return false;
                if (parseResult < 0 || parseResult > 6) return false;
            }

            return true;
        }
    }
}
