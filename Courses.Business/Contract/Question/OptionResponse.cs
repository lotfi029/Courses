namespace Courses.Business.Contract.Question;

public record OptionResponse(
    int Id, 
    string Text,
    bool? IsCorrect
    );
