using Courses.Business.Contract.Lesson;

namespace Courses.Business.Contract.Module;

public record UserModuleResponse(
    Guid Id,
    string Title,
    string Description,
    TimeSpan Duration,
    ICollection<UserModuleItemResponse> ModuleItems
);
