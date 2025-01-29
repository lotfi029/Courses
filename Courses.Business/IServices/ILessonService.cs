using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;

namespace Courses.Business.IServices;
public interface ILessonService
{
    Task<Result<Guid>> AddAsync(Guid moduleId, string userId, LessonRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateTitleAsync(Guid id, Guid moduleId, UpdateLessonRequest request, string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateVideoAsync(Guid id, string userId, UpdateLessonVideoRequest request, CancellationToken cancellationToken = default);
    Task<Result> AddResourceAsync(Guid id, RecourseRequest recourses, string userId, CancellationToken cancellationToken = default);
    Task<Result> ToggleIsPreviewAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<Result> ToggleIsDisableAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<Result<LessonResponse>> GetAsync(Guid id, string? userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LessonResponse>> GetAllAsync(Guid moduleId, string? userId, CancellationToken cancellationToken = default);
}
