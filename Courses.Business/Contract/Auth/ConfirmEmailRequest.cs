namespace Courses.Business.Contract.Auth;

public record ConfirmEmailRequest(string UserId, string Code);
