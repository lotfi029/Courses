using Microsoft.AspNetCore.Identity;

namespace Courses.Business.Entities;
public class ApplicationRole : IdentityRole
{
    public bool IsDefualt { get; set; }
    public bool IsDeleted { get; set; }
}
