namespace Courses.Business.Entities;

public class UserExam : UserModuleItem
{
    public float Score { get; set; }
    public ApplicationUser User { get; set; } = default!;
    public ICollection<Answer> UserAnswers { get; set; } = [];
}
