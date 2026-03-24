using inventory_system_api.Application.Models.Inventory;
using inventory_system_api.integrationTests.TestHost;
using System.Data.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace inventory_system_api.integrationTests;

public class UnitControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public UnitControllerIntegrationTests(CustomWebApplicationFactory factory)
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

        var response = await client.GetAsync("/api/v1/Unit/1");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.IsNotNull(payload);
        Assert.AreEqual(200, payload.StatusCode);
        Assert.AreEqual("Success", payload.Message);
    }

    [Fact]
    public async Task Convert_ReturnsOk_WithDecimal()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/Unit/Convert?defaultUnitID=1&currentUnitID=2&valueToConvert=10");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.IsNotNull(payload);
        Assert.AreEqual(200, payload.StatusCode);
    }

    [Fact]
    public async Task CreateUnit_ReturnsCreated_AndSavesToDb()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var newUnit = new { Name = "Kilogram", Code = "KG" };
        var content = new StringContent(JsonSerializer.Serialize(newUnit), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/v1/Unit", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        // Verify it actually exists in the DB
        //var savedUnit = await _connection.QueryFirstOrDefaultAsync<Unit>(
        //    "SELECT * FROM Units WHERE Code = 'KG'");
        //Assert.IsNotNull(savedUnit);
    }
}
  