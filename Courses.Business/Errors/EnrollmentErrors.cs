namespace Courses.Business.Errors;

public class EnrollmentErrors
{
    public static readonly Error InvalidEnrollment
        = new(nameof(InvalidEnrollment), "Invalid Enrollment", StatusCodes.Status400BadRequest);
    
    public static readonly Error DuplicatedEnrollment
        = new(nameof(DuplicatedEnrollment), "you have enrollment already", StatusCodes.Status400BadRequest);

    public static readonly Error CourseNotAvailable
        = new(nameof(CourseNotAvailable), "course not available for enrollment", StatusCodes.Status400BadRequest);

    public static readonly Error NotFoundEnrollment
        = new(nameof(NotFoundEnrollment), "you have not an enrollment", StatusCodes.Status400BadRequest);

}