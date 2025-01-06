namespace Courses.Business.Contract.User;
public record UserProfileResponse (
    string FirstName,
    string LastName,
    string? Level,
    float? Rating,
    DateOnly? DateOfBirth
    );
