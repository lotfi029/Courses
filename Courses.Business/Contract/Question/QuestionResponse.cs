namespace Courses.Business.Contract.Question;

public record QuestionResponse(
    int Id, 
    string Text,
    bool IsDisable,
    IEnumerable<OptionResponse> Options
    );
