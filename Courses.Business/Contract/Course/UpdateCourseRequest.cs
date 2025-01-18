using Courses.Business.Contract.UploadFile;

namespace Courses.Business.Contract.Course;

public record UpdateCourseRequest (
    string Title,
    string Description,
    string Level,
    double Price
);