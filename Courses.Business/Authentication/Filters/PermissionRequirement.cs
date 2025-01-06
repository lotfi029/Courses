using Microsoft.AspNetCore.Authorization;

namespace Courses.Business.Authentication.Filters;
public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
