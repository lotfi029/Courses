using System.ComponentModel.DataAnnotations;

namespace Courses.Business.Contract.User;

public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    
    public ResetPasswordRequestValidator()
    {
        RuleFor(e => e.NewPassword)
            .SetValidator(new ValidatePassword<string>());

        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("email/userName is requird.")
            .Must(e =>
            {
                var isEmail = new EmailAddressAttribute().IsValid(e);

                if (isEmail)
                    return true;

                return ValidateRegexs.IsUserNameValid(e);
            }).WithMessage("email/userName is invalid.");

        RuleFor(e => e.ResetToken)
            .NotEmpty();
    }
}
