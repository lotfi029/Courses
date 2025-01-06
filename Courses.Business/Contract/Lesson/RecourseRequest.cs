namespace Courses.Business.Contract.Lesson;

public record RecourseRequest(
    string Key,
    IFormFile? File,
    string? Value
    );
