namespace Courses.Business.Entities;

public class UserExam : UserModuleItem
{
    public float Score { get; set; }
    //public int ExamTimes { get; set; }
    public ApplicationUser User { get; set; } = default!;
    public ICollection<Answer> UserAnswers { get; set; } = [];
}

public class UserModuleItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    public TimeSpan? Duration { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public Guid ModuleItemId {  get; set; }
    public ModuleItem ModuleItem { get; set; } = default!;
}
//public class UserLesson
//{
//    public Guid UserCourseId { get; set; }
//    public TimeSpan? LastWatchedTimestamp { get; set; }
//    public DateTime LastInteractDate { get; set; } = DateTime.UtcNow;
//    public ApplicationUser User { get; set; } = default!;
//    public UserCourse UserCourse { get; set; } = default!;
//}