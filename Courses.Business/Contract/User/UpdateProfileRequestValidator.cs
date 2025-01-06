namespace Courses.Business.Contract.User;
public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty();

        RuleFor(e => e.LastName)
            .NotEmpty();

        RuleFor(e => e.Level)
            .NotEmpty();

        RuleFor(e => e.DateOfBirth)
            .NotNull()
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-10)));
    }
}
