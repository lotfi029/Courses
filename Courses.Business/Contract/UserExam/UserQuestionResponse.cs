namespace Courses.Business.Contract.UserExam;

public record UserQuestionResponse(
    int Id,
    string Text,
    bool Correct,
    int SelecedOptionId,
    IList<UserOptionResponse> Options
    );
