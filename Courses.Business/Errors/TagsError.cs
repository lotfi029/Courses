namespace Courses.Business.Errors;

public class TagsError
{
    public static readonly Error NotFound
        = new("Tag.NotFound", "Tag not found", StatusCodes.Status404NotFound);
}