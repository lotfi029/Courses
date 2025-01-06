namespace Courses.Business.Contract.User;

public record UserResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string? Level,
    float? Rating,
    DateOnly? DateOfBirth,
    IList<string> Roles
);