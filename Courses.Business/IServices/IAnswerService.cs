using Courses.Business.Contract.Answer;
using Courses.Business.Contract.Exam;
using Courses.Business.Contract.UserExam;

namespace Courses.Business.IServices;
public interface IAnswerService
{
    Task<Result> AddAnswer(Guid examId, string userId, IEnumerable<AnswerValues> request, CancellationToken cancellationToken = default);
    Task<Result<ExamResponse>> EnrollExamAsync(Guid examId, string userId, CancellationToken cancellationToken = default);
    Task<Result<ExamResponse>> ReEnrolExamAsync(Guid examId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserExamDetailResponse>> GetAllAsync(Guid courseId, string userId, CancellationToken cancellationToken);
    Task<Result<UserExamDetailResponse>> GetAsync(Guid examId, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserExamResponse>> GetExamUsersAsync(Exam exam, CancellationToken cancellationToken = default);
}
