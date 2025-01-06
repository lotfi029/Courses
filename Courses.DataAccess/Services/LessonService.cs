using Courses.Business.Contract.Lesson;
using Courses.Business.Entities;

namespace Courses.DataAccess.Services;
public class LessonService(
    ApplicationDbContext context,
    IFileService fileService) : ILessonService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<Guid>> AddAsync(Guid moduleId, string userId, LessonRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _context.Modules.AnyAsync(e => e.CreatedById == userId && e.Id == moduleId, cancellationToken))
            return Result.Failure<Guid>(ModuleErrors.NotFound);

        if (await _context.Lessons.AnyAsync(e => e.Title == request.Title && e.ModuleId == moduleId, cancellationToken))
            return Result.Failure<Guid>(LessonErrors.DuplicatedTitle);

        Lesson lesson = new()
        {
            Title = request.Title,
            ModuleId = moduleId,
            FileId = await _fileService.UploadAsync(request.File, cancellationToken)
        };

        await _context.Lessons.AddAsync(lesson, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(lesson.Id);
    }
    
    public async Task<Result> UpdateAsync(Guid id, LessonRequest request, string userId, CancellationToken cancellationToken = default)
    {
        //if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
        //    return Result.Failure(LessonErrors.NotFound);

        //if (lesson.CreatedById != userId)
        //    return Result.Failure(UserErrors.UnAutherizeUpdate);

        //lesson = request.Adapt(lesson);

        //await _context.SaveChangesAsync(cancellationToken);
        // TODO:
        var result = await _context.Lessons
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters
                //.SetProperty(e => e.Resources, request.Resources)
                .SetProperty(e => e.Title, request.Title)
                .SetProperty(e => e.Order, request.Order),
                //.SetProperty(e => e.VideoUrl, request.VideoUrl),
                cancellationToken
            );

        if (result == 0)
            return LessonErrors.NotFound;

        return Result.Success();
    }
    public async Task<Result> AddResourceAsync(Guid id, RecourseRequest recourse, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
            return Result.Failure(LessonErrors.NotFound);

        if (lesson.CreatedById != userId)
            return Result.Failure(UserErrors.UnAutherizeUpdate);

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
            return Result.Failure(LessonErrors.NotFound);

        if (lesson.CreatedById != userId)
            return Result.Failure(UserErrors.UnAutherizeUpdate);

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

    public async Task<IEnumerable<LessonResponse>> GetAllAsync(Guid moduleId,string? userId, CancellationToken cancellationToken = default)
    {
        var lessons = await _context.Lessons
            .Where(e => e.ModuleId == moduleId)
            .ProjectToType<LessonResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return lessons;
    }
}