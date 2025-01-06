using Microsoft.AspNetCore.Identity;

namespace Courses.Business.Entities;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Level {  get; set; } = string.Empty;
    public float? Rating { get; set; }
    public bool IsDisabled { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<UserCourse> UserCourses { get; set; } = [];
}
    