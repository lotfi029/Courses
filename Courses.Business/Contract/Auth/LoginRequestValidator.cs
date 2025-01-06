namespace Courses.Business.Contract.Auth;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty();

        RuleFor(e => e.Password)
               .SetValidator(new ValidatePassword<string>());
    }
}