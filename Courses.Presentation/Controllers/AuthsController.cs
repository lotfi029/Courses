using Courses.Business.Contract.Auth;

namespace Courses.Presentation.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthsController(IAuthService authService,IFileService fileService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IFileService _fileService = fileService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var result = await _authService.RegisterAsync(registerRequest);
        
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await _authService.ConfirmEmailAsync(request);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPost("re-confirm-email")]
    public async Task<IActionResult> ReConfirmEmail([FromBody] ResendConfirmEmailRequest request)
    {
        var result = await _authService.ReConfirmEmailAsync(request);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPost("get-token")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
    {
        var result = await _authService.GetTokenAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetRefreshTokenAsync(request, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeAsync(request, cancellationToken);
        
        return result.IsSuccess ? Ok(result) : result.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordTokenAsync(request.Email);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);
        
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    
}
