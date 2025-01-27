namespace Courses.Business.Entities;
public class Exam : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDisable { get; set; }
    public TimeSpan Duration {  get; set; }
    public Guid ModuleId { get; set; }
    public int NoQuestion { get; set; }
    public CourseModule Module { get; set; } = default!;
    public ICollection<UserExam> UserExams { get; set; } = [];
    public ICollection<ExamQuestion> ExamQuestions { get; set; } = [];
}
