using Courses.Business.Settings;

namespace Courses.Business.Contract.UploadFile;

public sealed class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
    public UploadImageRequestValidator()
    {
        RuleFor(e => e.Image)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new SignatureValidator());

        RuleFor(e => e.Image)
            .Must(e =>
            {
                var extension = Path.GetExtension(e.FileName.ToLower());

                return FileSettings.AllowedImageExtensions.Contains(extension);
            })
            .WithMessage(FileSettings.ImageErrorMessage)
            .When(e => e is not null);
    }
}
