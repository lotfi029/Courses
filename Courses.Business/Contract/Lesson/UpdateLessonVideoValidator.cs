using Courses.Business.Contract.UploadFile;

namespace Courses.Business.Contract.Lesson;

public sealed class UpdateLessonVideoValidator : AbstractValidator<UpdateLessonVideoRequest>
{
    public UpdateLessonVideoValidator()
    {
        RuleFor(e => e.Video)
            .SetValidator(new UploadVideoRequestValidator());
    }
};