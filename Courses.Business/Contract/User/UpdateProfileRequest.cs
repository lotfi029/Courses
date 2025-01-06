namespace Courses.Business.Contract.User;
public record UpdateProfileRequest (
    string FirstName,
    string LastName,
    string? Level,
    DateOnly? DateOfBirth
    );
