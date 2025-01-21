using Courses.Business.Contract.Category;
using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;
using Courses.Business.Contract.User;
using System.Reflection.Metadata;

namespace Courses.DataAccess.Services;
public class EnrollmentService(
    ApplicationDbContext context) : IEnrollmentService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> EnrollToCourseAsync(
        Guid courseId, 
        string userId, 
        CancellationToken cancellationToken = default)
    {

        if (await _context.Courses.FindAsync([courseId], cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        if (course.CreatedById == userId)
            return EnrollmentErrors.InvalidAdminEnrollment;

        if (!course.IsPublished)
            return EnrollmentErrors.CourseNotAvailable;

        if (!await _context.Users.AnyAsync(e => e.Id == userId, cancellationToken))
            return UserErrors.InvalidCredinitails;

        var hasEnroll = await _context.UserCourses
           .AnyAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken);

        if (hasEnroll)
            return EnrollmentErrors.DuplicatedCourseEnrollment;

        UserCourse userCourse = new()
        {
            UserId = userId,
            CourseId = courseId,
        };
        await _context.UserCourses.AddAsync(userCourse, cancellationToken);


        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<UserLessonResponse>> GetLessonAsync(Guid id, Guid courseId, string userId, CancellationToken cancellationToken = default)
    {
        var userCourse = await _context.UserCourses.SingleOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken);

        if (userCourse is null)
            return Result.Failure<UserLessonResponse>(EnrollmentErrors.NotFoundEnrollment);

        if (userCourse.IsBlocked)
            return Result.Failure<UserLessonResponse>(EnrollmentErrors.BlockedEnrollment);

        var userLessons = await _context.UserCourses
            .AsNoTracking()
            .Where(e => e.CourseId == courseId && userId == e.UserId)
            .ToListAsync(cancellationToken);

        var orderLessons = await (
            from m in _context.Modules
            join l in _context.Lessons
            on m.Id equals l.ModuleId into ls
            where m.CourseId == courseId
            select new
            {
                m.Id,
                m.Order,
                lessons = ls.OrderBy(e => e.Order).ToList()
            })
            .OrderBy(e => e.Order)
            .ToListAsync(cancellationToken);


        Lesson? nextLesson = null;
        Lesson? curLesson = null;
        foreach (var module in orderLessons)
        {
            foreach (var l in module.lessons)
            {
                if (userLessons.Select(e => e.Id).Contains(l.Id) && nextLesson is null)
                {
                    nextLesson = l;
                    break;
                }
            }

            curLesson = module.lessons.SingleOrDefault(e => e.Id == id);

            if (nextLesson is not null && curLesson is not null)
                break;
        }


        var userLesson = userCourse.UserLessons.SingleOrDefault(e => e.Id == id);
        
        userLesson ??= new UserLesson
        {
            LessonId = id,
            UserId = userId
        };

        Lesson lesson = new();
        
        userCourse.LastAccessLessonId = id;
        userCourse.LastInteractDate = DateTime.UtcNow;
        userCourse.LastWatchTimestamp = TimeSpan.Zero;

        userLesson.LastInteractDate = DateTime.UtcNow;
        userLesson.LastWatchedTimestamp = TimeSpan.Zero;

        await _context.SaveChangesAsync(cancellationToken);

        var response = (lesson, userLesson).Adapt<UserLessonResponse>();

        return Result.Success(response);
    }

    public async Task<Result<UserCourseResponse>> GetCourseAsync(
        Guid courseId, 
        string userId, 
        CancellationToken cancellationToken = default)
    {
        if (!await _context.UserCourses.AnyAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken))
            return Result.Failure<UserCourseResponse>(EnrollmentErrors.NotFoundEnrollment);

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
                userCourse.Progress,
                userCourseId = userCourse.Id,
                userCourse.IsCompleted,
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
                c.IsCompleted,
                c.LastInteractDate,
                c.FinshedDate,
                c.Progress
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
                c.Key.IsCompleted,
                c.Key.LastInteractDate,
                c.Key.FinshedDate,
                c.Key.Progress,
                Tags = c.SelectMany(e => e.Tags).ToList(),
                Categories = c.Select(e => e.Categories).Adapt<List<CategoryResponse>>()

            }).Single();

        var userLessons = await (
            from l in _context.Lessons
            join ul in _context.UserLessons
            on l.Id equals ul.LessonId into uls
            from ul in uls.DefaultIfEmpty()
            select new
            {
                l.ModuleId,
                LessonResponse = new UserLessonResponse(
                        l.Id,
                        l.Title,
                        l.FileId,
                        l.Duration,
                        ul == null ? null : ul.IsComplete,
                        ul.LastWatchedTimestamp ?? null,
                        ul == null ? null : ul.StartDate,
                        ul == null ? null : ul.LastInteractDate,
                        ul.FinshedDate ?? null,
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


        var respons = new UserCourseResponse(
            course.Id,
            course.Title,
            course.Description,
            course.Level,
            course.ThumbnailId,
            course.Duration,
            course.Rating,
            course.Progress,
            course.userCourseId,
            course.IsCompleted,
            course.LastInteractDate,
            course.FinshedDate,
            modules,
            course.Tags,
            course.Categories
            );

        return Result.Success(respons);
    }

    public async Task<Result> CompleteLessonAsync(Guid id, Guid courseId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.UserCourses.SingleOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken) is not { } userCourse)
            return EnrollmentErrors.NotFoundEnrollment;

        if (await _context.UserLessons.SingleOrDefaultAsync(e => e.LessonId == id && e.UserId == userId, cancellationToken) is not { } userLesson)
            return EnrollmentErrors.NotFoundEnrollment;

        if (userCourse.IsCompleted || userLesson.IsComplete)
            return Result.Success();

        var lessonCnt = await _context.Modules
            .Where(e => e.CourseId == courseId)
            .Join(_context.Lessons, m => m.Id, l => l.ModuleId, (m, l) => l.Id)
            .CountAsync(cancellationToken);

        var completeLesson = await _context.UserLessons
            .CountAsync(e => e.UserCourseId == userCourse.Id && e.UserId == userId, cancellationToken);
        var p = (float)(completeLesson + 1) / (float)lessonCnt;
        userCourse.Progress = p;

        userLesson.IsComplete = true;
        userLesson.FinshedDate = DateTime.UtcNow;
        userLesson.LastInteractDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> CompleteCourseAsync(Guid courseId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.UserCourses.Include(e => e.UserLessons).SingleOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken) is not { } userCourse)
            return EnrollmentErrors.NotFoundEnrollment;

        var userLessons = userCourse.UserLessons.ToList();

        return Result.Success();
    } 
    public async Task<IEnumerable<UserCourseResponse>> GetMyCoursesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var courses = await (
            from c in _context.Courses
            join cCat in _context.CourseCategories
            on c.Id equals cCat.CourseId into cCats
            from cCat in cCats.DefaultIfEmpty()
            join cats in _context.Categories 
            on cCat.Id equals cats.Id into categories
            from category in categories.DefaultIfEmpty()
            join uc in _context.UserCourses
            on c.Id equals uc.CourseId
            where uc.UserId == userId
            select new
            {
                c.Id,
                c.Title,
                c.Description,
                c.Level,
                c.ThumbnailId,
                c.Duration,
                c.Rating,
                uc.Progress,
                userCourseId = uc.Id,
                uc.IsCompleted,
                uc.LastInteractDate,
                uc.FinshedDate,
                category = category.Adapt<CategoryResponse>(),
                tags = c.Tags.Select(e => e.Title)
            })
            .GroupBy(x => new { x.Id, x.Title, x.Description, x.Level, x.ThumbnailId, x.Duration, x.Rating, x.Progress, x.userCourseId, x.IsCompleted, x.LastInteractDate, x.FinshedDate })
            .Select(c => new UserCourseResponse(
                c.Key.Id,
                c.Key.Title,
                c.Key.Description,
                c.Key.Level,
                c.Key.ThumbnailId,
                c.Key.Duration,
                c.Key.Rating,
                c.Key.Progress,
                c.Key.userCourseId,
                c.Key.IsCompleted,
                c.Key.LastInteractDate,
                c.Key.FinshedDate,
                null!,
                c.SelectMany(e => e.tags).ToList(),
                c.Select(e => e.category).ToList()
            ))
            .ToListAsync(cancellationToken);

        if (courses is null)
            return [];
            
        return courses;
    }
    
    // unauth + unsub
    public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(CancellationToken cancellationToken = default)
    {
        var courses = await (
            from c in _context.Courses
            join ccat in _context.CourseCategories
            on c.Id equals ccat.CourseId into ccats
            from cc in ccats.DefaultIfEmpty()
            join cat in _context.Categories
            on cc.CategoryId equals cat.Id into cat
            from cats in cat.DefaultIfEmpty()
            where c.IsPublished
            select new
            {
                c.Id,
                c.Title,
                c.Description,
                c.Level,
                c.ThumbnailId,
                c.Duration,
                c.Rating,
                c.IsPublished,
                c.Price,
                Tags = c.Tags.Select(e => e.Title),
                Categories = cats.Adapt<CategoryResponse>()
            })
            .GroupBy(x => new { x.Id, x.Title, x.Description, x.Level, x.ThumbnailId, x.Duration, x.Rating, x.IsPublished, x.Price, })
            .Select(c => new CourseResponse( 
                c.Key.Id,
                c.Key.Title,
                c.Key.Description,
                c.Key.Level,
                c.Key.ThumbnailId,
                c.Key.Duration,
                c.Key.Rating,
                true,
                c.Key.Price,
                0,
                0,
                c.SelectMany(e => e.Tags),
                c.Select(e => e.Categories).ToList(),
                null!))
            .ToListAsync(cancellationToken);

        
        return courses;
    }
    public async Task<(int NoCompleted, int NoEnrollment)> GetCourseInfoAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        var courses = await _context.UserCourses
            .Where(e => e.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var NoCompleted = courses.Count(e => e.FinshedDate is not null);

        return (NoCompleted, courses.Count - NoCompleted);
    }
    public async Task<IEnumerable<UserResponse>> GetUsersInCourseAsync(Guid id, CancellationToken cancellationToken = default)
    {

        var users = await (
            from uc in _context.UserCourses
            join u in _context.Users
            on uc.UserId equals u.Id
            where uc.CourseId == id
            select new UserResponse(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email!,
                u.Level,
                u.Rating,
                u.DateOfBirth,
                null!
                )
            ).ToListAsync(cancellationToken);

        return users;
    }
    public async Task<Result<UserCourse>> GetByCourseIdAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        var course = await _context.UserCourses
            .SingleOrDefaultAsync(e => e.CourseId == id && e.UserId == userId, cancellationToken);

        return course is null
            ? Result.Failure<UserCourse>(EnrollmentErrors.NotFoundEnrollment)
            : Result.Success(course);
    }
    // TODO:
    // How to update lastvideowtched 
    // how to update resume video
    // how to record the progress of the view
    // how to calculate the duration of the video

}
