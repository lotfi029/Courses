namespace Courses.Business.Contract.Question;

public sealed class OptionRequestValidator : AbstractValidator<OptionRequest>
{
    public OptionRequestValidator()
    {
        RuleForEach(e => e.Options)
            .SetValidator(new OptionValueValidator())
            .When(e => e.Options is not null);

        RuleFor(e => e.Options)
            .Must(e =>
            {
                bool isValid = e.Select(e => e.Text).Distinct(StringComparer.OrdinalIgnoreCase).Count() == e.Select(e => e.Text).Count()
                              && e.Count(e => e.IsCorrect == true) == 1
                              && e.Count <= 5 && e.Count >= 2;

                if (isValid)
                    return true;

                return false;
            })
            .WithMessage("the {PropertyName} must be distinct, between 2 and 5 options and contain at exact one correct answers")
            .When(e => e.Options is not null);
    }
}
