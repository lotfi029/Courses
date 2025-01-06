using Courses.Business.Contract.Role;

namespace Courses.Business.IServices;
public interface IRoleService
{
    Task<Result<string>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<RoleDetailsResponse>> GetAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisable = false, CancellationToken cancellationToken = default);
}
