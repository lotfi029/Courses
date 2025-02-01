using Courses.Business.Contract.Question;

namespace Courses.Presentation.Controllers;
[Route("api/{courseId:guid}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpPost("add-question")]
    [HasPermission(Permissions.AddQuestion)]
    public async Task<IActionResult> AddQuestion([FromRoute] Guid courseId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _questionService.AddQuestionsAsync(userId, courseId, request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(GetQuestion), new {courseId, id = result.Value}, null) : result.ToProblem();
    }
    [HttpPost("{questionId:int}/add-options")]
    [HasPermission(Permissions.UpdateQuestion)]
    public async Task<IActionResult> AddOptions([FromRoute] int questionId, [FromRoute] Guid courseId, [FromBody] OptionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _questionService.AddQuestionOptionsAsync(questionId, courseId, userId, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
    [HttpPut("{id:int}")]
    [HasPermission(Permissions.UpdateQuestion)]
    public async Task<IActionResult> UpdateQuestion([FromRoute] int id, [FromRoute] Guid courseId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _questionService.UpdateQuestionsAsync(id, courseId, userId, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{questionId:int}/toggle-status")]
    [HasPermission(Permissions.UpdateQuestion)]
    public async Task<IActionResult> ToggleIsDisable([FromRoute] int questionId, [FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _questionService.ToggleIsDisableAsync(questionId, courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.UpdateQuestion)]
    public async Task<IActionResult> RemoveQuestion([FromRoute] int id, [FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _questionService.RemoveQuestionAsync(id, courseId, userId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{questionId:int}/update-options")]
    [HasPermission(Permissions.UpdateQuestion)]
    public async Task<IActionResult> UpdateOptions([FromRoute] int questionId, [FromRoute] Guid courseId, [FromBody] OptionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _questionService.UpdateQuestionOptionsAsync(questionId, courseId, userId, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:int}")]
    [HasPermission(Permissions.GetQuestion)]
    public async Task<IActionResult> GetQuestion([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var result = await _questionService.GetQuestionAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    [HasPermission(Permissions.GetQuestion)]
    public async Task<IActionResult> GetAll([FromRoute] Guid courseId, CancellationToken cancellationToken = default)
    {
        var result = await _questionService.GetAllQuestionAsync(courseId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
