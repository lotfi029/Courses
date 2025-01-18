namespace Courses.Business.Contract.Lesson;

public sealed class LessonTitleUpdateValidator : AbstractValidator<UpdateLessonTitleRequest>
{
    public LessonTitleUpdateValidator()
    {
        RuleFor(e => e.Title)
            .Length(3, 400);
    }
}

//public record UpdateLessonVideoRequest (
//    IFormFile File
//);
// TODO: 