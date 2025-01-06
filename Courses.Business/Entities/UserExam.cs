namespace Courses.Business.Entities;

public class UserExam
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ExamId { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float Score { get; set; }

    public ApplicationUser User { get; set; } = default!;
    public Exam Exam {  set; get; } = default!;
    public ICollection<UserAnswer> UserAnswers { get; set; } = [];
}
