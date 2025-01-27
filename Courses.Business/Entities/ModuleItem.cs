using Courses.Business.Abstract.Enums;

namespace Courses.Business.Entities;

public class ModuleItem
{
    public int Id { get; set; }
    public Guid ModuleId { get; set; }
    public int OrderIndex { get; set; }
    public CourseModule Module { get; set; } = default!;
    public ModuleItemType ItemType { get; set; }
    public Guid? GuidItemId { get; set; }
    public int? IntItemId { get; set; }
}