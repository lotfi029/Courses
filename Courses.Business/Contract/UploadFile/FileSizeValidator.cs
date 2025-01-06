using Courses.Business.Settings;

namespace Courses.Business.Contract.UploadFile;

public sealed class FileSizeValidator : AbstractValidator<IFormFile>
{
    public FileSizeValidator()
    {
        RuleFor(e => e)
            .Must(e => e.Length <= FileSettings.MaxFileSizeInByte)
            .WithMessage(FileSettings.SizeErrorMessage)
            .When(e => e is not null).WithMessage("{PropertyName} is required");
    }
}
