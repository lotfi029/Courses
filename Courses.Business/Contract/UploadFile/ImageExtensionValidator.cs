using Courses.Business.Settings;

namespace Courses.Business.Contract.UploadFile;

public sealed class ImageExtensionValidator : AbstractValidator<IFormFile>
{
    public ImageExtensionValidator()
    {
        RuleFor(e => e)
        .Must(e =>
        {
            var extension = Path.GetExtension(e.FileName.ToLower());

            return FileSettings.AllowedImageExtensions.Contains(extension);
        })
        .WithMessage(FileSettings.ImageErrorMessage)
        .When(e => e is not null);
    }
}