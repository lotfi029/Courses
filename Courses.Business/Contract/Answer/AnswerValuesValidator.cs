namespace Courses.Business.Contract.Answer;

public sealed class AnswerValuesValidator : AbstractValidator<AnswerValues>
{
    public AnswerValuesValidator()
    {
        RuleFor(e => e.QuestionId)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(e => e.OptionId)
            .NotEmpty()
            .GreaterThan(0);
    }
}