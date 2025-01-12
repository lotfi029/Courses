namespace Courses.Business.Errors;

public class UserExamErrors
{
    public static readonly Error DuplicatedAnswer
        = new(nameof(DuplicatedAnswer), "you have already taken the exam and passed it.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidSubmitExam
        = new(nameof(InvalidSubmitExam), "to submit the exam you must be enrolled to this exam.", StatusCodes.Status404NotFound);

    public static readonly Error ExamNotAvailable
        = new(nameof(ExamNotAvailable), "this exam is not available now.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidEnrollment
        = new(nameof(InvalidEnrollment), "You have not enrolled in this exam.", StatusCodes.Status400BadRequest);
}