using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        var response = await client.GetAsync("/api/v1/Tax");

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

    [Fact]
    public async Task CreateTax_ReturnsCreated_AndSavesToDb()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        string guid = Guid.NewGuid().ToString().Substring(0, 5);
        var newUnit = new { Name = "Tax 0001" + guid, Code = "T000" + guid, Rate= 7.8, Remarks= "This is a test tax" };
        var content = new StringContent(JsonSerializer.Serialize(newUnit), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/v1/Tax", content);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        
        int newId = payload.GetProperty("data").GetProperty("id").GetInt32();

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        // Verify the unit was saved by fetching it back (assuming ID is returned in response)
        var getResponse = await client.GetAsync($"/api/v1/Tax/{newId}");

        var payloadReturn = await response.Content.ReadFromJsonAsync<Models.Response>();

        Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.IsNotNull(payloadReturn?.Data);
    }
}
  