using Microsoft.VisualBasic;

namespace Courses.Business.Entities;

public class UserLesson
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public Guid LessonId { get; set; }
    public Guid UserCourseId { get; set; }
    public bool IsComplete { get; set; } = false;
    public TimeSpan? LastWatchedTimestamp { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime LastInteractDate { get; set; } = DateTime.UtcNow;
    public DateTime? FinshedDate { get; set; }
    public Lesson Lesson { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
    public UserCourse UserCourse { get; set; } = default!;
}