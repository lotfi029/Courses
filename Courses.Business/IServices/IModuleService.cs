using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;

namespace Courses.Business.IServices;
public interface IModuleService
{
    Task<Result<Guid>> AddAsync(string userId,Guid courseId,ModuleRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, ModuleRequest request, string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateOrderAsync(Guid id, string userId, UpdateOrderRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<Result<ModuleResponse>> GetAsync(Guid id, string? userId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<ModuleResponse>> GetAllAsync(Guid courseId, string? userId = null, CancellationToken cancellationToken = default);
}
