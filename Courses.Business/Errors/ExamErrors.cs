namespace Courses.Business.Errors;

public class ExamErrors
{
    public static readonly Error NotFoundExam
        = new(nameof(NotFoundExam), "Exam not found", StatusCodes.Status404NotFound);
    
    public static readonly Error DuplicatedTitle
        = new(nameof(DuplicatedTitle), "Duplicated title with same module", StatusCodes.Status400BadRequest);

}
public class UserExamErrors
{
    public static readonly Error DuplicatedAnswer
        = new(nameof(DuplicatedAnswer), "you passed this exam with more than 70% and you can't answer again", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidSubmitExam
        = new(nameof(InvalidSubmitExam), "to submit the exam you must be enrolled to this exam", StatusCodes.Status404NotFound);

    public static readonly Error ExamNotAvailable
        = new(nameof(ExamNotAvailable), "this exam is not available now", StatusCodes.Status400BadRequest);
}
public class QuestionErrors
{
    public static readonly Error InvalidQuestion
        = new(nameof(InvalidQuestion), "this question is disable or not exist", StatusCodes.Status400BadRequest);
    public static readonly Error NotFoundQuestion
        = new(nameof(NotFoundQuestion), "question not found", StatusCodes.Status404NotFound);

    public static readonly Error NotFoundOptions
        = new(nameof(NotFoundOptions), "option not found", StatusCodes.Status404NotFound);

    public static readonly Error NotFoundQuestionsToRemove
        = new(nameof(NotFoundQuestionsToRemove), "one or more question not found to remove it from exam", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedOptionsText
        = new(nameof(DuplicatedOptionsText), "this options already exist", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedOptions
        = new(nameof(DuplicatedOptions), "this question have already options", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidOptions
        = new(nameof(InvalidOptions), "options must have be at least 2 options or less than or 5 options and have one correct answer", StatusCodes.Status400BadRequest);
}