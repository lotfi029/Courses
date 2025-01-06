using Microsoft.EntityFrameworkCore;

namespace Courses.Business.Entities;

[Owned]
public sealed record Recourse
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; } = string.Empty;
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
    public Guid? FileId { get; set; }
    public UploadedFile? File { get; set; } = default!;
}