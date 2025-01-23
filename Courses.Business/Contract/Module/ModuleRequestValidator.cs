namespace Courses.Business.Contract.Module;
public sealed class ModuleRequestValidator : AbstractValidator<ModuleRequest>
{
    public ModuleRequestValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .Length(3, 400);

        RuleFor(e => e.Description)
            .NotEmpty()
            .Length(3, 1000);

    }
}
