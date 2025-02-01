namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GuestCoursesController(IGuestCourseService userCourseResponse) : ControllerBase
{
    private readonly IGuestCourseService _userCourseResponse = userCourseResponse;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCourse(Guid id, CancellationToken cancellationToken)
    {
        var result = await _userCourseResponse.GetCourseAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _userCourseResponse.GetAllAsync(cancellationToken);

        return Ok(result);
    }
}
