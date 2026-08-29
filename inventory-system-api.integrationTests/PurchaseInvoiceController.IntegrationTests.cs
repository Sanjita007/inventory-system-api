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
    private readonly HttpClient _client;

    public PurchaseInvoiceControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    [Fact]
    public async Task Get_ReturnsOk_WithPurchaseList()
    {
        var response = await _client.GetAsync("/api/v1/PurchaseInvoice");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {

        var response = await _client.GetAsync("/api/v1/PurchaseInvoice/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task CreatePurchase_ReturnsOk_AndReturnsId()
    {
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

        var response = await _client.PostAsync("/api/v1/PurchaseInvoice", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.TryGetProperty("data", out var data));
        Assert.True(data.TryGetProperty("id", out var idProp));
        Assert.True(idProp.GetInt32() > 0);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenDeleted()
    {
        // create first
        var newInvoice = new
        {
            VoucherNo = "VDEL" + Guid.NewGuid().ToString(),
            EntityName = "To Delete",
            Date = DateTime.UtcNow,
            NetAmount = 77.00m,
            SpecialDiscount = 0m,
            TenderAmount = 0m,
            ChangeAmount = 0m,
            AdjustmentAmount = 0m,
            
            GrossAmount = 77.00m,
            TotalAmount = 77.00m,
            Details = new[] {
                new {
                    ProductID = 1, ProductName = "P1",
                                

                    Quantity = 1, Price = 77.00m, NetAmount = 77.00m
                }
            }
        };
        var createResp = await _client.PostAsync("/api/v1/PurchaseInvoice", JsonContent.Create(newInvoice));
        Assert.Equal(HttpStatusCode.OK, createResp.StatusCode);
        var createPayload = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(createPayload.TryGetProperty("data", out var createData));
        Assert.True(createData.TryGetProperty("id", out var idProp));
        var id = idProp.GetInt32();

        // delete
        var delResp = await _client.DeleteAsync($"/api/v1/PurchaseInvoice/{id}");
        Assert.Equal(HttpStatusCode.OK, delResp.StatusCode);

        var delPayload = await delResp.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(delPayload);
        Assert.Equal(200, delPayload.StatusCode);
        Assert.Equal("Success", delPayload.Message);
    }

}
