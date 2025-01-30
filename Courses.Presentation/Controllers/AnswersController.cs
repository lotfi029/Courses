using Courses.Business.Contract.Answer;

namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnswersController(IAnswerService answerService) : ControllerBase
{
    private readonly IAnswerService _answerService = answerService;

    [HttpPost("{examId:guid}")]
    public async Task<IActionResult> EnrollExamAsync([FromRoute]Guid examId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _answerService.EnrollExamAsync(examId, userId, cancellationToken);

        return result.IsSuccess ?  Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("{examId:guid}/submit-answer")]
    public async Task<IActionResult> SubmitAnswer([FromRoute]Guid examId,[FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _answerService.AddAnswer(examId, userId, request.Answers, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("{examId:guid}/re-enrol")]
    public async Task<IActionResult> ReEnrolExam([FromRoute] Guid examId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _answerService.ReEnrolExamAsync(examId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{examId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid examId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _answerService.GetAsync(examId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{moduleId:guid}/in-course")]
    public async Task<IActionResult> GetAll([FromRoute] Guid moduleId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _answerService.GetAllAsync(moduleId, userId, cancellationToken); // TODO: Updte this to return right response

        return Ok(result);
    }
}
