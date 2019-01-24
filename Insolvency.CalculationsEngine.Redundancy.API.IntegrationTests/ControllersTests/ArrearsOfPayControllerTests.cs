using System.Net.Http;
using FluentAssertions;
using Insolvency.CalculationsEngine.Redundancy.API.UnitTests.TestData;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.API.IntegrationTests.ControllersTests
{
    public class ArrearsOfPayControllerTests
    {
        //public ArrearsOfPayControllerTests()
        //{
        //    // Arrange
        //    _arrearsOfPayControllerTestDataGenerator = new ArrearsOfPayTestsDataGenerator();
        //    _server = new TestServer(new WebHostBuilder()
        //        .UseStartup<Startup>());
        //    _client = _server.CreateClient();
        //}

        //private readonly TestServer _server;
        //private readonly HttpClient _client;
        //private readonly ArrearsOfPayTestsDataGenerator _arrearsOfPayControllerTestDataGenerator;

        //[Fact]
        //public async void PostAsync_ReturnsBadRequest()
        //{
        //    var request = "/api/ArrearsOfPay";
        //    var requestData = _arrearsOfPayControllerTestDataGenerator.GetInvalidRequestData();
        //    var payload = JsonConvert.SerializeObject(requestData);
        //    var result = await _client.PostAsJsonAsync(request, requestData);
        //    result.StatusCode.Should().Be(400);
        //}
    }
}