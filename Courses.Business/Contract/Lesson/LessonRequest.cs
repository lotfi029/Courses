namespace Courses.Business.Contract.Lesson;
public record LessonRequest(
    string Title,
    IFormFile File,
    int Order
);

//public record UpdateLessonVideoRequest (
//    IFormFile File
//);
//public record UpdateLessonTitleRequest (
//    string? Title,
//    int? Order
//);
// TODO: 