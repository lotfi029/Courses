namespace Courses.Business.Contract.Course;

public record UpdateCourseRequest (
    string Title,
    string Description,
    string ThumbnailUrl,
    string Level,
    double Price
);
