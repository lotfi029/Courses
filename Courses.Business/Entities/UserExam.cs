namespace Courses.Business.Entities;

public class UserExam : UserModuleItem
{
    public float Score { get; set; }
    public ICollection<Answer> UserAnswers { get; set; } = [];
}
