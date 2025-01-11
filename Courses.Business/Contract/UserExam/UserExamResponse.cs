namespace Courses.Business.Contract.UserExam;
public record UserExamDetailResponse(
    int Id,
    string Title,
    string Description,
    TimeSpan Duratin,
    int NoQuestions,
    IEnumerable<UserExamResponse> UserExams
    );



public record UserExamResponse(
    int Id,
    TimeSpan? UserDuration,
    DateTime StartDate,
    DateTime? EndDate,
    float Score,
    IEnumerable<UserQuestionResponse> Question
);
