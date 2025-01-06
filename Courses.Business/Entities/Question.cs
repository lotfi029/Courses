namespace Courses.Business.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ExamId { get; set; }
    public float Points { get; set; }
    public Exam Exam { get; set; } = default!;
    public virtual ICollection<Option> Options { get; set; } = [];
}
