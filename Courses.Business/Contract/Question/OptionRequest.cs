namespace Courses.Business.Contract.Question;

public record OptionRequest(
    IList<OptionValue> Options
    );
