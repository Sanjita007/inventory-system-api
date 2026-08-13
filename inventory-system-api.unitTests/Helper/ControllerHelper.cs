using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class ControllerHelper
{
    public static void SetMockUser(this ControllerBase controller, int userId = 1)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.NameId, userId.ToString())
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }
}