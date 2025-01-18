using Courses.Business.Settings;

namespace Courses.Business.Contract.UploadFile;

public class UploadVideoRequestValidator : AbstractValidator<IFormFile>
{
    public UploadVideoRequestValidator()
    {
        RuleFor(e => e)
            .SetValidator(new SignatureValidator())
            .Must(e =>
            {
                var extension = Path.GetExtension(e.FileName);
                return FileSettings.AllowedVideoExtensions.Contains(extension);
            })
            .WithMessage(FileSettings.VideoErrorMessage);
    }
}