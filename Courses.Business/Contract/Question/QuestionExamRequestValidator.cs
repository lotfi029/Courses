namespace Courses.Business.Contract.Question;

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
