using Courses.Business.Contract.Lesson;
using Courses.Business.Entities;

namespace Courses.Presentation.Controllers;
[Route("api/{courseId:guid}/modules/{moduleId:guid}/[controller]")]
[ApiController]
[Authorize]
public class LessonsController(ILessonService lessonService) : ControllerBase
{
    private readonly ILessonService _lessonService = lessonService;
    
    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute]Guid courseId, [FromRoute] Guid moduleId,[FromForm] LessonRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _lessonService.AddAsync(moduleId, userId, request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new {courseId,moduleId,id = result.Value}, null) : result.ToProblem();
    }
    [HttpPost("add-recourse/{id:guid}")]
    public async Task<IActionResult> AddResourse([FromRoute] Guid id, [FromForm] RecourseRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _lessonService.AddResourceAsync(id, request, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] LessonRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        var result = await _lessonService.UpdateAsync(id, request, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("toggle-preview/{id:guid}")]
    public async Task<IActionResult> ToggleIsPreview([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        var result = await _lessonService.ToggleIsPreviewAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _lessonService.GetAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] Guid moduleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _lessonService.GetAllAsync(moduleId, userId, cancellationToken);

        return Ok(result);
    }
}
