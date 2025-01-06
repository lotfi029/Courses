using System.Runtime.CompilerServices;

namespace Courses.Business.Contract.Auth.Common;
public sealed class ValidatePassword<T> 
    : AbstractValidator<T>
    where T : class

{
    public ValidatePassword()
    {
        RuleFor(e => e)
            .Must(ValidPassword);
    }
    private static bool ValidPassword(T _, T password, ValidationContext<T> context)
    {
        if (password is not string pass)
            return false;

        bool isValid = ValidateRegexs.IsPasswordValid(pass);
        if (isValid)
            return true;

        if (!pass.Any(char.IsLower))
            context.AddFailure("Passwords must have at least one lowercase ('a'-'z').");

        if (!pass.Any(char.IsUpper))
            context.AddFailure("Passwords must have at least one uppercase ('A'-'Z').");

        if (!pass.Any(char.IsDigit))
            context.AddFailure("Passwords must have at least one digit ('0'-'9').");

        if (!Regex.Match(pass, @"^(?=.*[^a-zA-Z0-9]).+$").Success)
            context.AddFailure("Passwords must have at least one non alphanumeric character.");

        if (pass.Length < 8)
            context.AddFailure("Passwords must be at least 8 characters.");
        return true;
    }
}