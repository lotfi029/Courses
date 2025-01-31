namespace Courses.Business.Errors;

public class ModuleErrors
{
    public static readonly Error NotFound
        = new(nameof(NotFound), "Module not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedTitle
        = new(nameof(DuplicatedTitle), "Another module with same title", StatusCodes.Status409Conflict);

    public static readonly Error InvalidReOrderOperation
        = new(nameof(InvalidReOrderOperation), "Invalid Re Order Operation", StatusCodes.Status400BadRequest);
    
    public static readonly Error ModuleItemNotFound
        = new(nameof(ModuleItemNotFound), "this item is not found", StatusCodes.Status404NotFound);
}
