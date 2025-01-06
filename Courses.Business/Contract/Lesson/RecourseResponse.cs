namespace Courses.Business.Contract.Lesson;

public record RecourseResponse (
    string Key,
    string? Value,
    Guid? FileId
    );