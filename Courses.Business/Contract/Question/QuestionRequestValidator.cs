namespace Courses.Business.Contract.Question;

public sealed class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(e => e.Text)
            .NotEmpty()
            .Length(10, 450);
    }
}