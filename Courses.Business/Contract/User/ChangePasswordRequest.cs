namespace Courses.Business.Contract.User;

public record ChangePasswordRequest(
    string NewPassword,
    string CurrentPassword
    );
