using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Insolvency.CalculationsEngine.Redundancy.API.IntegrationTests.ControllersTests
{
    public class HealthControllerTests
    {
        //public HealthControllerTests()
        //{
        //    // Arrange
        //    _server = new TestServer(new WebHostBuilder()
        //        .UseStartup<Startup>());
        //    _client = _server.CreateClient();
        //}

        //private readonly TestServer _server;
        //private readonly HttpClient _client;

        //[Fact]
        //[Trait("Category", "IntegrationTest")]
        //public async void Get()
        //{
        //    var request = "/api/v1/HealthCheck";
        //    var response = await _client.GetAsync(request);
        //    var okResult = response.StatusCode.Should().Be(200);
        //}
    }
}