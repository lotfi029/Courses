using Courses.Business.Contract.Exam;
using Courses.Business.Contract.Lesson;

namespace Courses.Business.Contract.Module;
public record ModuleResponse(
    Guid Id,
    string Title,
    string Description,
    TimeSpan Duration,
    IList<LessonResponse> Lessons
);
public record TestModuleResponse(
    Guid Id,
    string Title,
    string Description,
    TimeSpan Duration,
    IList<LessonResponse> Lessons,
    IList<ExamResponse> Exams
);
