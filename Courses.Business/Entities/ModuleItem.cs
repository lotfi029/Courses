using Courses.Business.Abstract.Enums;

namespace Courses.Business.Entities;

public class ModuleItem : AuditableEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDisable { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid ModuleId { get; set; }
    public int OrderIndex { get; set; }
    public CourseModule Module { get; set; } = default!;
    public ModuleItemType ItemType { get; set; }
}
    
