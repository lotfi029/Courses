using Courses.Business.Contract.Exam;

namespace Courses.Presentation.Controllers;
[Route("api/{moduleId:guid}/[controller]")]
[ApiController]
[Authorize]
public class ExamsController(IExamService examService) : ControllerBase
{
    private readonly IExamService _examService = examService;

    [HttpPost("")]
    public async Task<IActionResult> AddExam([FromRoute] Guid moduleId, [FromBody] ExamRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.AddAsync(moduleId, userId, request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(GetExam), new {moduleId, id = result.Value}, null) : result.ToProblem();
    }
    [HttpPost("assign-question/{id:int}")]
    public async Task<IActionResult> AssignQuestion([FromRoute] int id, [FromRoute] Guid moduleId, [FromBody] QuestionExamRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.AddExamQuestionsAsync(id, moduleId, userId, request.QuestionIds, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetExam([FromRoute] Guid moduleId,[FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.GetExamAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
