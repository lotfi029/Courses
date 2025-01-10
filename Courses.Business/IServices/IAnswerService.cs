using Courses.Business.Contract.Answer;

namespace Courses.Business.IServices;
public interface IAnswerService
{
    Task<Result> AddAnswer(int examId, string userId, IEnumerable<AnswerValues> request, CancellationToken cancellationToken = default);
    Task<Result> EnrollExamAsync(int examId, string userId, CancellationToken cancellationToken = default);
}
