using Courses.Business.Settings;

namespace Courses.Business.Contract.UploadFile;

public sealed class SignatureValidator : AbstractValidator<IFormFile>
{
    public SignatureValidator()
    {
        RuleFor(e => e)
            .Must(e =>
            {
                BinaryReader binary = new(e.OpenReadStream());
                var bytes = binary.ReadBytes(2);

                var fileSequance = BitConverter.ToString(bytes);

                if (FileSettings.BlockedSignatures.Contains(fileSequance, StringComparer.OrdinalIgnoreCase))
                    return false;

                return true;
            }).WithMessage(FileSettings.ContentTypeErrorMessage)
            .When(e => e is not null);
    }
}
