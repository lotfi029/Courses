namespace Courses.Business.Contract.Role;

public sealed class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty();

        //RuleFor(e => e.Permissions)
        //    .NotNull().WithMessage("This field is required.")
        //    .NotEmpty();

        RuleFor(e => e.Permissions)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Must(e => e.Distinct().Count() == e.Count)
            .WithMessage("{PropertyName} is not be duplicated.")
            .When(e => e.Permissions != null).WithMessage("this must not be null");
    }
}
