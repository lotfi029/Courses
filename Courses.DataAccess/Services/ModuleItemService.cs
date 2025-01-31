namespace Courses.DataAccess.Services;
public class ModuleItemService(ApplicationDbContext context) : IModuleItemService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result> UpdateModuleItemIndexAsync(Guid moduleId, Guid moduleItemId, string userId, int newIndex, CancellationToken cancellationToken = default)
    {
        
        var moduleItems = await _context.ModuleItems
            .Where(e => e.ModuleId == moduleId && e.CreatedById == userId)
            .OrderBy(e => e.OrderIndex)
            .ToListAsync(cancellationToken);

        if (moduleItems is null || moduleItems.Count == 0)
            return ModuleErrors.InvalidReOrderOperation;

        var moduleItem = moduleItems.SingleOrDefault(e => e.Id == moduleItemId);

        if (moduleItem is null)
            return ModuleErrors.ModuleItemNotFound;

        var moduleIndex = moduleItem.OrderIndex;

        if (newIndex == moduleIndex)
            return Result.Success();

        newIndex = Math.Clamp(newIndex, 1, moduleItems.Count);

        
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {   
                moduleItem.OrderIndex = -1;
                await _context.SaveChangesAsync(cancellationToken);

                if (newIndex < moduleIndex)
                {
                    foreach (var item in moduleItems)
                    {
                        if (item.Id == moduleItem.Id)
                            item.OrderIndex = newIndex;
                        else if (item.OrderIndex >= newIndex && item.OrderIndex < moduleIndex)
                            item.OrderIndex += 1;
                    }
                }
                else
                {
                    foreach (var item in moduleItems)
                    {
                        if (item.Id == moduleItem.Id)
                            item.OrderIndex = newIndex;
                        else if (item.OrderIndex > moduleIndex && item.OrderIndex <= newIndex)
                            item.OrderIndex -= 1;
                    }
                }

                var hasDuplicates = moduleItems.GroupBy(e => e.OrderIndex).Any(g => g.Count() > 1);
                if (hasDuplicates)
                    throw new InvalidOperationException("Duplicate OrderIndex detected.");

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new Error(nameof(ex),$"An error occurred while updating the order: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

        return Result.Success();
    }
}
