using Courses.Business.Contract.Category;

namespace Courses.Business.IServices;
public interface ICategoryService
{
    Task<Result<Guid>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id,CategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryResponse>> GetAllAsync(bool iscludeDisable, CancellationToken cancellationToken = default);


}
