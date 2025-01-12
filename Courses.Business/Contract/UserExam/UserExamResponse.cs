namespace Courses.Business.Contract.UserExam;
public record UserExamResponse(
    int Id,
    TimeSpan? UserDuration,
    DateTime StartDate,
    DateTime? EndDate,
    float Score,
    string UserId,
    IEnumerable<UserQuestionResponse> Question
);
