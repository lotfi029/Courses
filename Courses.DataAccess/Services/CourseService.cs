using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;
using Courses.Business.Contract.UploadFile;
using Courses.Business.Contract.User;

namespace Courses.DataAccess.Services;
public partial class CourseService(
    ApplicationDbContext context,
    IFileService fileService,
    IEnrolmentService enrollmentService) : ICourseService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IFileService _fileService = fileService;
    private readonly IEnrolmentService _enrollmentService = enrollmentService;

    public async Task<Result<Guid>> AddAsync(AddCourseRequest request, CancellationToken cancellationToken = default)
    {
        var course = request.Adapt<Course>();

        foreach (var id in course.CourseCategories)
            id.CourseId = course.Id;

        course.Thumbnail = request.Thumbnail.FileName;

        //course.Tags = tags;

        await _context.AddAsync(course, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(course.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string userId, UpdateCourseRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.FindAsync([id], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        course = request.Adapt(course);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateThumbnailAsync(Guid id, string userId, UploadImageRequest request,CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.FindAsync([id], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        var imageId = await _fileService.UploadAsync(request.Image, cancellationToken);

        //course.ThumbnailId = imageId;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleIsPublishAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {

        var course = await _context.Courses            
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (course is null)
            return CourseErrors.NotFound;

        if (course.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        var modules = await _context.Modules
            .Where(e => e.CourseId == id)
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        if (modules is null || modules.Count == 0)
            return ModuleErrors.NotFound;
        
        var lesson = await _context.Lessons
            .Where(e => modules.Contains(e.ModuleId))
            .Select(e => e.IsPreview)
            .ToListAsync(cancellationToken);

        int preViewCnt = lesson.Select(e => e == true).Count();
        
        if (preViewCnt < 2)
            return CourseErrors.CourseNotValidToPublish;

        course.IsPublished = !course.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> AssignCourseToCategoryAsync(Guid id, string userId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.FindAsync([id], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        if (!await _context.Categories.AnyAsync(e => e.Id == categoryId, cancellationToken))
            return Result.Failure(CategoryErrors.NotFound);

        if (await _context.CourseCategories.AnyAsync(e => e.CourseId == id && e.CategoryId == categoryId, cancellationToken))
            return Result.Failure(CourseErrors.CourseDuplicatedCategory);
        
        await _context.AddAsync(new CourseCategories
        {
            CategoryId = categoryId,
            CourseId = id,
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    
    public async Task<Result> UnAssignCourseToCategoriesAsync(Guid id, string userId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.FindAsync([id], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        if (await _context.CourseCategories.SingleOrDefaultAsync(e => e.CourseId == id && e.CategoryId == categoryId, cancellationToken) is not { } courseCategory)
            return CourseErrors.InvalidCategory;

        _context.Remove(courseCategory);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    
    public async Task<Result> BlockedUserAsync(Guid id, string userId, UserIdentifierRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.FindAsync([id], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        var result = await _enrollmentService.GetByCourseIdAsync(id, request.Id, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        var userCourse = result.Value!;

        if (userCourse.IsBlocked)
            return CourseErrors.InvalidUserBlock;

        userCourse.IsBlocked = true;

        _context.UserCourses.Update(userCourse);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UnBlockedUserAsync(Guid id, string userId, UserIdentifierRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.FindAsync([id], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.CreatedById != userId)
            return UserErrors.UnAutherizeAccess;

        var result = await _enrollmentService.GetByCourseIdAsync(id, request.Id, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        var userCourse = result.Value!;

        if (!userCourse.IsBlocked)
            return CourseErrors.InvalidUserUnBlock;

        userCourse.IsBlocked = false;

        _context.UserCourses.Update(userCourse);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    // Get Methods
    public async Task<Result<CourseResponse>> GetAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {

        var course = await _context.Courses
            .Where(e => e.Id == id && e.CreatedById == userId)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (course is null)
            return Result.Failure<CourseResponse>(CourseErrors.NotFound);

        var modules = await (
            from m in _context.Modules
            join l in _context.Lessons
            on m.Id equals l.ModuleId into lessons
            where m.CourseId == id
            select new ModuleResponse(
                m.Id,
                m.Title,
                m.Description,
                m.Duration,
                lessons.Adapt<List<LessonResponse>>()
                )
            ).ToListAsync(cancellationToken);

        (int NoCompleted, int NoEnrollment) = await _enrollmentService.GetCourseInfoAsync(id, cancellationToken);

        var response = (course, modules, NoCompleted, NoEnrollment).Adapt<CourseResponse>();

        return Result.Success(response);
    }
    public async Task<IEnumerable<UserResponse>> GetUsersInCourseAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        var response = await _enrollmentService.GetUsersInCourseAsync(id, cancellationToken);

        return response;
    }
    public async Task<IEnumerable<CourseResponse>> GetAllAsync(string userId, CancellationToken cancellationToken = default)
    {
        var courses = await _context.Courses
            .Where(e => e.CreatedById == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (courses is null)
            return [];

        var response = courses.Adapt<List<CourseResponse>>();

        return response;
    }

}