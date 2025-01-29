namespace Courses.Business.Contract.Lesson;

public sealed class UpdateLessonValidator : AbstractValidator<UpdateLessonRequest>
{
    public UpdateLessonValidator()
    {
        RuleFor(e => e.Title)
            .Length(3, 400);
        
        RuleFor(e => e.Description)
            .Length(10, 1000);
    }
}
