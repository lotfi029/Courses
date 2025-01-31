namespace Courses.Business.Entities;

public class UserLesson : UserModuleItem
{
    public Guid UserCourseId { get; set; }
    public TimeSpan? LastWatchedTimestamp { get; set; }
    public DateTime LastInteractDate { get; set; } = DateTime.UtcNow;
    public UserCourse UserCourse { get; set; } = default!;
}