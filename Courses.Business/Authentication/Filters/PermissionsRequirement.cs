using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Courses.Business.Authentication.Filters;
public class PermissionsRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}

public class HasPermission(string permission) : AuthorizeAttribute(permission);

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true } 
        || !context.User.Claims.Any(e => e.Value == requirement.Permission && e.Type == Permissions.Type))
            return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    private readonly AuthorizationOptions _options = options.Value;

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
            return policy;

        var permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionsRequirement(policyName))
            .Build();

        _options.AddPolicy(policyName, permissionPolicy);

        return permissionPolicy;
    }
}