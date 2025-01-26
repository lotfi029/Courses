using Courses.Business.Contract.User;

namespace Courses.Presentation.Controllers;
[Route("u")]
[ApiController]
[Authorize]
public class AccountsController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = User.GetUserId()!;

        var result = await _userService.GetProfileAsync(userId);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request)
    {
        var userId = User.GetUserId()!;

        var result = await _userService.UpdateProfileAsync(userId, request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.GetUserId()!;

        var result = await _userService.ChangePasswordAsync(userId, request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    
}
