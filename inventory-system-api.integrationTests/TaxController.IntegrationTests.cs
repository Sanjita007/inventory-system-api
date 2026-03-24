using System.Net;
using System.Net.Http.Json;
using inventory_system_api.integrationTests.TestHost;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace inventory_system_api.integrationTests;

public class TaxControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public TaxControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ReturnsOk_WithUnitsList()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/Unit");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.IsNotNull(payload);
        Assert.AreEqual(200, payload.StatusCode);
        Assert.AreEqual("Success", payload.Message);
        // Data may be null or an array depending on repository; assert shape only
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenUnitExists()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/Tax/1");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.IsNotNull(payload);
        Assert.AreEqual(200, payload.StatusCode);
        Assert.AreEqual("Success", payload.Message);
    }

  
}
  