using Courses.Business.Contract.Category;
using Courses.Business.Contract.Module;

namespace Courses.Business.Contract.Course;

public record CourseResponse (
    Guid Id, 
    string Title,
    string Description,
    string Level,
    Guid ThumbnailId,
    TimeSpan? Duration,
    float Rating,
    bool IsPublished,
    double Price,
    int NoStudent,
    int NoCompleted,
    IEnumerable<string> Tags,
    IEnumerable<CategoryResponse> Categories,
    IEnumerable<ModuleResponse> Modules
);