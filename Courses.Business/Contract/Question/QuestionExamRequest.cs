namespace Courses.Business.Contract.Question;

public record QuestionExamRequest(
    IEnumerable<int> QuestionIds
    );
