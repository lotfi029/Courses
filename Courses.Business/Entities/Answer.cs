namespace Courses.Business.Entities;

public class Answer
{
    public int Id { get; set; }
    public int UserExamId { get; set; }
    public int QuestionId { get; set; }
    public int OptionId { get; set; }

    public UserExam UserExam { get; set; } = default!;
    public Question Question { get; set; } = default!;
    public Option Option { set; get; } = default!;
}