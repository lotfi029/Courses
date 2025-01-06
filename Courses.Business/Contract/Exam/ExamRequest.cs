namespace Courses.Business.Contract.Exam;
public record ExamRequest (
    string Title,
    string Description,
    TimeSpan Duration
    );

public record QuestionRequest (
    string Text,
    float Points
    );

public record OptionRequest(
    string Text,
    bool IsCorrect
    );

public record QuestionResponse(
    int Id, 
    string Text,
    float Points,
    IList<OptionResponse> Options,
    int? SelectedOptionId
    );

public record OptionResponse(
    int Id, 
    string Text,
    bool IsCorrect
    );


public record ExamResponse(
    int Id,
    string Title,
    string Description,
    float Degree,
    TimeSpan Duration,
    IList<QuestionResponse>? Questions,
    float? Score
    );