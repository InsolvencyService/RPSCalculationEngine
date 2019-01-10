using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Common;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.TestData;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;

namespace Insolvency.CalculationsEngine.Redundancy.BL.UnitTests.ServicesTests
{
    public class NoticeCalculationsServiceTests
    {
        private readonly NoticeCalculationService _noticeCalculationsServiceTests;
        private readonly INoticeWorkedNotPaidCalculationService _nwnpService;
        private readonly ICompensatoryNoticePayCalculationService _cnpService;
        private readonly IOptions<ConfigLookupRoot> _options;
        public NoticeCalculationsServiceTests()
        {
            _nwnpService = new NoticeWorkedNotPaidCalculationService();
            _cnpService = new CompensatoryNoticePayCalculationService();
            _noticeCalculationsServiceTests  = new NoticeCalculationService(_nwnpService, _cnpService);
            var testConfigLookupDataHelper = new TestConfigLookupDataHelper();
            _options = Options.Create(testConfigLookupDataHelper.PopulateConfigLookupRoot());

        }

        [Theory]
        [Trait("Category", "UnitTest")]
        [ClassData(typeof(NoticeTestDataHelper.NoticeRequestData))]
        public async Task PerformNoticeCalculationTests(
            NoticePayCompositeCalculationRequestModel req1, NoticePayCompositeCalculationResponseDTO res1, 
            NoticePayCompositeCalculationRequestModel req2, NoticePayCompositeCalculationResponseDTO res2, 
            NoticePayCompositeCalculationRequestModel req3, NoticePayCompositeCalculationResponseDTO res3)
        {
            //arrange
            var inputDataList = new List<NoticePayCompositeCalculationRequestModel>()
            {
                req1, req2, req3
            };
            var expectedOutputList = new List<NoticePayCompositeCalculationResponseDTO>()
            {
                res1, res2, res3
            };

            //add selected weeks to the expectedOutputList
            expectedOutputList[0].Cnp.WeeklyResults.ToList().ForEach(x => x.IsSelected = true);
            expectedOutputList[1].Nwnp.rp14aResults.WeeklyResult.ToList().ForEach(x => x.IsSelected = true);
            expectedOutputList[2].Nwnp.rp14aResults.WeeklyResult.ToList().ForEach(x => x.IsSelected = true);
            expectedOutputList[2].Cnp.WeeklyResults.ForEach(x => x.IsSelected = true);
            expectedOutputList[2].Nwnp.rp14aResults.WeeklyResult.ForEach(x => x.IsSelected = true);


            var actualOutputList = new List<NoticePayCompositeCalculationResponseDTO>();
            
            int i = 0;
            foreach (var data in inputDataList)
            {
                //act
                var res = await _noticeCalculationsServiceTests.PerformNoticePayCompositeCalculationAsync(data, _options);
                //assert for empty CNP/NWNP in output & selectedWeeks
                if (data.Cnp != null)
                {
                    Assert.True(res.Cnp.WeeklyResults.Count() > 0);
                    res.Cnp.WeeklyResults.Where(x => x.IsSelected == true).Count()
                        .Should().Be(expectedOutputList[i].Cnp.WeeklyResults.Where(x => x.IsSelected == true).Count());
                }
                if(data.Nwnp != null && data.Nwnp.Where(x => x.InputSource == InputSource.Rp1).Any())
                {
                    Assert.True(res.Nwnp.rp1Results.WeeklyResult.Count() > 0);
                    res.Nwnp.rp1Results.WeeklyResult.Where(x => x.IsSelected == true).Count()
                        .Should().Be(actualOutputList[i].Nwnp.rp1Results.WeeklyResult.Where(x => x.IsSelected == true).Count());
                }
                if (data.Nwnp != null && data.Nwnp.Where(x => x.InputSource == InputSource.Rp14a).Any())
                {
                    Assert.True(res.Nwnp.rp14aResults.WeeklyResult.Count() > 0);
                    res.Nwnp.rp14aResults.WeeklyResult.Where(x => x.IsSelected == true).Count()
                        .Should().Be(expectedOutputList[i].Nwnp.rp14aResults.WeeklyResult.Where(x => x.IsSelected == true).Count());
                }
                i++;
            } 
        }
        
    }
}
