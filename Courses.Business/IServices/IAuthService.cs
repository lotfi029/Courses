using Courses.Business.Contract.User;

namespace Courses.Business.IServices;
public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest request);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ReConfirmEmailAsync(ResendConfirmEmailRequest request);
    Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> RevokeAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> SendResetPasswordTokenAsync(string email);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
}
