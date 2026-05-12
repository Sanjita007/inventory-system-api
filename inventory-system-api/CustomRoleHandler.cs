using Microsoft.AspNetCore.Authorization;
public class MethodRoleRequirement : IAuthorizationRequirement { }
public class CustomMethodRoleHandler : AuthorizationHandler<MethodRoleRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Inject the accessor
    public CustomMethodRoleHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MethodRoleRequirement requirement)
    {
        // Use the accessor to get HttpContext
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null) return Task.CompletedTask;

        var endpoint = httpContext.GetEndpoint();

        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var method = httpContext.Request.Method;
        var user = context.User;

        // logic for GET/POST/PUT...
        if (HttpMethods.IsGet(method) && user.Identity.IsAuthenticated)
        {
            context.Succeed(requirement);
        }
        else if ((HttpMethods.IsPost(method) || HttpMethods.IsPut(method))
                 && (user.IsInRole("Admin") || user.IsInRole("User")))
        {
            context.Succeed(requirement);
        }
        // only allow delete for the admin for now..
        else if (HttpMethods.IsDelete(method) 
                 && user.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}