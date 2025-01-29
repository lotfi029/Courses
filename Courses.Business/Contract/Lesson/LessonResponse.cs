namespace Courses.Business.Contract.Lesson;
public record LessonResponse (
    Guid Id,
    string Title,
    string Description,
    Guid FileId,
    TimeSpan Duration,
    bool IsDisable,
    bool IsPreview,
    ICollection<RecourseResponse> Resources
    );
