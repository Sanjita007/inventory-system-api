using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class ProductGroupControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ProductGroupControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ReturnsOk_WithGroups()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/ProductGroup");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.IsNotNull(payload);
        Assert.AreEqual(200, payload.StatusCode);
        Assert.AreEqual("Success", payload.Message);
    }

    [Fact]
    public async Task CreateGroup_ReturnsOk_AndReturnsId()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var newGroup = new { ParentGroupID = 0, EngName = "IGroup", NepName = "समूह", Level = 0, ParentGroupName = "", Remarks = "test" };
        var content = JsonContent.Create(newGroup);

        var response = await client.PostAsync("/api/v1/ProductGroup", content);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.IsTrue(payload.TryGetProperty("data", out var data));
        Assert.IsTrue(data.TryGetProperty("id", out var idProp));
        Assert.IsTrue(idProp.GetInt32() > 0);
    }
}
