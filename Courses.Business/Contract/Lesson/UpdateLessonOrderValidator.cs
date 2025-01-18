namespace Courses.Business.Contract.Lesson;

public sealed class UpdateLessonOrderValidator : AbstractValidator<UpdateLessonOrderRequest>
{
    public UpdateLessonOrderValidator()
    {
        RuleFor(e => e.Order)
            .NotEmpty()
            .GreaterThan(0);
    }
}

//public record UpdateLessonVideoRequest (
//    IFormFile File
//);
// TODO: 