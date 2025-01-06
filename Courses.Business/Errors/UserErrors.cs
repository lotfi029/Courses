namespace Courses.Business.Errors;
public class UserErrors
{
    public static readonly Error InvalidCredinitails
        = new(nameof(InvalidCredinitails), "Invalid Email/Password", StatusCodes.Status401Unauthorized);
    public static readonly Error UserIsDisable
        = new(nameof(UserIsDisable), "is disable", StatusCodes.Status401Unauthorized);
    public static readonly Error LockedUser
        = new(nameof(LockedUser), "Locked User", StatusCodes.Status401Unauthorized);
    public static readonly Error UserNotFound
        = new(nameof(UserNotFound), "User Not Found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidEmail
        = new("User.Dublicated", "Dublicated Email", StatusCodes.Status409Conflict);
    public static readonly Error InvalidToken 
        = new("User.InvalidToken", "Invalid Token", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidCode
        = new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
    public static readonly Error EmailConfirmed
        = new("User.EmailConfimed", "this email is confirmed before", StatusCodes.Status400BadRequest);
    
    public static readonly Error EmailNotConfirmed
        = new("User.EmailNotConfimed", "this email is Not confirmed.", StatusCodes.Status400BadRequest);
    public static readonly Error UnAutherizeUpdate
        = new("User.UnAutherizeUpdate", "unautherized user", StatusCodes.Status401Unauthorized);

    public static readonly Error UnAutherizeAdd
        = new("User.UnAutherizeAdd", "unautherized user", StatusCodes.Status401Unauthorized);
}
