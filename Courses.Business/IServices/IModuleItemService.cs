namespace Courses.Business.IServices;

public interface IModuleItemService
{
    Task<Result> UpdateIndexLessonAsync(Guid moduleId, Guid lessonId, string userId, int newIndex, CancellationToken cancellationToken = default);
    Task<Result> UpdateIndexExamAsync(Guid moduleId, int examId, string userId, int newIndex, CancellationToken cancellationToken = default);
}