namespace Courses.Business.Contract.Lesson;
public record LessonRequest(string Title, string Description, IFormFile Video);
