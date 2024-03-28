using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData
{
    public class HolidayTakenNotPaidTestDataHelper: IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 01, 10), new DateTime(2018, 01, 03), new DateTime(2017, 12, 12)
                    , new DateTime(2017, 12, 29), 306.85m, new List<string> { "1", "2", "3", "4", "6" }, 6, true),
                InputSource.Rp14a,
                8.3m,
                8.3m,
                new HolidayTakenNotPaidResponseDTO(InputSource.Rp14a, 489, weeklyResult: new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2017, 12, 16), 489m, 245.48M, 245.48M, true, 49.10m, 6.66m, 189.72m, 7, 4m, 489m, 245.48m, 245.48m, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2017, 12, 23), 489M, 42.96M, 42.96M,   true, 8.59m, 0m, 34.37M, 7, 0.7m, 489m, 42.96M, 42.96M, false),
                    new HolidayTakenNotPaidWeeklyResult(3, new DateTime(2017, 12, 23), 489M, 263.89M, 263.89M, true, 52.78m, 8.87m, 202.24m, 7, 4.3m, 489m, 263.89M, 263.89M, true),
                    new HolidayTakenNotPaidWeeklyResult(4, new DateTime(2017, 12, 30), 489M, 245.48M, 245.48M, true, 49.10m, 6.66m, 189.72m, 7, 4m, 489m, 245.48m, 245.48m, true)
                })
            };
            yield return new object[]
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 05, 15), new DateTime(2018, 04, 20), new DateTime(2017, 12, 20)
                    ,new DateTime(2017, 12, 22), 320m ,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                InputSource.Rp1,
                2.22m,
                2.22m,
                new HolidayTakenNotPaidResponseDTO(InputSource.Rp1, 508, weeklyResult:new List<HolidayTakenNotPaidWeeklyResult>()
                    {
                        new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2017, 12, 23), 508m, 49.92M, 49.92M, true, 9.98M, 0.0m, 39.94M, 7, 0.78m, 0m, 0m, 0m, false),
                        new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2017, 12, 23), 508m, 142.08M, 142.08M , true, 28.42M, 0.0m, 113.66M, 7, 2.22m, 0m, 0m, 0m, true)
                    }),
            };
            yield return new object[]
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 05, 15), new DateTime(2018, 04, 20), new DateTime(2017, 12, 20)
                    ,new DateTime(2017, 12, 22), 320m ,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                InputSource.Rp14a,
                2.22m,
                2.22m,
                new HolidayTakenNotPaidResponseDTO(InputSource.Rp14a, 508, weeklyResult:new List<HolidayTakenNotPaidWeeklyResult>())
            };
            yield return new object[]
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 8, 10), new DateTime(2018, 8, 03), new DateTime(2018, 7, 2)
                    , new DateTime(2018, 7, 4), 360m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                InputSource.Rp14a,
                0m,
                10m,
                new HolidayTakenNotPaidResponseDTO(InputSource.Rp14a, 508m, weeklyResult: new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 7, 7), 508m, 216M, 216M, true, 43.20m, 3.12m, 169.68m, 7, 3m, 508m, 216M, 216M, false),
                })
            };
            yield return new object[]
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 8, 10), new DateTime(2018, 8, 03), new DateTime(2018, 7, 2)
                    , new DateTime(2018, 7, 4), 360m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                InputSource.Rp14a,
                1m,
                10m,
                new HolidayTakenNotPaidResponseDTO(InputSource.Rp14a, 508m, weeklyResult: new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2018, 7, 7), 508m, 144M, 144M, true, 28.80m, 0m, 115.20m, 7, 2m, 508m, 144M, 144M, false),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2018, 7, 7), 508m, 72M, 72M, true, 14.40m, 0m, 57.60m, 7, 1m, 508m, 72M, 72M, true),
                })
            };
            yield return new object[]
            {
                new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 01, 10), new DateTime(2018, 01, 03), new DateTime(2017, 12, 12)
                    , new DateTime(2017, 12, 29), 306.85m, new List<string> { "1", "2", "3", "4", "6" }, 6, true),
                InputSource.Rp14a,
                0M,
                30m,
                new HolidayTakenNotPaidResponseDTO(InputSource.Rp14a, 489, weeklyResult: new List<HolidayTakenNotPaidWeeklyResult>()
                {
                    new HolidayTakenNotPaidWeeklyResult(1, new DateTime(2017, 12, 16), 489m, 245.48M, 245.48M, true, 49.10m, 6.66m, 189.72m, 7, 4m, 489m, 245.48m, 245.48m, true),
                    new HolidayTakenNotPaidWeeklyResult(2, new DateTime(2017, 12, 23), 489M, 306.85M, 306.85M, true, 61.37m, 14.02m, 231.46M, 7, 5m, 489m, 306.85M, 306.85M, true),
                    new HolidayTakenNotPaidWeeklyResult(3, new DateTime(2017, 12, 30), 489M, 245.48M, 245.48M, true, 49.10m, 6.66m, 189.72m, 7, 4m, 489m, 245.48m, 245.48m, true)
                })
            };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
