namespace Courses.Business.Entities;
public class Exam : ModuleItem
{
    public int NoQuestion { get; set; }
    public ICollection<UserExam> UserExams { get; set; } = [];
    public ICollection<ExamQuestion> ExamQuestions { get; set; } = [];

}
