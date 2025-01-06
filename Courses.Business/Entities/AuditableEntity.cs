namespace Courses.Business.Entities;

public class AuditableEntity
{
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ApplicationUser? UpdatedBy { get; set; }
    public ApplicationUser CreatedBy { get; set; } = default!;
}