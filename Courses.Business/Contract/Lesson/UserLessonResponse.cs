namespace Courses.Business.Contract.Lesson;

public record UserLessonResponse (
    Guid Id,
    string Title,
    Guid FileId,
    TimeSpan Duration,
    bool? IsComplete,
    TimeSpan? LastWatchedTimestamp,
    DateTime? StartDate,
    DateTime? LastInteractDate,
    DateTime? FinshedDate,
    ICollection<RecourseResponse> Resources
    );