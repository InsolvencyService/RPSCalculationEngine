using System;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Insolvency.CalculationsEngine.Redundancy.Common.Exceptions;
using Insolvency.CalculationsEngine.Redundancy.Common.UnitTests.TestData;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.Common.UnitTests.ConfigValueLookupHelperTests
{
    public class ConfigValueLookupHelperTests
    {
        private readonly IOptions<ConfigLookupRoot> _configOptions;
        private readonly ConfigLookupRoot _confLookupRoot;

        public ConfigValueLookupHelperTests()
        {
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _confLookupRoot = testConfigLookupDataHelper.PopulateConfigLookupRoot();
            _configOptions = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.StatutoryMaxUnitTestData))]
        public void GetStatutaryMax_Returns_CorrectStatMax_ForGiven_InsolvencyDate(DateTime insolvencyDate,
            decimal expectedStatutoryMax)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetStatutoryMax(_configOptions, insolvencyDate);
            //Assert
            result.Should().Be(expectedStatutoryMax);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetStatutaryMax_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(2010, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetStatutoryMax(_configOptions, date));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.BenefitsWaitingDaysUnitTestData))]
        public void GetBenefitsWaitingDays_Returns_CorrectBenefitsWaitingDays_ForGiven_benefitsStartDate(
            DateTime benefitsStartDate, decimal expectedBenefitsWaitingDays)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetBenefitsWaitingDays(_configOptions, benefitsStartDate);
            //Assert
            result.Should().Be(expectedBenefitsWaitingDays);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetBenefitsWaitingDays_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(2010, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetBenefitsWaitingDays(_configOptions, date));
        }


        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.TaxRateUnitTestData))]
        public void GetTaxRate_Returns_CorrectTaxRate_ForGiven_date(
            DateTime someDate, decimal expectedTaxRate)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetTaxRate(_configOptions, someDate);
            //Assert
            result.Should().Be(expectedTaxRate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetTaxRate_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(1899, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetTaxRate(_configOptions, date));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.NIRateUnitTestData))]
        public void GetNIRate_Returns_CorrectNIRate_ForGiven_date(
            DateTime someDate, decimal expectedNIRate)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetNIRate(_configOptions, someDate);
            //Assert
            result.Should().Be(expectedNIRate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetNIRate_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(1899, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetNIRate(_configOptions, date));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.NIUpperRateUnitTestData))]
        public void GetNIUpperRate_Returns_CorrectNIRate_ForGiven_date(
            DateTime someDate, decimal expectedNIRate)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetNIUpperRate(_configOptions, someDate);
            //Assert
            result.Should().Be(expectedNIRate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetNIUpperRate_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(1899, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetNIUpperRate(_configOptions, date));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.NIThresholdUnitTestData))]
        public void GetNIThreshold_Returns_CorrectNIThreshold_ForGiven_date(
            DateTime someDate, decimal expectedNIThresholdRate)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetNIThreshold(_configOptions, someDate);
            //Assert
            result.Should().Be(expectedNIThresholdRate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetNIThreshold_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(1899, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetNIThreshold(_configOptions, date));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.NIUpperThresholdUnitTestData))]
        public void GetNIUpperThreshold_Returns_CorrectNIThreshold_ForGiven_date(
            DateTime someDate, decimal expectedNIThresholdRate)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetNIUpperThreshold(_configOptions, someDate);
            //Assert
            result.Should().Be(expectedNIThresholdRate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetNIUpperThreshold_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(1899, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetNIUpperThreshold(_configOptions, date));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.PreferentialLimitUnitTestData))]
        public void GetPreferentialLimit_Returns_CorrectPreferentialLimit_ForGiven_date(
            DateTime someDate, decimal expectedPreferntialLimit)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetPreferentialLimit(_configOptions, someDate);
            //Assert
            result.Should().Be(expectedPreferntialLimit);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetPreferentialLimit_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(1899, 1, 1);

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetPreferentialLimit(_configOptions, date));
        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(TestDataHelper.NotionalBenefitsWeeklyRateUnitTestData))]
        public void GetNotionalBenefitsWeeklyRate_Returns_CorrectNotionalBenefitsWeeklyRate_ForGivenDateAndAge(
            DateTime someDate, int age, decimal expectedWeeklyRate)
        {
            //Arrange 
            //Act
            var result = ConfigValueLookupHelper.GetNotionalBenefitsWeeklyRate(_configOptions, someDate, age);
            //Assert
            result.Should().Be(expectedWeeklyRate);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void GetNotionalBenefitsWeeklyRate_ThrowsException_WhenNoConfigForDate()
        {
            var date = new DateTime(1899, 1, 1);
            var age = 26;

            //Act
            Exception ex = Assert.Throws<MissingConfigurationException>(() => ConfigValueLookupHelper.GetNotionalBenefitsWeeklyRate(_configOptions, date, age));
        }
    }
}