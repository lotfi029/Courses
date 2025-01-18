namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService = enrollmentService;

    [HttpPost("")]
    public async Task<IActionResult> Enroll([FromHeader] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.EnrollToCourseAsync(courseId, userId, cancellationToken);


        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{courseId}")]
    public async Task<IActionResult> Get([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _enrollmentService.GetAsync(courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
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
