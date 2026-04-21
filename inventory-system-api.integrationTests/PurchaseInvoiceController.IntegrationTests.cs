using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class PurchaseInvoiceControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PurchaseInvoiceControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ReturnsOk_WithPurchaseList()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/PurchaseInvoice");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.IsNotNull(payload);
        Assert.AreEqual(200, payload.StatusCode);
        Assert.AreEqual("Success", payload.Message);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/PurchaseInvoice/1");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.IsNotNull(payload);
        Assert.AreEqual(200, payload.StatusCode);
        Assert.AreEqual("Success", payload.Message);
    }

    [Fact]
    public async Task CreatePurchase_ReturnsOk_AndReturnsId()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var newInvoice = new
        {
            EntityName = "Test Supplier",
            Date = DateTime.UtcNow,
            NetAmount = 200.00m,
            Details = new[] {
                new { ProductID = 1, ProductName = "P1", Quantity = 2, Price = 100.00m, NetAmount = 200.00m }
            }
        };

        var content = JsonContent.Create(newInvoice);

        var response = await client.PostAsync("/api/v1/PurchaseInvoice", content);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.IsTrue(payload.TryGetProperty("data", out var data));
        Assert.IsTrue(data.TryGetProperty("id", out var idProp));
        Assert.IsTrue(idProp.GetInt32() > 0);
    }
}
