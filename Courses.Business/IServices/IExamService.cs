using Courses.Business.Contract.Exam;
using Courses.Business.Contract.UserExam;

namespace Courses.Business.IServices;
public interface IExamService
{
    Task<Result<Guid>> AddAsync(Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default);
    Task<Result> AddExamQuestionsAsync(Guid id, Guid moduleId, string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<Result<ExamResponse>> GetAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExamResponse>> GetModuleExamsAsync(Guid id, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserExamDetailResponse>> GetUserExams(Guid moduleId, string studentId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserExamResponse>> GetExamUsersAsync(Guid examId, string userId, CancellationToken cancellationToken = default);
    Task<Result> RemoveExamQuestionsAsync(Guid id, Guid moduleId, string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);
}
