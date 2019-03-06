using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insolvency.CalculationsEngine.Redundancy.Common.Extensions
{
    public static class Extensions
    {
        public static async Task<DateTime> GetAdjustedPeriodFromAsync(this DateTime unpaidPeriodFrom,
            DateTime insolvencyDate)
        {
            //Use this for any adjustment to unpaidPeriodFrom

            var adjustedPeriodFrom = unpaidPeriodFrom;
            return await Task.FromResult(adjustedPeriodFrom);
        }

        public static async Task<DateTime> GetAdjustedPeriodToAsync(this DateTime unpaidPeriodTo,
            DateTime insolvencyDate, DateTime dismissalDate)
        {
            var adjustedPeriodTo = unpaidPeriodTo;
            if (unpaidPeriodTo > insolvencyDate) adjustedPeriodTo = insolvencyDate;

            if (adjustedPeriodTo > dismissalDate) adjustedPeriodTo = dismissalDate;

            return await Task.FromResult(adjustedPeriodTo);
        }

        public static async Task<DayOfWeek> GetEnumValueAsync(this int? payDay)
        {
            var dayOfWeek = DayOfWeek.Sunday;
            foreach (var value in Enum.GetValues(typeof(DayOfWeek)))
                if ((int)value == payDay)
                {
                    dayOfWeek = (DayOfWeek)value;
                    break;
                }

            return await Task.FromResult(dayOfWeek);
        }

        public static async Task<DateTime> GetFourMonthDateAsync(this DateTime insolvencyDate)
        {
            return await Task.FromResult(insolvencyDate.AddMonths(-4));
        }

        public static async Task<List<DateTime>> GetWeekDatesAsync(this DateTime weekEndDate)
        {
            var dates = new List<DateTime>();
            //generate the back dates
            var weekStartDate = weekEndDate.AddDays(-6);
            for (var dt = weekStartDate; dt <= weekEndDate; dt = dt.AddDays(1)) dates.Add(dt);
            return await Task.FromResult(dates);
        }

        public static async Task<int> GetServiceYearsAsync(this DateTime firstDate, DateTime secondDate)
        {
            int yearsService = secondDate.Year - firstDate.Year;

            if (secondDate.Month < firstDate.Month || (secondDate.Month == firstDate.Month && secondDate.Day < firstDate.Day))
                yearsService--;

            return await Task.FromResult(Math.Max(0, yearsService));
        }

        public static async Task<DateTime> GetRelevantNoticeDate(this DateTime noticeDate, DateTime dismissalDate)
        {
            if (noticeDate <= dismissalDate)
            {
                return await Task.FromResult(noticeDate);
            }
            else
            {
                return await Task.FromResult(dismissalDate);
            }
        }



        public static async Task<DateTime> GetProjectedNoticeDate(this DateTime relNoticeDate, int noticeWeeks)
        {
            var projectedNoticeDate = relNoticeDate.AddDays(noticeWeeks * 7);
            return await Task.FromResult(projectedNoticeDate);
        }

        public static async Task<decimal> GetTaxDeducted(this decimal grossEntitlement, decimal taxRate, bool isTaxable)
        {
            if (isTaxable.Equals(false))
            {
                return await Task.FromResult(0);
            }
            var taxDeducted = grossEntitlement * taxRate;

            return await Task.FromResult(taxDeducted);
        }

        public static async Task<decimal> GetNIDeducted(this decimal grossEntitlement, decimal niThreshold, decimal niRate, bool isTaxable)
        {
            return await grossEntitlement.GetNIDeducted(niThreshold, niThreshold, niRate, niRate, isTaxable);
        }

        public static async Task<decimal> GetNIDeducted(this decimal grossEntitlement, decimal niThreshold, decimal niUpperThreshold, decimal niRate, decimal niUpperRate, bool isTaxable)
        {
            var niDeducted = 0m;

            if (isTaxable.Equals(false) || grossEntitlement <= niThreshold)
            {
                niDeducted = 0;
            }
            else if (grossEntitlement <= niUpperThreshold)
            {
                niDeducted = (grossEntitlement - niThreshold) * niRate;
            }
            else
            {
                niDeducted = (grossEntitlement - niUpperThreshold) * niUpperRate + (niUpperThreshold - niThreshold) * niRate;
            }

            return await Task.FromResult(niDeducted);
        }

        public static async Task<decimal> GetNetLiability(this decimal grossEntitlement, decimal taxDeducted, decimal niDeducted)
        {
            var netLiability = grossEntitlement - taxDeducted - niDeducted;

            return await Task.FromResult(netLiability);
        }

        public static async Task<decimal> GetStatutoryHolidayEntitlement(this decimal statHolEntitlement, List<string> shiftPattern)
        {
            var statutorytHolidayEntitlement = Math.Min(28m, (shiftPattern.Count) * 5.60m);
            return await Task.FromResult(statutorytHolidayEntitlement);
        }

        public static async Task<int> GetNumBusinessDaysInRange(this DateTime startDate, DateTime endDate, List<string> shiftPattern)
        {
            int numDays = 0;

            DateTime start = startDate.Date;
            DateTime end = endDate.Date;

            if (end >= start)
            {
                string weekDayNames = await shiftPattern.GetShiftDayNames();

                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    if (weekDayNames.Contains(date.DayOfWeek.ToString()))
                        numDays++;
                }
            }

            return await Task.FromResult(numDays);
        }

        public static async Task<int> GetNumDaysInIntersectionOfTwoRanges(this DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            // handle non-intersects
            if (endDate2.Date < startDate1.Date || startDate2.Date > endDate1.Date)
                return await Task.FromResult(0);

            var intersectStart = (startDate1.Date > startDate2.Date) ? startDate1.Date : startDate2.Date;
            var intersectEnd = (endDate1.Date < endDate2.Date) ? endDate1.Date : endDate2.Date;

            return await Task.FromResult((intersectEnd - intersectStart).Days + 1);
        }

        public static async Task<bool> DoRangesIntersect(this DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            var days = await startDate1.GetNumDaysInIntersectionOfTwoRanges(endDate1, startDate2, endDate2);
            return await Task.FromResult(days > 0);
        }

        public static async Task<List<DateTime>> GetDaysInRange(this DateTime startDate, DateTime endDate, DayOfWeek day)
        {
            var days = new List<DateTime>();

            if (endDate >= startDate)
            {
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    if (date.DayOfWeek == day)
                        days.Add(date.Date);
                }
            }

            return await Task.FromResult(days);
        }

        public static async Task<List<DateTime>> GetBusinessDaysInRange(this DateTime startDate, DateTime endDate, List<string> shiftPattern)
        {
            var days = new List<DateTime>();

            if (endDate >= startDate)
            {
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    if(await date.IsEmploymentDay(shiftPattern))
                        days.Add(date.Date);
                }
            }

            return await Task.FromResult(days);
        }

        public static async Task<bool> IsEmploymentDay(this DateTime date, List<string> shiftPattern)
        {
            string weekDayNames = await shiftPattern.GetShiftDayNames();
            bool result = weekDayNames.Contains(date.DayOfWeek.ToString());
            return await Task.FromResult(result);
        }

        public static async Task<DateTime> AddEmploymentDays(this DateTime date, int days, List<string> shiftPattern)
        {
            DateTime result = date.Date;

            while(days > 0)
            {
                result = result.AddDays(1);
                if (await result.IsEmploymentDay(shiftPattern))
                    days--;
            }

            return await Task.FromResult(result);
        }

        public static async Task<DateTime> GetPayDay(this DateTime date, DayOfWeek payDay)
        {
            var day = date;
            while (day.DayOfWeek != payDay)
                day = day.AddDays(1);

            return await Task.FromResult(day);
        }

        public static async Task<decimal> GetDailyAmount(this decimal amount, DateTime startDate, DateTime endDate)
        {
            int numDays = (endDate.Date - startDate.Date).Days + 1;

            return await Task.FromResult(amount / (decimal)numDays);
        }

        public static async Task<string> GetShiftDayNames(this List<string> shiftPattern)
        {
            string shiftDayNames = "";
            foreach (var day in shiftPattern)
            {
                switch (day)
                {
                    case "0": shiftDayNames += "Sunday"; break;
                    case "1": shiftDayNames += "Monday"; break;
                    case "2": shiftDayNames += "Tuesday"; break;
                    case "3": shiftDayNames += "Wednesday"; break;
                    case "4": shiftDayNames += "Thursday"; break;
                    case "5": shiftDayNames += "Friday"; break;
                    case "6": shiftDayNames += "Saturday"; break;
                }
            }

            return await Task.FromResult(shiftDayNames);
        }

        public static async Task<bool> ContainsDayWeek(this List<string> shiftPattern, DayOfWeek day)
        {
            var shift = ((int)day).ToString();
            bool contains = shiftPattern.Contains(shift);
            return await Task.FromResult(contains);
        }

        public static async Task<int> GetTaxYear(this DateTime date)
        {
            int taxYearStartYear = 0;

            if (date.Date < new DateTime(date.Date.Year, 4, 6))
                taxYearStartYear = date.Date.Year - 1;
            else
                taxYearStartYear = date.Date.Year;

            return await Task.FromResult(taxYearStartYear);
        }

        public static async Task<decimal> GetEarnings(this decimal weeklyWage, DateTime startDate, DateTime endDate)
        {
            int numDaysWorked = (endDate.Date - startDate.Date).Days + 1;

            decimal wholeWeeks = (numDaysWorked / 7) * weeklyWage;
            decimal partialWeek = (numDaysWorked % 7) * weeklyWage / 7m;

            return await Task.FromResult(wholeWeeks + partialWeek);
        }

        public static async Task<int> GetAge(this DateTime dob, DateTime now)
        {
            int age = now.Year - dob.Year;

            if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
                age--;

            return await Task.FromResult(age);
        }
    }
}