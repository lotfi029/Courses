namespace Courses.Business.Entities;

public class UserModuleItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    public TimeSpan? Duration { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public Guid ModuleItemId {  get; set; }
    public ModuleItem ModuleItem { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
    
}
