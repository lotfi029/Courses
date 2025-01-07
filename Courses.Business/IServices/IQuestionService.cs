using Courses.Business.Contract.Exam;

namespace Courses.Business.IServices;
public interface IQuestionService
{
    Task<Result<int>> AddQuestionsAsync(string userId, Guid courseId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateQuestionsAsync(int id, Guid courseId, string userId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleIsDisableAsync(int questionId, Guid courseId, string userId, CancellationToken cancellationToken = default);
    Task<Result> RemoveQuestionAsync(int id, Guid courseId, string userId, CancellationToken cancellationToken = default);
    Task<Result> AddQuestionOptionsAsync(int questionId, Guid courseId, string userId, OptionRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateQuestionOptionsAsync(int questionId, Guid courseId, string userId, OptionRequest request, CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> GetQuestionAsync(int questionId, CancellationToken cancellationToken = default);
    Task<Result<List<QuestionResponse>>> GetAllQuestionAsync(Guid courseId, CancellationToken cancellationToken = default);
}
