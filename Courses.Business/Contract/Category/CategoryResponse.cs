namespace Courses.Business.Contract.Category;
public record CategoryResponse (
    Guid Id,
    string Title,
    string Description
);