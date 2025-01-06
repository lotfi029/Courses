using Courses.Business.Contract.Exam;

namespace Courses.Business.IServices;
public interface IExamService
{
    Task<Result<int>> AddAsync(Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, Guid moduleId, string userId, ExamRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleAsync(int id, string userId, CancellationToken cancellationToken = default);
    Task<Result<ExamResponse>> GetExamAsync(int id, string userId, CancellationToken cancellationToken = default);
    Task<Result> AddQuestionsAsync(int examId, string userId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateQuestionsAsync(int id, int examId, string userId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> RemoveQuestionAsync(int id, int examId, string userId, CancellationToken cancellationToken = default);
    Task<Result> AddOptionAsync(int questionId, int examId, string userId, IEnumerable<OptionRequest> request, CancellationToken cancellationToken = default);
    Task<Result> UpdateOptionAsync(int id, int examId, string userId, OptionRequest request, CancellationToken cancellationToken = default);
    Task<Result> RemoveOptionAsync(int id, int examId, string userId, CancellationToken cancellationToken = default);
}
