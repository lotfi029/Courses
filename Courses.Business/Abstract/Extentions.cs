using System.Security.Claims;

namespace Courses.Business.Abstract;

public static class Extentions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)! ?? string.Empty;
    }
}