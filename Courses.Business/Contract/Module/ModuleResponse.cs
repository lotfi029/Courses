using Courses.Business.Contract.Lesson;

namespace Courses.Business.Contract.Module;
public record ModuleResponse(
    Guid Id,
    string Title,
    string Description,
    TimeSpan Duration,
    IList<LessonResponse> Lessons
);
