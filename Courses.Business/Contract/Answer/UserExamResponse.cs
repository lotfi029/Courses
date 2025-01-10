namespace Courses.Business.Contract.Answer;
public record UserExamResponse(
    int Id,
    string Title,
    string Description,
    TimeSpan Duratin,
    TimeSpan? UserDuration,
    DateTime StartDate,
    DateTime? EndDate,
    float Score,
    IList<UserQuestionResponse> Question
    );
