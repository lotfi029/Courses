namespace Courses.Business.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsDisable { get; set; } = true;
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<Option> Options { get; set; } = [];
    public ICollection<ExamQuestion> ExamQuestions { get; set; } = [];
}
