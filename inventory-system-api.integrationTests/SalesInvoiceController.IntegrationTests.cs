using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class SalesInvoiceControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public SalesInvoiceControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ReturnsOk_WithSalesList()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/SalesInvoice");

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

        var response = await client.GetAsync("/api/v1/SalesInvoice/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task CreateSales_ReturnsOk_AndReturnsId()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var newInvoice = new
        {
            ID = 0,
            VoucherNo = "VNEW",
            EntityName = "New",
            Date = DateTime.UtcNow,
            ProjectID = 1,
            TotalQty = 1m,
            GrossAmount = 123.45,
            SpecialDiscount = 0m,
            NetAmount = 123.45m,
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
                new { ProductID = 1, ProductName = "P1", Quantity = 1, Price = 123.45m, NetAmount = 123.45m }
            }
        };

        var content = JsonContent.Create(newInvoice);

        var response = await client.PostAsync("/api/v1/SalesInvoice", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.TryGetProperty("data", out var data));
        Assert.True(data.TryGetProperty("id", out var idProp));
        Assert.True(idProp.GetInt32() > 0);
    }

    [Fact]
    public async Task Put_ReturnsOk_OnSuccess()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // create first
        var newInvoice = new
        {
            ID = 0,
            VoucherNo = "VNEW",
            EntityName = "New",
            Date = DateTime.UtcNow,
            ProjectID = 1,
            TotalQty = 1m,
            GrossAmount = 50,
            SpecialDiscount = 0m,
            NetAmount = 50m,
            TotalTCAmount = 0m,
            TenderAmount = 0m,
            ChangeAmount = 0m,
            AdjustmentAmount = 0m,
            Status = "PAID",
            CreatedDate = DateTime.UtcNow,
            CreatedBy = 1,
            CompanyID = 1,
            Remarks = "unit test",
            Details = new[] { new { ProductID = 1, ProductName = "P1", Quantity = 1, Price = 50.00m, NetAmount = 50.00m } }
        };
        var createResp = await client.PostAsync("/api/v1/SalesInvoice", JsonContent.Create(newInvoice));
        Assert.Equal(HttpStatusCode.OK, createResp.StatusCode);
        var createPayload = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(createPayload.TryGetProperty("data", out var createData));
        Assert.True(createData.TryGetProperty("id", out var idProp));
        var id = idProp.GetInt32();

        // perform update
        var updateInvoice = new
        {
            ID = id,
            EntityName = "Updated Customer",
            Date = DateTime.UtcNow,
            NetAmount = 200.00m,
            Details = new[] { new { ProductID = 1, ProductName = "P1", Quantity = 2, Price = 100.00m, NetAmount = 200.00m } }
        };

        var putResp = await client.PutAsync("/api/v1/SalesInvoice", JsonContent.Create(updateInvoice));
        Assert.Equal(HttpStatusCode.OK, putResp.StatusCode);

        var putPayload = await putResp.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(putPayload);
        Assert.Equal(200, putPayload.StatusCode);
        Assert.Equal("Success", putPayload.Message);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenDeleted()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // create first
        var newInvoice = new
        {
            EntityName = "To Delete",
            Date = DateTime.UtcNow,
            NetAmount = 77.00m,
            Details = new[] { new { ProductID = 1, ProductName = "P1", Quantity = 1, Price = 77.00m, NetAmount = 77.00m } }
        };
        var createResp = await client.PostAsync("/api/v1/SalesInvoice", JsonContent.Create(newInvoice));
        Assert.Equal(HttpStatusCode.OK, createResp.StatusCode);
        var createPayload = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(createPayload.TryGetProperty("data", out var createData));
        Assert.True(createData.TryGetProperty("id", out var idProp));
        var id = idProp.GetInt32();

        // delete
        var delResp = await client.DeleteAsync($"/api/v1/SalesInvoice/{id}");
        Assert.Equal(HttpStatusCode.OK, delResp.StatusCode);

        var delPayload = await delResp.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(delPayload);
        Assert.Equal(200, delPayload.StatusCode);
        Assert.Equal("Success", delPayload.Message);
    }
}
