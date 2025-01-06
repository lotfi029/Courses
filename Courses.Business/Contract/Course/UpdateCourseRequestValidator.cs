namespace Courses.Business.Contract.Course;

public sealed class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .Length(3, 400);

        RuleFor(e => e.Description)
            .NotEmpty()
            .Length(3, 1000);

        RuleFor(e => e.Level)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(e => e.Price)
            .NotEmpty();
    }
}