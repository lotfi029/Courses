﻿namespace Courses.Business.Errors;

public class ExamErrors
{
    public static readonly Error NotFoundExam
        = new(nameof(NotFoundExam), "Exam not found", StatusCodes.Status404NotFound);
    
    public static readonly Error DuplicatedTitle
        = new(nameof(DuplicatedTitle), "Duplicated title with same module", StatusCodes.Status400BadRequest);

    public static readonly Error ExamedNotAvailable
        = new(nameof(ExamedNotAvailable), "exam is disabled at this time.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidEnrollment
        = new(nameof(InvalidEnrollment), "your are the auther of this exam and you can't enroll this exam as student", StatusCodes.Status400BadRequest);
}
