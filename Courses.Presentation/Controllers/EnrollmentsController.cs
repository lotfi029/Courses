using FFmpeg.AutoGen;

namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService = enrollmentService;

    [HttpPost("{courseId:guid}")]
    [Authorize]
    public async Task<IActionResult> Enroll([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.EnrollToCourseAsync(courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{courseId:guid}/lesson/{lessonId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetLesson([FromRoute] Guid lessonId, [FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.GetLessonAsync(lessonId, courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{courseId:guid}")]
    [Authorize]
    public async Task<IActionResult> Get([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.GetCourseAsync(courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("my-courses")]
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.GetMyCoursesAsync(userId, cancellationToken);

        return Ok(result);
    }
    [HttpGet("all-courses")]
    public async Task<IActionResult> GetAllCourse(CancellationToken cancellationToken)
    {
        var result = await _enrollmentService.GetAllCoursesAsync(cancellationToken);

        return Ok(result);
    }
}
