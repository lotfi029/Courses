using Courses.Business.Contract.UploadFile;
using Courses.Business.Settings;

namespace Courses.Business.Contract.Lesson;
public class LessonRequestValidator : AbstractValidator<LessonRequest>
{
    public LessonRequestValidator()
    {
        RuleFor(e => e.Title)
            .Length(3, 400);

        RuleFor(e => e.File)
            .SetValidator(new SignatureValidator())
            .Must(e =>
            {
                var extension = Path.GetExtension(e.FileName);
                return FileSettings.AllowedVideoExtensions.Contains(extension);
            })
            .WithMessage(FileSettings.VideoErrorMessage);
    }
}
