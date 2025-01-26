using Courses.Business.Abstract.Enums;
using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;

namespace Courses.DataAccess.Services;
public class LessonService(
    ApplicationDbContext context,
    IFileService fileService) : ILessonService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<Guid>> AddAsync(Guid moduleId, string userId, LessonRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Modules.FindAsync([moduleId], cancellationToken) is not { } module)
            return Result.Failure<Guid>(ModuleErrors.NotFound);

        if (module.CreatedById != userId)
            return Result.Failure<Guid>(UserErrors.UnAutherizeAccess);

        if (await _context.Lessons.AnyAsync(e => e.ModuleId == moduleId && request.Title == e.Title, cancellationToken))
            return Result.Failure<Guid>(LessonErrors.DuplicatedTitle);

        var prevOrder = await _context.ModuleItems
            .Where(e => e.ModuleId == moduleId)
            .MaxAsync(e => e.Order, cancellationToken);

        var moduleItem = new ModuleItem
        {
            ModuleId = moduleId,
            ItemType = ModuleItemType.Lesson,
            Order = prevOrder + 1,
        };

        await _context.ModuleItems.AddAsync(moduleItem, cancellationToken);

        var fileId = await _fileService.UploadAsync(request.Video, cancellationToken);

        Lesson lesson = new()
        {
            Title = request.Title,
            ModuleId = moduleId,
            FileId = fileId
        };

        await _context.Lessons.AddAsync(lesson, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(lesson.Id);
    }
    public async Task<Result> UpdateTitleAsync(Guid id, Guid moduleId, UpdateLessonTitleRequest request, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
            return LessonErrors.NotFound;

        if (lesson.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        if (await _context.Lessons.AnyAsync(e => e.Title == request.Title && e.ModuleId == moduleId && e.Id != id, cancellationToken))
            return LessonErrors.DuplicatedTitle;

        var result = await _context.Lessons
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.Title, request.Title),
                cancellationToken
            );

        return Result.Success();
    }
    public async Task<Result> UpdateOrderAsync(Guid moduleId ,Guid id, string userId, int newOrder, CancellationToken cancellationToken = default)
    {
        //var lessons = await _context.Lessons
        //    .Where(e => e.ModuleId == moduleId)
        //    .OrderBy(e => e.Order)
        //    .ToListAsync(cancellationToken);
        
        //if (lessons is null)
        //    return LessonErrors.NotFound;

        //int lessonCnt = lessons.Count;

        //var updatedLesson = lessons.SingleOrDefault(e => e.Id == id && e.CreatedById == userId);

        //if (updatedLesson == null)
        //    return LessonErrors.NotFound;

        //if (newOrder == updatedLesson.Order)
        //    return Result.Success();

        //var oldOrder = updatedLesson.Order;

        //if (newOrder > lessonCnt)
        //    newOrder = lessonCnt;
        //else if (newOrder < 1)
        //    newOrder = 1;

        //if (newOrder < oldOrder)
        //    foreach(var l in lessons)
        //    {
        //        if (l.Order <= oldOrder && l.Order >= newOrder)
        //        {
        //            if (l.Id == updatedLesson.Id)
        //                l.Order = newOrder;
        //            else 
        //                l.Order++;
        //        }
        //    }
        //else
        //    foreach (var l in lessons)
        //    {
        //        if (l.Order >= oldOrder && l.Order <= newOrder)
        //        {
        //            if (l.Id == updatedLesson.Id)
        //                l.Order = newOrder;
        //            else
        //                l.Order--;
        //        }
        //    }

        //_context.Lessons.UpdateRange(lessons);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateVideoAsync(Guid id, string userId, UpdateLessonVideoRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
            return LessonErrors.NotFound;

        if (lesson.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        // TODO: Remove the old video and upload the new 
        var fileId = await _fileService.UploadAsync(request.Video, cancellationToken);

        lesson.FileId = fileId;

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
    public async Task<Result> AddResourceAsync(Guid id, RecourseRequest recourse, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
            return Result.Failure(LessonErrors.NotFound);

        if (lesson.CreatedById != userId)
            return Result.Failure(UserErrors.UnAutherizeAccess);

        var addResources = new Recourse
        {
            Key = recourse.Key,
            Value = recourse.Value == "none" ? null : recourse.Value,
            FileId = recourse.File != null ? await _fileService.UploadAsync(recourse.File, cancellationToken) : null,
            LessonId = id
        };
        
        await _context.AddAsync(addResources, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> RemoveRecourseAsync(int id, CancellationToken cancellationToken = default)
    {
        await _context.Recourses
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleIsPreviewAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
            return LessonErrors.NotFound;

        if (lesson.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        lesson.IsPreview = !lesson.IsPreview;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result<LessonResponse>> GetAsync(Guid id, string? userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
            return Result.Failure<LessonResponse>(LessonErrors.NotFound);

        var response = lesson.Adapt<LessonResponse>();

        return Result.Success(response);
    }

    public async Task<IEnumerable<LessonResponse>> GetAllAsync(Guid moduleId, string? userId, CancellationToken cancellationToken = default)
    {
        var lessons = await _context.Lessons
            .Where(e => e.ModuleId == moduleId)
            .ProjectToType<LessonResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return lessons;
    }
}