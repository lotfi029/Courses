using Courses.Business.Contract.Exam;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Question;

namespace Courses.Presentation.Controllers;
[Route("api/{moduleId:guid}/[controller]")]
[ApiController]
[Authorize]
public class ExamsController(
    IExamService examService,
    IModuleItemService moduleItemService) : ControllerBase
{
    private readonly IExamService _examService = examService;
    private readonly IModuleItemService _moduleItemService = moduleItemService;

    [HttpPost("")]
    public async Task<IActionResult> AddExam([FromRoute] Guid moduleId, [FromBody] ExamRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.AddAsync(moduleId, userId, request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(GetExam), new {moduleId, id = result.Value}, null) : result.ToProblem();
    }
    [HttpPost("{id:guid}/assign-questions")]
    public async Task<IActionResult> AssignQuestion([FromRoute] Guid id, [FromRoute] Guid moduleId, [FromBody] QuestionExamRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.AddExamQuestionsAsync(id, moduleId, userId, request.QuestionIds, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPut("{id:guid}/unassigned-questions")]
    public async Task<IActionResult> UnAssignedQuestion([FromRoute] Guid id, [FromRoute] Guid moduleId, [FromBody] QuestionExamRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.RemoveExamQuestionsAsync(id, moduleId, userId, request.QuestionIds, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/update-index")] 
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [FromRoute] Guid moduleId, [FromBody] UpdateIndexRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _moduleItemService.UpdateIndexExamAsync(moduleId, id, userId, request.Index, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/toggle-status")]
    public async Task<IActionResult> ToggleExam([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.ToggleAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExam([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.GetAsync(id, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] Guid moduleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.GetModuleExamsAsync(moduleId, userId, cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id:guid}/students")]
    public async Task<IActionResult> GetExamUsers([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.GetExamUsersAsync(id, userId, cancellationToken);

        return Ok(result);
    }
    [HttpGet("student-exams")]
    public async Task<IActionResult> GetExamUsers([FromRoute] Guid moduleId, [FromQuery] string studentId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _examService.GetUserExams(moduleId, studentId, userId, cancellationToken);

        return Ok(result);
    }
}
