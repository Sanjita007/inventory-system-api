using Azure;
using inventory_system_api.integrationTests.TestHost;
using inventory_system_api.Models;
using inventory_system_api.Models.Inventory;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class ProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;    

    public ProductControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    [Fact]
    public async Task Get_ReturnsOk_WithProductList()
    {

        var response = await _client.GetAsync("/api/v1/Product");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task Get_ReturnsOk_WithProductId()
    {
        int id = 17643;

        var response = await _client.GetAsync($"/api/v1/Product/{id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOk_AndReturnsId()
    {
        // Arrange: setup the product to save

        // use GUID so that every time we run this test, unique product name is created and we don't have to worry about duplicate product name issue
        string guid = Guid.NewGuid().ToString();

        var newProduct = CreateSampleProductPayload(guid);
       
        //Act : send a POST request to save the data
        var response = await _client.PostAsync("/api/v1/Product", JsonContent.Create(newProduct));

        // Assert: check if everything went according to the intent
        var payload = await ((HttpResponseMessage)response).Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(200, payload.GetProperty("statusCode").GetInt32());
        Assert.Equal("Success", payload.GetProperty("message").GetString());
        Assert.True(payload.GetProperty("data").GetProperty("id").GetInt32() > 0);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOk_AndReturnsId()
    {
        // Arrange: Setup unique identifier for test isolation
        string guid = Guid.NewGuid().ToString();
        var newProductPayload = CreateSampleProductPayload(guid);

        // Act: Send POST request to create the initial product
        var response = await _client.PostAsync("/api/v1/Product", JsonContent.Create(newProductPayload));
        response.EnsureSuccessStatusCode();

        var payload = await ((HttpResponseMessage)response).Content.ReadFromJsonAsync<JsonElement>();
        int id = payload.GetProperty("data").GetProperty("id").GetInt32();

        // Instantiate and populate the strongly-typed Product model for update
        var updatedProduct = new Product
        {
            ID = id,
            EngName = "Updated Product Name " + guid,
            NepName = "अपडेटेड नाम " + guid,
            SalesRate = 200.0m,
            PurchaseRate = 180.0m,
            IsActive = false,
            IsVatApplicable = true,
            IsInventoryApplicable = true,
            IsDecimalApplicable = true,
            ContactPerson = "John U",
            Address1 = "Addr 1",
            Address2 = "Addr 2",
            City = "City U",
            Telephone = "1234545",
            Email = "prod@testupdate.com",
            Company = "TestCompany",
            Website = "https://test.updated.com",
            CompanyID = 1,
            Size = "6",
            TaxID = 1,
            GroupID = 72,
            Code = "TP" + guid,
            Remarks = "integration test",
            UnitID = 18,
            UnitName = "Piece",
            UnitSymbol = "pc",
            PurchaseDiscount = 0.0m,
            IsBuiltIn = false,
        };

        // Act: Send PUT request to update the product using the Product model
        var updateResponse = await _client.PutAsJsonAsync("/api/v1/Product", updatedProduct);
        updateResponse.EnsureSuccessStatusCode();

        // Act: GET the updated product from database to verify persistence
        var updateCheckResponse = await _client.GetAsync($"/api/v1/Product/{id}");

        // Assert: Verify HTTP status code
        Assert.Equal(HttpStatusCode.OK, updateCheckResponse.StatusCode);

        var updateCheckPayload = await updateCheckResponse.Content.ReadFromJsonAsync<CustomResponse<Product>>();

        // Assert: Validate payload structure and property values
        Assert.NotNull(updateCheckPayload);
        Assert.NotNull(updateCheckPayload.Data);

        // Check specific persisted fields
        // update later to test all the fields
        Assert.Equal(id, updateCheckPayload.Data.ID);
        Assert.Equal(updatedProduct.EngName, updateCheckPayload.Data.EngName);
        Assert.Equal(updatedProduct.NepName, updateCheckPayload.Data.NepName);
        Assert.Equal(updatedProduct.SalesRate, updateCheckPayload.Data.SalesRate);
        Assert.Equal(updatedProduct.PurchaseRate, updateCheckPayload.Data.PurchaseRate);
        Assert.Equal(updatedProduct.OpeningQuantity, updateCheckPayload.Data.OpeningQuantity);
        Assert.Equal(updatedProduct.IsActive, updateCheckPayload.Data.IsActive);
        Assert.Equal(updatedProduct.Email, updateCheckPayload.Data.Email);
    }

    [Fact]
    public async Task GetProduct_ExistingProduct_ReturnsSuccess()
    {
        // Arrange: First, create a product explicitly to ensure there is a product to fetch later
        string guid = Guid.NewGuid().ToString();
        var newProduct = CreateSampleProductPayload(guid);

        // Send POST request to create the product 
        var createResponse = await _client.PostAsync("/api/v1/Product", JsonContent.Create(newProduct));
        createResponse.EnsureSuccessStatusCode();

        // Deserialize the created product response to get the assigned Product ID
        var createPayload = await ((HttpResponseMessage)createResponse).Content.ReadFromJsonAsync<JsonElement>();
        var productId = createPayload.GetProperty("data").GetProperty("id").GetInt32();

        // Act: Send a GET request to fetch the product by its assigned ID
        var response = await _client.GetAsync($"/api/v1/Product/{productId}");

        // Assert: Confirm the GET request succeeded
        response.EnsureSuccessStatusCode();

        // Deserialize the product details from the response
        var getPayload = await response.Content.ReadFromJsonAsync<CustomResponse<Product>>();
        var Product = getPayload?.Data;

        // Assert: Validate the response is not null and the returned product properties match
        Assert.NotNull(getPayload?.Data);
        Assert.Equal(getPayload.Data, Product);
    }

    // Helper method to build reusable test product payload
    private static dynamic CreateSampleProductPayload(string guid)
    {
        return new Product()
        {
            EngName = "Test Product 1 " + guid,
            NepName = "परीक्षण 1 " + guid,
            GroupID = 72,
            Code = "TP" + guid,
            Remarks = "integration test",
            UnitID = 18,
            UnitName = "Piece",
            UnitSymbol = "pc",
            SalesRate = 100.0m,
            PurchaseRate = 90.0m,
            PurchaseDiscount = 0.0m,
            IsBuiltIn = false,
            IsActive = true,
            CreatedBy = "integration",
            IsVatApplicable = true,
            IsInventoryApplicable = true,
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
            Size = "3",
            TaxID = 1,
            ID=0
        };
    }
}
