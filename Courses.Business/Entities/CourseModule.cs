using Courses.Business.Abstract.Enums;

namespace Courses.Business.Entities;
public class CourseModule : AuditableEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsDisable { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<Lesson> Lessons { get; set; } = [];
    public ICollection<Exam>? Exams { get; set; } = [];
    public ICollection<ModuleItem> ModuleItems { get; set; } = [];
}
public class ModuleItem
{
    public int Id { get; set; }
    public Guid ModuleId { get; set; }
    public int Order { get; set; }
    public ModuleItemType ItemType { get; set; }

    public CourseModule Module { get; set; } = default!;
}