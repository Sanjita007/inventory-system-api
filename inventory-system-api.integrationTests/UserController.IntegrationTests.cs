using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public UserControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_ReturnsOk_WithToken_WhenCredentialsValid()
    {
        var client = _factory.CreateClient();

        var login = new { UserName = "admin", Password = "admin" };
        var content = JsonContent.Create(login);

        var response = await client.PostAsync("/api/v1/User/Login", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.TryGetProperty("data", out var data));
        Assert.True(data.TryGetProperty("token", out var token));
        Assert.False(string.IsNullOrEmpty(token.GetString()));
    }
}
