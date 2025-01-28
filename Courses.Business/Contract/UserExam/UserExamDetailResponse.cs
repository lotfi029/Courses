namespace Courses.Business.Contract.UserExam;

public record UserExamDetailResponse(
    Guid Id,
    string Title,
    string Description,
    TimeSpan Duratin,
    int NoQuestions,
    int NoTime,
    IEnumerable<UserExamResponse> UserExams
    );
