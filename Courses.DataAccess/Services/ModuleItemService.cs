using Courses.Business.Abstract.Enums;
using Courses.Business.Entities;

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

        if (moduleItems is null)
            return ModuleErrors.InvalidReOrderOperation;

        var moduleItem = await _context.Lessons.SingleOrDefaultAsync(e => e.Id == moduleItemId,cancellationToken);

        if (moduleItem is null)
            return ModuleErrors.ModuleItemNotFound;

        var moduleIndex = moduleItem.OrderIndex;

        var itemCount = moduleItems.Count;

        if (newIndex == moduleIndex)
            return Result.Success();

        if (newIndex > itemCount)
            newIndex = itemCount;

        else if (newIndex < 1)
            newIndex = 1;

        if (newIndex < moduleIndex)
        {
            foreach (var item in moduleItems)
            {
                if (item.Id == moduleItem.Id)
                    item.OrderIndex = newIndex;

                else if (item.OrderIndex >= newIndex)
                    item.OrderIndex += 1;
            }
        }
        else
        {
            foreach (var item in moduleItems)
            {

                if (item.Id == moduleItem.Id)
                    item.OrderIndex = newIndex;

                else if (item.OrderIndex >= moduleIndex)
                    item.OrderIndex -= 1;
            }
        }

        var indexs = moduleItems.Select(e => e.OrderIndex);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
