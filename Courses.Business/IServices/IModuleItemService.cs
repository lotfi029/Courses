namespace Courses.Business.IServices;

public interface IModuleItemService
{
    Task<Result> UpdateModuleItemIndexAsync(Guid moduleId, Guid moduleItemId, string userId, int newIndex, CancellationToken cancellationToken = default);
}