namespace Courses.Business.Contract.Tag;

public sealed class TagsRequestValidator : AbstractValidator<TagsRequest>
{
    public TagsRequestValidator()
    {
        RuleFor(e => e.Tags)
            .NotEmpty()
            .Must(e => e.Count == e.Distinct().Count())
            .WithMessage("Tags must not be distinct.")
            .When(e => e.Tags is not null);
    }
}