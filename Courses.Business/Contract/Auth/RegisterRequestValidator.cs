using FluentValidation;

namespace Courses.Business.Contract.Auth;
public sealed partial class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.UserName)
            .Length(1, 100)
            .Must((_, context, _) =>
            {
                return ValidateRegexs.IsUserNameValid(context);
            })
            .WithMessage("{PropertyName} {PropertyValue} is invalid, can only contain (a-z), (A_Z), (0-9), or (._!@#$)");

        RuleFor(e => e.Password)
            .SetValidator(new ValidatePassword<string>());
    }

}