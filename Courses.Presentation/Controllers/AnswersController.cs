using Courses.Business.Contract.Answer;

namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnswersController(IAnswerService answerService) : ControllerBase
{
    private readonly IAnswerService _answerService = answerService;

    [HttpPost("{examId:int}")]
    public async Task<IActionResult> EnrollExamAsync([FromRoute] int examId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _answerService.EnrollExamAsync(examId, userId, cancellationToken);

        return result.IsSuccess ?  Ok() : result.ToProblem();
    }

    [HttpPost("submit-answer/{examId:int}")]
    public async Task<IActionResult> SubmitAnswer([FromRoute] int examId,[FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _answerService.AddAnswer(examId, userId, request.Answers, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
