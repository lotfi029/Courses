namespace Courses.Business.Entities;
public class UserModule
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ModuleId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public float Progress { get; set; }
    public bool IsComplete { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime LastInteractDate { get; set; } = DateTime.UtcNow;
    public DateTime? FinshedDate { get; set; }
    public CourseModule Module { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}
