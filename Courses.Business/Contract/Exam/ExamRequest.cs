namespace Courses.Business.Contract.Exam;
public record ExamRequest (
    string Title,
    string Description,
    TimeSpan Duration
    );

public record QuestionExamRequest(
    IEnumerable<int> QuestionIds
    );

public sealed class QuestionExamRequestValidator : AbstractValidator<QuestionExamRequest>
{
    public QuestionExamRequestValidator()
    {
        RuleFor(e => e.QuestionIds)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.QuestionIds)
            .Must(e =>
            {
                return e.Count() == e.Distinct().Count()
                       && e.Any();
            }).WithMessage("the question must be distinct and not be empty")
            .When(e => e.QuestionIds is not null);

    }
}
public record OptionResponse(
    int Id, 
    string Text,
    bool IsCorrect
    );


public record ExamResponse(
    int Id,
    string Title,
    string Description,
    TimeSpan Duration,
    IList<QuestionResponse>? Questions,
    float? Score
    );