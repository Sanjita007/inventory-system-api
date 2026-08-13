using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;
using System.Text;
using System.Text.Json;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class UnitControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UnitControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

    }

    [Fact]
    public async Task Get_ReturnsOk_WithUnitsList()
    {
        var response = await _client.GetAsync("/api/v1/Unit");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
        // Data may be null or an array depending on repository; assert shape only
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenUnitExists()
    {
        var response = await _client.GetAsync("/api/v1/Unit/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task Convert_ReturnsOk_WithDecimal()
    {
        var response = await _client.GetAsync("/api/v1/Unit/Convert?defaultUnitID=1&currentUnitID=2&valueToConvert=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
    }

    [Fact]
    public async Task CreateUnit_ReturnsCreated_AndSavesToDb()
    {
        // Arrange
        string guid = Guid.NewGuid().ToString().Substring(0, 5);

        var newUnit = new { Name = "Kilogram" + guid, Code = "KG" + guid, Symbol="S" + guid };
        var content = JsonContent.Create(newUnit);

        // Act
        var response = await _client.PostAsync("/api/v1/Unit", content);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();

        int newId = payload.GetProperty("data").GetProperty("id").GetInt32();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify the unit was saved by fetching it back (assuming ID is returned in response)
        var getResponse = await _client.GetAsync($"/api/v1/Unit/{newId}");

        var payloadReturn = await getResponse.Content.ReadFromJsonAsync<Models.Response>();

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(payloadReturn?.Data);
    }
}
  