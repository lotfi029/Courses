namespace Courses.Business.Contract.ModuleItem;

public record UserModuleItemResponse(
    Guid Id,
    string Title,
    TimeSpan Duration,
    bool IsComplete,
    DateTime StartDate,
    DateTime? EndDate,
    int ModuleItemType
    );