using Courses.Business.Contract.Category;

namespace Courses.Business.Contract.Course;

public record RegularUserCourseResponse(
    Guid Id,
    string Title,
    string Description,
    string Level,
    Guid ThumbnailId,
    TimeSpan? Duration,
    IEnumerable<CategoryResponse> Categories,
    IEnumerable<string> Tags
    );