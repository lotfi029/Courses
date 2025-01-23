namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService = enrollmentService;

    [HttpPost("{courseId:guid}")]
    public async Task<IActionResult> Enroll([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.EnrollToCourseAsync(courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{courseId:guid}/lesson/{lessonId:guid}")]
    public async Task<IActionResult> GetLesson([FromRoute] Guid lessonId, [FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.GetLessonAsync(lessonId, courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("{courseId:guid}/complete")]
    public async Task<IActionResult> CompleteLesson([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.CompleteCourseAsync(courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{courseId:guid}/lesson/{lessonId:guid}/completed")]
    public async Task<IActionResult> CompleteLesson([FromRoute] Guid lessonId, [FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.CompleteLessonAsync(lessonId, courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{courseId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.GetCourseAsync(courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("my-courses")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.GetMyCoursesAsync(userId, cancellationToken);

        return Ok(result);
    }
}
