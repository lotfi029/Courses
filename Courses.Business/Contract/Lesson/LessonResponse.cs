namespace Courses.Business.Contract.Lesson;
public record LessonResponse (
    Guid Id,
    string Title,
    Guid FileId,
    TimeSpan Duration,
    ICollection<RecourseResponse> Resources
    );
