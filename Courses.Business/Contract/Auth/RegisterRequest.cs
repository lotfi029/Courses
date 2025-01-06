namespace Courses.Business.Contract.Auth;

public record RegisterRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string UserName,
    string Password
);