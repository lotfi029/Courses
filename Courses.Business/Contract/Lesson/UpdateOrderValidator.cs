namespace Courses.Business.Contract.Lesson;

public sealed class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
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