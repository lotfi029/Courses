using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;

namespace Courses.Business.IServices;
public interface ILessonService
{
    Task<Result<Guid>> AddAsync(Guid moduleId, string userId, LessonRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, LessonRequest request, string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateLessonOrderAsync(Guid moduleId, Guid id, string userId, int newOrder, CancellationToken cancellationToken = default);
    Task<Result> AddResourceAsync(Guid id, RecourseRequest recourses, string userId, CancellationToken cancellationToken = default);
    Task<Result> ToggleIsPreviewAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<Result<LessonResponse>> GetAsync(Guid id, string? userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LessonResponse>> GetAllAsync(Guid moduleId, string? userId, CancellationToken cancellationToken = default);

}
