namespace Courses.Business.Errors;

public class ExamErrors
{
    public static readonly Error NotFoundExam
        = new(nameof(NotFoundExam), "Exam not found", StatusCodes.Status404NotFound);
    
    public static readonly Error DuplicatedTitle
        = new(nameof(DuplicatedTitle), "Duplicated title with same module", StatusCodes.Status400BadRequest);

}
public class QuestionErrors
{
    public static readonly Error InvalidQuestion
        = new(nameof(InvalidQuestion), "this question is disable or not exist", StatusCodes.Status400BadRequest);
    public static readonly Error NotFoundQuestion
        = new(nameof(NotFoundQuestion), "question not found", StatusCodes.Status404NotFound);

    public static readonly Error NotFoundOptions
        = new(nameof(NotFoundOptions), "option not found", StatusCodes.Status404NotFound);


    public static readonly Error DuplicatedOptionsText
        = new(nameof(DuplicatedOptionsText), "this options already exist", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedOptions
        = new(nameof(DuplicatedOptions), "this question have already options", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidOptions
        = new(nameof(InvalidOptions), "options must have be at least 2 options or less than or 5 options and have one correct answer", StatusCodes.Status400BadRequest);
}