using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class ProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ProductControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ReturnsOk_WithProductList()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/Product");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOk_AndReturnsId()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        string guid = Guid.NewGuid().ToString();

        var newProduct = new
        {
            EngName = "Test Product 1" + guid,
            NepName = "परीक्षण 1" + guid,

            GroupID = 72,
            Code = "TP" + guid,
            DepotID = 1,
            Remarks = "integration test",
            UnitID = 18,
            UnitName = "Piece",
            UnitSymbol = "pc",
            SalesRate = 100.0m,
            PurchaseQuantity = 1.0m,
            PurchaseRate = 90.0m,
            PurchaseDiscount = 0.0m,
            TotalValue = 90.0m,
            IsBuiltIn = false,
            IsActive = true,
            CreatedBy = "integration",
            IsVatApplicable = true,
            IsInventoryApplicable = true,
            Quantity = 10.0m,
            IsDecimalApplicable = false,
            ContactPerson = "John",
            Address1 = "Addr1",
            Address2 = "Addr2",
            City = "City",
            Telephone = "12345",
            Email = "prod@test.com",
            Company = "TestCo",
            Website = "https://test",
            CompanyID = 1,
            Size ="3",
            TaxID = 1,
            ConversionRate = 1.0m
        };

        var content = JsonContent.Create(newProduct);

        var response = await client.PostAsync("/api/v1/Product", content);


        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(payload.TryGetProperty("data", out var data));
        Assert.True(data.TryGetProperty("id", out var idProp));
        Assert.True(idProp.GetInt32() > 0);
    }
}
