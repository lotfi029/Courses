using Courses.Business.Contract.Category;
using Courses.Business.Contract.Module;

namespace Courses.Business.Contract.Course;
public record UserCourseResponse (
    Guid Id, 
    string Title,
    string Description,
    string Level,
    string Thumbnail,
    TimeSpan? Duration,
    float Rating,
    float Progress,
    Guid UserCourseId,
    bool IsCompleted,
    DateTime LastInteractDate,
    DateTime? FinshedDate,
    ICollection<UserModuleResponse>? Modules,
    ICollection<CategoryResponse>? Categories
);
