namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    [HasPermission(Permissions.GetProfile)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _userService.GetAllAsync(cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.GetProfile)]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        
        var result = await _userService.GetAsync(id.ToString(), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
