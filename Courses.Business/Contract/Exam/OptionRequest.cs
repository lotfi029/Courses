namespace Courses.Business.Contract.Exam;

public record OptionRequest(
    IList<OptionValue> Options
    );
