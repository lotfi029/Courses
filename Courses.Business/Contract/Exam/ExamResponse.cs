using Courses.Business.Contract.Question;

namespace Courses.Business.Contract.Exam;

public record ExamResponse(
    int Id,
    string Title,
    string Description,
    TimeSpan Duration,
    IList<QuestionResponse>? Questions,
    float? Score
    );