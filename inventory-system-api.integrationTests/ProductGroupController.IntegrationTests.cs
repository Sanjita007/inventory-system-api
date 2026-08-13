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
    private readonly HttpClient _client;

    public ProductGroupControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    [Fact]
    public async Task Get_ReturnsOk_WithGroups()
    {

        var response = await _client.GetAsync("/api/v1/ProductGroup");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<Models.Response>();
        Assert.NotNull(payload);
        Assert.Equal(200, payload.StatusCode);
        Assert.Equal("Success", payload.Message);
    }

    [Fact]
    public async Task CreateGroup_ReturnsOk_AndReturnsId()
    {
        string guid = Guid.NewGuid().ToString();   

        var newGroup = new { ParentGroupID = 72, EngName = "IGroup" + guid, NepName = "समूह" + guid, Level = 0, ParentGroupName = "", Remarks = "test" };
        var content = JsonContent.Create(newGroup);

        var response = await _client.PostAsync("/api/v1/ProductGroup", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.TryGetProperty("data", out var data));
        Assert.True(data.TryGetProperty("id", out var idProp));
        Assert.True(idProp.GetInt32() > 0);
    }
}
