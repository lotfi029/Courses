using Courses.Business.Contract.UploadFile;
using Courses.Business.Settings;

namespace Courses.Business.Contract.Lesson;
public class LessonRequestValidator : AbstractValidator<LessonRequest>
{
    public LessonRequestValidator()
    {
        RuleFor(e => e.Title)
            .Length(3, 400);

        RuleFor(e => e.Video)
            .SetValidator(new UploadVideoRequestValidator());
    }
}
