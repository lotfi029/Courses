using Microsoft.AspNetCore.Authorization;

namespace Courses.Business.Authentication.Filters;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{

}
