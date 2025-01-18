namespace Courses.Business.Errors;

public class ModuleErrors
{
    public static readonly Error NotFound
        = new("Module.NotFound", "Module not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedTitle
        = new("Module.DuplicatedTitle", "Another module with same title", StatusCodes.Status409Conflict);
}
