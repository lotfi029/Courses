namespace Courses.Business.Errors;
public class CategoryErrors
{
    public static readonly Error NotFound = new("Category.NotFound", "no category founded", StatusCodes.Status404NotFound);

}
