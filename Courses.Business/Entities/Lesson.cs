namespace Courses.Business.Entities;
public class Lesson : AuditableEntity   
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public Guid ModuleId { get; set; }
    public Guid FileId { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsPreview { get; set; }
    public CourseModule Module { get; set; } = default!;
    public UploadedFile File { get; set; } = default!;
    public ICollection<Recourse>? Resources { get; set; } = [];
    public ICollection<UserLesson>? UserLessons { get; set; } = [];
}
