namespace Courses.Business.Entities;

public class UserExam
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid ExamId { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float Score { get; set; }
    public TimeSpan? Duration { get; set; }
    //public int ExamTimes { get; set; }
    public ApplicationUser User { get; set; } = default!;
    public Exam Exam {  set; get; } = default!;
    public ICollection<Answer> UserAnswers { get; set; } = [];
}

public class UserModuleItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? FinshedDate { get; set; }
}
//public class UserLesson
//{
//    public Guid Id { get; set; } = Guid.CreateVersion7();
//    public string UserId { get; set; } = string.Empty;
//    public Guid LessonId { get; set; }
//    public Guid UserCourseId { get; set; }
//    
//    public TimeSpan? LastWatchedTimestamp { get; set; }
//    
//    public DateTime LastInteractDate { get; set; } = DateTime.UtcNow;
//    
//    public Lesson Lesson { get; set; } = default!;
//    public ApplicationUser User { get; set; } = default!;
//    public UserCourse UserCourse { get; set; } = default!;
//}