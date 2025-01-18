using System.Collections.Specialized;

namespace Courses.Business.Errors;

public class CourseErrors
{
    public static readonly Error NotFound 
        = new(nameof(NotFound),"course not found", StatusCodes.Status404NotFound);

    public static readonly Error CourseDuplicatedCategory
        = new(nameof(CourseDuplicatedCategory), "This Category is assigned already to this course.", StatusCodes.Status409Conflict);
    
    public static readonly Error InvalidCategory
        = new(nameof(InvalidCategory), "invalid category", StatusCodes.Status409Conflict);

    public static readonly Error InvalidUserBlock
        = new(nameof(InvalidUserBlock), "this user is already blocked.", StatusCodes.Status400BadRequest);
    
    public static readonly Error InvalidUserUnBlock
        = new(nameof(InvalidUserUnBlock), "this user is already Un blocked.", StatusCodes.Status400BadRequest);
}
