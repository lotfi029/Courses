using Courses.Business.Contract.Question;

namespace Courses.Business.Contract.Exam;

public record ExamResponse(
    Guid Id,
    string Title,
    string Description,
    TimeSpan Duration,
    bool? IsDisable,
    IEnumerable<QuestionResponse> Questions = default!
    );

