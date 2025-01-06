namespace Courses.Business.Errors;

public class RoleErrors
{
    public static readonly Error InvalidName
        = new(nameof(InvalidName), "select another name this name is exist.", StatusCodes.Status400BadRequest);
    public static readonly Error NotFound
        = new(nameof(NotFound), "this role not founded.", StatusCodes.Status404NotFound);
    public static readonly Error InvalidPermission
        = new(nameof(InvalidPermission), "invalid permission", StatusCodes.Status400BadRequest);
    public static readonly Error BadRequest
        = new(nameof(BadRequest), "invalid operation check the name or permissions is invalid", StatusCodes.Status400BadRequest);
}