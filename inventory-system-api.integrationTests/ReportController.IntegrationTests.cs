using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class ReportControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ReportControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GrossProfit_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/Report/GrossProfit");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.True(payload.StatusCode == 200);
    }

    [Fact]
    public async Task Inventory_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/Report/Inventory");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.True(payload.StatusCode == 200);
    }
}
