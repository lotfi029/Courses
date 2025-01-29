namespace Courses.Business.Entities;
public class Lesson : ModuleItem   
{
    public Guid FileId { get; set; }
    public bool IsPreview { get; set; }
    public UploadedFile File { get; set; } = default!;
    public ICollection<Recourse>? Resources { get; set; } = [];
    public ICollection<UserLesson>? UserLessons { get; set; } = [];
}
