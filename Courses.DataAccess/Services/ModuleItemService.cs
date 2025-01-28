
using Courses.Business.Abstract.Enums;
using Courses.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace Courses.DataAccess.Services;
public class ModuleItemService(ApplicationDbContext context) : IModuleItemService
{
    private readonly ApplicationDbContext _context = context;

    public  Task<Result> UpdateIndexExamAsync(Guid moduleId, Guid examId, string userId, int newIndex, CancellationToken cancellationToken = default)
    {
        //if (!await _context.Exams.AnyAsync(e => e.Id == examId, cancellationToken))
        //    return LessonErrors.NotFound;

        //var moduleItems = await _context.ModuleItems
        //    .Where(e => e.ModuleId == moduleId)
        //    .OrderBy(e => e.OrderIndex)
        //    .ToListAsync(cancellationToken);

        //var examItem = moduleItems.SingleOrDefault(e => e.ItemType == ModuleItemType.Exam && e.IntItemId == examId)!;

        //var result = await OrderAsync(moduleItems, examItem, newIndex, cancellationToken);

        //return result;

        throw new NotImplementedException();
    }

    public async Task<Result> UpdateIndexLessonAsync(Guid moduleId, Guid lessonId, string userId, int newIndex, CancellationToken cancellationToken = default)
    {
        //if (!await _context.Lessons.AnyAsync(e => e.Id == lessonId, cancellationToken))
        //    return LessonErrors.NotFound;

        //var moduleItems = await _context.ModuleItems
        //    .Where(e => e.ModuleId == moduleId)
        //    .OrderBy(e => e.OrderIndex)
        //    .ToListAsync(cancellationToken);

        //var lessonItem = moduleItems.SingleOrDefault(e => e.ItemType == ModuleItemType.Lesson && e.GuidItemId == lessonId)!;

        //var result = await OrderAsync(moduleItems, lessonItem, newIndex, cancellationToken);

        //return result;

        throw new NotImplementedException();

    }
    //private async Task<Result> OrderAsync(List<ModuleItem> moduleItems, ModuleItem moduleItem, int newIndex, CancellationToken cancellationToken = default)
    //{
    //    var moduleIndex = moduleItem.OrderIndex;

    //    var itemCount = moduleItems.Count;

    //    if (newIndex == moduleIndex)
    //        return Result.Success();

    //    if (newIndex > itemCount)
    //        newIndex = itemCount;
    //    else if (newIndex < 1)
    //        newIndex = 1;

    //    if (newIndex < moduleIndex)
    //    {
    //        foreach (var item in moduleItems)
    //        {
    //            if (item.Id == moduleItem.Id)
    //                item.OrderIndex = newIndex;

    //            else if (item.OrderIndex >= newIndex)
    //                item.OrderIndex += 1;
    //        }
    //    }
    //    else
    //    {
    //        foreach (var item in moduleItems)
    //        {
    //            if (item.Id == moduleItem.Id)
    //                item.OrderIndex = newIndex;

    //            else if (item.OrderIndex > moduleIndex)
    //                item.OrderIndex -= 1;
    //        }
    //    }

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Result.Success();
    //}
}
