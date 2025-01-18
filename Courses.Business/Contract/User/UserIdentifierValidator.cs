namespace Courses.Business.Contract.User;

public sealed class UserIdentifierValidator : AbstractValidator<UserIdentifierRequest>
{
    public UserIdentifierValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty()
            .WithMessage("the {PropertyName} must be not empty");
    }
}
