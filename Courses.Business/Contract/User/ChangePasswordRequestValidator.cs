namespace Courses.Business.Contract.User;

public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(e => e.NewPassword)
            .SetValidator(new ValidatePassword<string>());

        RuleFor(e => e.CurrentPassword)
            .SetValidator(new ValidatePassword<string>());

        RuleFor(e => e.NewPassword)
            .NotEqual(e => e.CurrentPassword);
    }
}
