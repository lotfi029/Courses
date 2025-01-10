using Courses.Business.Contract.Answer;
using Courses.Business.Contract.Exam;

namespace Courses.Business.IServices;
public interface IAnswerService
{
    Task<Result> AddAnswer(int examId, string userId, IEnumerable<AnswerValues> request, CancellationToken cancellationToken = default);
    Task<Result<ExamResponse>> EnrollExamAsync(int examId, string userId, CancellationToken cancellationToken = default);
    Task<Result<ExamResponse>> ReEnrolExamAsync(int examId, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserExamResponse>> UserExamsAsync(int examId, string userId, CancellationToken cancellationToken);
}
