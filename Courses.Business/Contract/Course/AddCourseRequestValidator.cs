using Courses.Business.Contract.UploadFile;

namespace Courses.Business.Contract.Course;

public sealed class AddCourseRequestValidator : AbstractValidator<AddCourseRequest>
{
    public AddCourseRequestValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .Length(3, 400);
        
        RuleFor(e => e.Description)
            .NotEmpty()
            .Length(3, 1000);
        
        RuleFor(e => e.Level)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(e => e.Price)
            .NotEmpty();

        RuleFor(e => e.Thumbnail)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new SignatureValidator())
            .SetValidator(new ImageExtensionValidator());

    }
}