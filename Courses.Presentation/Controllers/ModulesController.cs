using Courses.Business.Contract.Module;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Presentation.Controllers;
[Route("api/{courseId:guid}/[controller]")]
[ApiController]
[Authorize]
public class ModulesController(IModuleService moduleService) : ControllerBase
{
    private readonly IModuleService _moduleService = moduleService;
    
    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] Guid courseId, [FromBody] ModuleRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _moduleService.AddAsync(userId, courseId, request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new {courseId,id = result.Value}, null) : result.ToProblem();
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ModuleRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _moduleService.UpdateAsync(id, request, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _moduleService.ToggleStatusAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _moduleService.GetAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _moduleService.GetAllAsync(courseId, userId, cancellationToken);

        return Ok(result);
    }
}