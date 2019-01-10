using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.Calculations.Holiday.Extensions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Holiday;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ExtensionsTests
{
    public class HolidayTakenNotPaidExtensionsTests
    {
        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetHTNPDays_Returns_TotalBusinessDaysInRp1Periods()
        {
            // Arrange
            var list = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                 new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1,
                    new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), 
                    new DateTime(2018, 09, 20), new DateTime(2018, 09, 29),
                    320m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                 new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a,
                    new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), 
                    new DateTime(2018, 09, 18), new DateTime(2018, 09, 26),
                    320m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                 new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1,
                    new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), 
                    new DateTime(2018, 8, 15), new DateTime(2018, 8, 15),
                    320m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
            };

            // Act
            var result = await list.GetHTNPDays(InputSource.Rp1, new DateTime(2018, 08, 14), new DateTime(2018, 09, 24));

            // Assert
            result.Count.Should().Be(4);
            result[0].Should().Be(new DateTime(2018, 9, 24));
            result[1].Should().Be(new DateTime(2018, 9, 21));
            result[2].Should().Be(new DateTime(2018, 9, 20));
            result[3].Should().Be(new DateTime(2018, 8, 15));
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public async Task GetHTNPDays_Returns_TotalBusinessDaysInRp14aPeriods()
        {
            // Arrange
            var list = new List<HolidayTakenNotPaidCalculationRequestModel>()
            {
                 new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1,
                    new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), 
                    new DateTime(2018, 09, 20), new DateTime(2018, 09, 29),
                    320m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                 new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp14a,
                    new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), 
                    new DateTime(2018, 09, 18), new DateTime(2018, 09, 26),
                    320m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
                 new HolidayTakenNotPaidCalculationRequestModel(InputSource.Rp1,
                    new DateTime(2018, 10, 1), new DateTime(2018, 10, 1), 
                    new DateTime(2018, 8, 15), new DateTime(2018, 8, 15),
                    320m, new List<string> { "1", "2", "3", "4", "5" }, 6, true),
            };

            // Act
            var result = await list.GetHTNPDays(InputSource.Rp14a, new DateTime(2018, 08, 14), new DateTime(2018, 09, 24));

            // Assert
            result.Count.Should().Be(5);
            result[0].Should().Be(new DateTime(2018, 9, 24));
            result[1].Should().Be(new DateTime(2018, 9, 21));
            result[2].Should().Be(new DateTime(2018, 9, 20));
            result[3].Should().Be(new DateTime(2018, 9, 19));
            result[4].Should().Be(new DateTime(2018, 9, 18));
        }
    }
}
