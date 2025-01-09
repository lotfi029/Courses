namespace Courses.Business.Contract.Question;

public sealed class OptionValueValidator : AbstractValidator<OptionValue>
{
    public OptionValueValidator()
    {
        RuleFor(e => e.Text)
            .NotEmpty()
            .Length(10, 450);

        RuleFor(e => e.IsCorrect)
            .NotNull();
    }
}
