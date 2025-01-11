namespace Courses.Business.Contract.UserExam;

public record UserOptionResponse(
    int Id,
    string Text,
    bool IsCorrect
    );
