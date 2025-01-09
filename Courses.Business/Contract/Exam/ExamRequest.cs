namespace Courses.Business.Contract.Exam;
public record ExamRequest (
    string Title,
    string Description,
    TimeSpan Duration
    );
