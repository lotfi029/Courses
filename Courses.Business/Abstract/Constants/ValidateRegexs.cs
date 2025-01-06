namespace Courses.Business.Abstract.Constants;
public static partial class ValidateRegexs
{
    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$")]
    public static partial Regex UserPasswordRegex();

    public static bool IsPasswordValid(string password)
    {
        return UserPasswordRegex().IsMatch(password);
    }
    
    [GeneratedRegex(@"^[a-zA-Z0-9._!@#$]+$")]
    public static partial Regex UserNameRegex();

    public static bool IsUserNameValid(string password)
    {
        return UserNameRegex().IsMatch(password);
    }
    
}
