using Courses.Business.Contract.Course;
namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;
    [HttpPost("")]
    [HasPermission(Permissions.AddCourse)]
    public async Task<IActionResult> Add([FromForm] AddCourseRequest request, CancellationToken cancellationToken)
    {
        var result = await _courseService.AddAsync(request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new {id = result.Value}, null) : result.ToProblem();
    }
    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.UpdateCourse)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.UpdateAsync(id, userId, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/update-thumbnail")]
    [HasPermission(Permissions.UpdateCourse)]
    public async Task<IActionResult> UpdateThumbnail([FromRoute] Guid id, [FromForm] UploadImageRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.UpdateThumbnailAsync(id, userId, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/assign-category/{categoryId:guid}")]
    [HasPermission(Permissions.UpdateCourse)]
    public async Task<IActionResult> AssignCourseToCategories([FromRoute] Guid id,[FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.AssignCourseToCategoryAsync(id, userId, categoryId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }
    [HttpPut("{id:guid}/unassign-category/{categoryId:guid}")]
    [HasPermission(Permissions.UpdateCourse)]
    public async Task<IActionResult> UnAssignCourseToCategories([FromRoute] Guid id, [FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.UnAssignCourseToCategoriesAsync(id, userId, categoryId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/toggle-status")]
    [HasPermission(Permissions.UpdateCourse)]
    public async Task<IActionResult> ToggleIsPublish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.ToggleIsPublishAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/block-user")]
    [HasPermission(Permissions.BlockUserCourse)]
    public async Task<IActionResult> BlockUser([FromRoute] Guid id, [FromBody] UserIdentifierRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.BlockedUserAsync(id, userId, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/unblock-user")]
    [HasPermission(Permissions.BlockUserCourse)]
    public async Task<IActionResult> UnBlockUser([FromRoute] Guid id, [FromBody] UserIdentifierRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.UnBlockedUserAsync(id, userId, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.GetCourse)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.GetAsync(id, userId, cancellationToken: cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    [HasPermission(Permissions.GetCourse)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.GetAllAsync(userId, cancellationToken: cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id:guid}/users")]
    [HasPermission(Permissions.GetCourse)]
    public async Task<IActionResult> GetUsersCourse([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.GetUsersInCourseAsync(id, userId, cancellationToken);

        return Ok(result);
    }
    
}