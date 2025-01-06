using Courses.Business.Abstract.Constants;
using Courses.Business.Contract.User;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Courses.DataAccess.Services;
public class AuthService(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext context,
    IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ApplicationDbContext _context = context;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDays = 14;
    public async Task<Result> RegisterAsync(RegisterRequest request)
    {
        if (await _userManager.Users.AnyAsync(u => u.Email == request.Email))
            return Result.Failure(UserErrors.InvalidEmail);

        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.UserName;

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new(error.Code, error.Description, StatusCodes.Status409Conflict));
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        Log.Information(code);

        return Result.Success();
    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return UserErrors.InvalidCode;

        var code = request.Code;
        IdentityResult result;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            result = await _userManager.ConfirmEmailAsync(user, code);
        }
        catch(FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }


        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }

        await _userManager.AddToRoleAsync(user, DefaultRoles.Student.Name);

        return Result.Success();
    }
    public async Task<Result> ReConfirmEmailAsync(ResendConfirmEmailRequest request)
    {

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return UserErrors.EmailConfirmed;


        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));        

        Log.Information(code);


        return Result.Success();
    }
    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request)
    {
        var user = new EmailAddressAttribute().IsValid(request.Email) 
            ? await _userManager.FindByEmailAsync(request.Email)
            : await _userManager.FindByNameAsync(request.Email);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredinitails);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.UserIsDisable);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (!result.Succeeded)
        {
            var error =
                result.IsNotAllowed
                ? UserErrors.EmailNotConfirmed
                : result.IsLockedOut
                ? UserErrors.LockedUser
                : UserErrors.InvalidCredinitails;

            return Result.Failure<AuthResponse>(error);
        }

        var accessToken = await GetTokenAsync(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiresOn = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            ExpiresOn = refreshTokenExpiresOn,
            Token = refreshToken,
            CreateOn = DateTime.UtcNow
        });

        await _userManager.UpdateAsync(user);

        var respone = new AuthResponse(accessToken.AccessToken, accessToken.ExpiresIn, refreshToken, refreshTokenExpiresOn);

        return Result.Success(respone);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(
        RefreshTokenRequest request, 
        CancellationToken cancellationToken
        )
    {
        var userId = _jwtProvider.ValidateToken(request.Token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.UserIsDisable);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.LockedUser);

        var userRefreshToken = user.RefreshTokens
            .SingleOrDefault(e => e.IsActive && e.Token == request.RefreshToken);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        userRefreshToken.RevokeOn = DateTime.UtcNow;

        var accessToken = await GetTokenAsync(user);
        var newRefreshToken = GenerateRefreshToken();

        var refreshTokenExpiresOn = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            ExpiresOn = refreshTokenExpiresOn,
            Token = newRefreshToken,
            CreateOn = DateTime.UtcNow
        });

        await _userManager.UpdateAsync(user);

        var respone = new AuthResponse(
            accessToken.AccessToken, 
            accessToken.ExpiresIn, 
            newRefreshToken, 
            refreshTokenExpiresOn
            );

        return Result.Success(respone);
    }
    public async Task<Result> RevokeAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(request.Token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidToken);

        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.InvalidToken);

        var userRefreshToken = user.RefreshTokens
            .SingleOrDefault(e => e.IsActive && e.Token == request.RefreshToken);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidToken);

        userRefreshToken.RevokeOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
    public async Task<Result> SendResetPasswordTokenAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return UserErrors.EmailNotConfirmed;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user!);

        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        Log.Information(token);

        return Result.Success();
    }
    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Success();

        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetToken));

            result = await _userManager.ResetPasswordAsync(user!, code, request.NewPassword);
        }
        catch(FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }
        
        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }

        return Result.Success();
    }
    public async Task<AccessTokenResponse> GetTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var permission = await _context.Roles
            .Join(
                _context.RoleClaims,
                r => r.Id,
                rc => rc.RoleId,
                (role, claim) => new { role, claim }
            )
            .Where(e => roles.Contains(e.role.Name!))
            .Select(e => e.claim.ClaimValue!)
            .Distinct()
            .ToListAsync();

        var accessToken = _jwtProvider.GenerateToken(user, roles, permission);

        return accessToken;
    }
    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
