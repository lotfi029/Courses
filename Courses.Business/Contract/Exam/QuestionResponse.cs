namespace Courses.Business.Contract.Exam;

public record QuestionResponse(
    int Id, 
    string Text,
    bool IsDisable,
    IList<OptionResponse>? Options,
    int? SelectedOptionId
    );
