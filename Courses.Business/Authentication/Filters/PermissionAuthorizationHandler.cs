﻿using Microsoft.AspNetCore.Authorization;

namespace Courses.Business.Authentication.Filters;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {

        if (context.User.Identity is not { IsAuthenticated: true } 
        || !context.User.Claims.Any(e => e.Value == requirement.Permission && e.Type == Permissions.Type))
            return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
