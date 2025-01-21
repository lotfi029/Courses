namespace Courses.Business.Errors;

public class EnrollmentErrors
{
    public static readonly Error InvalidEnrollment 
        = new(nameof(InvalidEnrollment), "Invalid Enrollment", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidLessonComplete
        = new(nameof(InvalidLessonComplete), "watch lesson first to be able complete it", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedCourseEnrollment
        = new(nameof(DuplicatedCourseEnrollment), "you have enrollment already", StatusCodes.Status400BadRequest);
    
    public static readonly Error DuplicatedLessonEnrollment
        = new(nameof(DuplicatedLessonEnrollment), "you have enrollment already", StatusCodes.Status400BadRequest);

    public static readonly Error CourseNotAvailable
        = new(nameof(CourseNotAvailable), "course not available for enrollment", StatusCodes.Status400BadRequest);

    public static readonly Error NotFoundEnrollment
        = new(nameof(NotFoundEnrollment), "you have not an enrollment", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidAdminEnrollment
        = new(nameof(InvalidAdminEnrollment),"you are the instructor of this course and you can't be enrolled as user.",StatusCodes.Status400BadRequest);

    public static readonly Error BlockedEnrollment
        = new(nameof(BlockedEnrollment), "your enrollment is block please contact your adminstrator.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidGettingLesson
        = new(nameof(InvalidGettingLesson), "must be watched the prevouse lesson to be able to watch this lesson", StatusCodes.Status400BadRequest);
}