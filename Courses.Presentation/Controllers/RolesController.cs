using Courses.Business.Contract.Role;

namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpPost("")]
    [HasPermission(Permissions.AddRole)]
    public async Task<IActionResult> Add([FromBody] RoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.AddAsync(request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new {result.Value}, null) : result.ToProblem();
    }
    [HttpPut("{id:alpha}")]
    [HasPermission(Permissions.UpdateRole)]
    public async Task<IActionResult> Update([FromRoute]string id, [FromBody] RoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("toggle-status/{id:alpha}")]
    [HasPermission(Permissions.UpdateRole)]
    public async Task<IActionResult> ToggleStatus([FromRoute]string id, CancellationToken cancellationToken)
    {
        var result = await _roleService.ToggleStatusAsync(id, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:alpha}")]
    [HasPermission(Permissions.GetRole)]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    [HasPermission(Permissions.GetRole)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken )
    {
        var result = await _roleService.GetAllAsync(cancellationToken: cancellationToken);

        return Ok(result);
    }
}