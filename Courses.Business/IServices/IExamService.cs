using Courses.Business.Contract.Exam;
using Courses.Business.Contract.UserExam;

namespace Courses.Business.IServices;
public interface IExamService
{
    Task<Result<int>> AddAsync(Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default);
    Task<Result> AddExamQuestionsAsync(int id, Guid moduleId, string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, string userId, CancellationToken cancellationToken = default);
    Task<Result<ExamResponse>> GetAsync(int id, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExamResponse>> GetModuleExamsAsync(Guid id, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserExamDetailResponse>> GetUserExams(Guid moduleId, string studentId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserExamResponse>> GetExamUsersAsync(int examId, string userId, CancellationToken cancellationToken = default);
    Task<Result> RemoveExamQuestionsAsync(int id, Guid moduleId, string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);
}
