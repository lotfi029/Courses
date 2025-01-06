namespace Courses.Business.Contract.User;

public record ResetPasswordRequest (
    string Email,
    string ResetToken, 
    string NewPassword
    );
