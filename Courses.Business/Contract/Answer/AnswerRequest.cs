namespace Courses.Business.Contract.Answer;

public record AnswerRequest(
    IList<AnswerValues> Answers
    );
