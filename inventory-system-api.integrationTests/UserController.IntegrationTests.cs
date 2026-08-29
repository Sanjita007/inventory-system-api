using inventory_system_api.integrationTests.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Assert = Xunit.Assert;

namespace inventory_system_api.integrationTests;

public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UserControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
    }

    [Fact]
    public async Task Login_ReturnsOk_WithToken_WhenCredentialsValid()
    {
        var login = new { UserName = "SanjitaPau", Password = "Qazwsx@123" };
        var content = JsonContent.Create(login);

        var response = await _client.PostAsync("/api/v1/User/Login", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.TryGetProperty("data", out var data));
        Assert.True(data.TryGetProperty("token", out var token));
        Assert.False(string.IsNullOrEmpty(token.GetString()));
    }
}
