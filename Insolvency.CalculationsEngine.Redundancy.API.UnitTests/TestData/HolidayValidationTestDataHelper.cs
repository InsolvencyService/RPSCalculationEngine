using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;

namespace Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData
{
    class HolidayValidationTestDataHelper : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "Bad payload" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = null
                },
                "Neither Hpa nor any Htnp data has been provided" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                },
                "Neither Hpa nor any Htnp data has been provided" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                    {
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 10), new DateTime(2018, 9, 18), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 18), new DateTime(2018, 9, 20), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                    }
                },
                "The same day appears in more than one Holiday Taken Not Paid period" };
            yield return new object[] {
                new HolidayCalculationRequestModel()
                {
                    Hpa = null,
                    Htnp = new List<HolidayTakenNotPaidCalculationRequestModel>()
                    {
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 10), new DateTime(2018, 9, 18), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                        new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a, new DateTime(2018, 10, 6), new DateTime(2018, 10, 6), new DateTime(2018, 9, 18), new DateTime(2018, 9, 20), 320m,  new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                    }
                },
                "The same day appears in more than one Holiday Taken Not Paid period" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}



