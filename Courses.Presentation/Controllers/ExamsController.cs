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
    [HttpPost("{examId:int}/add-question")]
    public async Task<IActionResult> AddQuestion([FromRoute] int examId, [FromBody]  QuestionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        
        var result = await _examService.AddQuestionsAsync(examId, userId, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPost("{examId:int}/add-question/{questionId:int}/add-options")]
    public async Task<IActionResult> AddOption([FromRoute] int questionId, [FromRoute] int examId, [FromBody] List<OptionRequest> request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.AddOptionAsync(questionId, examId, userId, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();

    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetExam([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.GetExamAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
