namespace Courses.Business.Contract.Answer;

public sealed class AnswerRequestValidator : AbstractValidator<AnswerRequest>
{
    public AnswerRequestValidator()
    {
        RuleForEach (e => e.Answers)
            .SetValidator(new AnswerValuesValidator());

        RuleFor(e => e.Answers)
            .Must(e =>
            {
                return e.Count == e.Distinct().Count();
            }).WithMessage("answer must be unique per question")
            .When(e => e.Answers is not null);
    }
}
