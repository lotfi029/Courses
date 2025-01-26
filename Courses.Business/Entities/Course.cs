namespace Courses.Business.Entities;
public class Course : AuditableEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid ThumbnailId { get; set; }
    public string Level { get; set; } = string.Empty;
    public TimeSpan? Duration { get; set; }
    public double Price { get; set; }
    public float Rating { get; set; }
    public bool IsPublished { get; set; }
    public UploadedFile Thumbnail { get; set; } = default!;
    public ICollection<CourseCategories> CourseCategories { get; set; } = [];
    //public ICollection<Tag> Tags { get; set; } = [];
    public ICollection<CourseModule> Modules { get; set; } = []; 
    public ICollection<UserCourse> UserCourses { get; set; } = [];
    public ICollection<Exam>? Exams { get; set; } = [];
}
