using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Courses.Business.Authentication;
public interface IJwtProvider
{
    AccessTokenResponse GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? ValidateToken(string Token);

}
