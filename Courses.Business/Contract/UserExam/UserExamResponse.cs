namespace Courses.Business.Contract.UserExam;
public record UserExamResponse(
    Guid Id,
    TimeSpan? UserDuration,
    DateTime StartDate,
    DateTime? EndDate,
    float Score,
    string UserId,
    IEnumerable<UserQuestionResponse> Question
);
