namespace Courses.Business.Contract.Auth;

public record AuthResponse(
    string AccessToken, 
    long ExpireOn, 
    string RefreshToken, 
    DateTime RefreshTokenExpiration
    )
{
    public string TokenType { get; } = "Bearer";
    // TODO: RefreshToken
}
