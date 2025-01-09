namespace Courses.Business.Contract.Exam;

public sealed class ExamRequestValidator : AbstractValidator<ExamRequest>
{
    public ExamRequestValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .Length(10, 450);

        RuleFor(e => e.Description)
            .NotEmpty()
            .Length(100, 10000);

        RuleFor(e => e.Duration)
            .NotNull();
    }
}
