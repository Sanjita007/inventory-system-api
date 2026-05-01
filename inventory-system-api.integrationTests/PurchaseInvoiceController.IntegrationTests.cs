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

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/PurchaseInvoice/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task CreatePurchase_ReturnsOk_AndReturnsId()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var newInvoice = new
        {
            ID = 0,
            VoucherNo = "PNEW",
            EntityName = "NewSupplier",
            Date = DateTime.UtcNow,
            TotalQty = 0m,
            GrossAmount = 0m,
            SpecialDiscount = 0m,
            NetAmount = 0m,
            TotalTCAmount = 0m,
            TenderAmount = 0m,
            ChangeAmount = 0m,
            AdjustmentAmount = 0m,
            Status = "PAID",
            CreatedDate = DateTime.UtcNow,
            CreatedBy = 1,
            CompanyID = 1,
            Remarks = "unit test",
            Details = new[] {
                new { ProductID = 1, ProductName = "P1", Quantity = 2, Price = 100.00m, NetAmount = 200.00m }
            }
        };

        var content = JsonContent.Create(newInvoice);

        var response = await client.PostAsync("/api/v1/PurchaseInvoice", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.TryGetProperty("data", out var data));
        Assert.True(data.TryGetProperty("id", out var idProp));
        Assert.True(idProp.GetInt32() > 0);
    }
}
