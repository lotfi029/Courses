using Courses.Business.Settings;

namespace Courses.Business.Contract.Lesson;

public sealed class RecourseRequestValidator : AbstractValidator<RecourseRequest>
{
    public RecourseRequestValidator()
    {
        RuleFor(e => e.Key)
            .Length(3, 100);

        RuleFor(e => e.Value)
            .Length(3, 100);

        RuleFor(e => e.File)
            .Must(ValidateFile);    
    }
    private static bool ValidateFile(RecourseRequest _, IFormFile? file, ValidationContext<RecourseRequest> context)
    {
        if (file is null)
            return true;

        if (file.Length > FileSettings.MaxFileSizeInByte)
        {
            context.AddFailure(FileSettings.SizeErrorMessage);
            return false;
        }

        BinaryReader binary = new(file.OpenReadStream());
        var bytes = binary.ReadBytes(2);

        var fileSignatureHex = BitConverter.ToString(bytes);

        if (FileSettings.BlockedSignatures.Contains(fileSignatureHex))
        {
            context.AddFailure(FileSettings.ContentTypeErrorMessage);
            return false;
        }
        
        return true;
    }
}