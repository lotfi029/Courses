namespace Courses.Business.Contract.Category;
public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .Length(5, 450);
        
        RuleFor(e => e.Description)
            .NotEmpty()
            .Length(5, 100);
    }
}
