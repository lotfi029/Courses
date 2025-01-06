namespace Courses.Business.Contract.UploadFile;

public sealed class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequestValidator()
    {
        RuleFor(e => e.File)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new SignatureValidator());
    }
}
