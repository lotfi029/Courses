namespace Courses.Business.Contract.Lesson;

public sealed class UpdateOrderValidator : AbstractValidator<UpdateIndexRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(e => e.Index)
            .NotEmpty()
            .GreaterThan(0);
    }
}

//public record UpdateLessonVideoRequest (
//    IFormFile File
//);
// TODO: 