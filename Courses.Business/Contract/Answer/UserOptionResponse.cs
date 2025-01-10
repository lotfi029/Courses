namespace Courses.Business.Contract.Answer;

public record UserOptionResponse(
    int Id,
    string Text,
    bool IsCorrect
    );
