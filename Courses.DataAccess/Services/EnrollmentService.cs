using Courses.Business.Contract.Category;
using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;

namespace Courses.DataAccess.Services;
public class EnrollmentService(ApplicationDbContext context) : IEnrollmentService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> EnrollToCourseAsync(
        Guid courseId, 
        string userId, 
        CancellationToken cancellationToken = default)
    {

        if (await _context.Courses.FindAsync([courseId], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.IsPublished)
            return EnrollmentErrors.CourseNotAvailable;

        if (!await _context.Users.AnyAsync(e => e.Id == userId, cancellationToken))
            return UserErrors.InvalidCredinitails;

        var hasEnroll = await _context.UserCourses
           .AnyAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken);

        if (hasEnroll)
            return EnrollmentErrors.DuplicatedEnrollment;

        UserCourse userCourse = new()
        {
            UserId = userId,
            CourseId = courseId,
        };
        await _context.UserCourses.AddAsync(userCourse, cancellationToken);

        await AddUserLessonsAsync(courseId, userId, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    private async Task<Result> AddUserLessonsAsync(
        Guid courseId, 
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var modules = await _context.Modules
            .Where(e => e.CourseId == courseId)
            .ToListAsync(cancellationToken);

        if (modules is null)
            return Result.Success();
        List<UserLesson> userLessons = [];
        foreach (var module in modules)
        {
            var lessons = await _context.Lessons
                .Where(e => e.ModuleId == module.Id)
                .Select(e => e.Id)
                .ToListAsync(cancellationToken);

            if (lessons is not null)
            {
                foreach (var lesson in lessons)
                    userLessons.Add(new()
                    {
                        UserId = userId,
                        LessonId = lesson,
                    });
            }
        }
        await _context.AddRangeAsync(userLessons, cancellationToken);
        
        return Result.Success();
    }
    public async Task<Result<CourseDetailedResponse>> GetAsync(
        Guid courseId, 
        string userId, 
        CancellationToken cancellationToken = default)
    {
        if (!await _context.UserCourses.AnyAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken))
            return Result.Failure<CourseDetailedResponse>(EnrollmentErrors.NotFoundEnrollment);

        var query = await (
            from c in _context.Courses
            join cCat in _context.CourseCategories
            on c.Id equals cCat.CourseId into cc
            from cCats in cc.DefaultIfEmpty()
            join cats in _context.Categories
            on cCats.CategoryId equals cats.Id into cats
            from categories in cats.DefaultIfEmpty()
            join uc in _context.UserCourses.Where(e => e.UserId == userId)
            on c.Id equals uc.CourseId into ucs
            from userCourse in ucs.DefaultIfEmpty()
            where c.Id == courseId
            select new {
                c.Id,
                c.Title,
                c.Description,
                c.Level,
                c.Rating,
                c.Duration,
                c.ThumbnailId,
                userCourseId =userCourse.Id,
                userCourse.CompleteStatus,
                userCourse.LastInteractDate,
                userCourse.FinshedDate,
                Tags = c.Tags.Select(t => t.Title),
                Categories = categories
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var course = query
            .GroupBy(c => new {
                c.Id,
                c.Title,
                c.Description,
                c.Level,
                c.Rating,
                c.Duration,
                c.ThumbnailId,
                c.userCourseId,
                c.CompleteStatus,
                c.LastInteractDate,
                c.FinshedDate,
            })
            .Select(c => new {
                c.Key.Id,
                c.Key.Title,
                c.Key.Description,
                c.Key.Level,
                c.Key.ThumbnailId,
                c.Key.Duration,
                c.Key.Rating,
                c.Key.userCourseId,
                c.Key.CompleteStatus,
                c.Key.LastInteractDate,
                c.Key.FinshedDate,
                Tags = c.SelectMany(e => e.Tags).ToList(),
                Categories = c.Select(e => e.Categories).Adapt<List<CategoryResponse>>()

            }).Single();

        var userLessons = await _context.UserLessons
            .Where(ul => ul.UserId == userId)
            .Join(_context.Lessons,
                ul => ul.LessonId,
                l => l.Id,
                (ul, l) => new
                {
                    l.ModuleId,
                    LessonResponse = new UserLessonResponse( 
                        l.Id,
                        l.Title,
                        l.FileId,
                        l.Duration,
                        ul.IsComplete,
                        ul.LastWatchedTimestamp,
                        ul.StartDate,
                        ul.LastInteractDate,
                        ul.FinshedDate,
                        l.Resources.Adapt<List<RecourseResponse>>()
                    )
                })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        
        var lessonsByModule = userLessons
            .GroupBy(ul => ul.ModuleId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.LessonResponse).ToList());

        var modules = await _context.Modules
            .Where(m => m.CourseId == courseId)
            .Select(m => new UserModuleResponse(
                m.Id,
                m.Title,
                m.Description,
                m.Duration,
                lessonsByModule.ContainsKey(m.Id) ? lessonsByModule[m.Id] : new List<UserLessonResponse>()
            ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var respons = new CourseDetailedResponse(
            course.Id,
            course.Title,
            course.Description,
            course.Level,
            course.ThumbnailId,
            course.Duration,
            course.Rating,
            course.userCourseId,
            course.CompleteStatus,
            course.LastInteractDate,
            course.FinshedDate,
            modules,
            course.Tags,
            course.Categories
            );

        return Result.Success(respons);
    }
    
    
    // TODO:
    // How to update lastvideowtched 
    // how to update resume video
    // how to record the progress of the view
    // how to calculate the duration of the video

}
