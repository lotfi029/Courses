using Courses.Business.Contract.Category;
using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;
using Courses.Business.Contract.User;
using Courses.Business.Entities;

namespace Courses.DataAccess.Services;
public class EnrolmentService(
    ApplicationDbContext context) : IEnrolmentService
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
        
        
        if (await _context.Lessons.FindAsync([id], cancellationToken) is not { } lesson)
            return Result.Failure<UserLessonResponse>(LessonErrors.NotFound);

        var userLessons = await _context.UserLessons
            .AsNoTracking()
            .Where(e => e.UserCourseId == userCourse.Id && userId == e.UserId)
            .ToListAsync(cancellationToken);

        var orderLessons = await (
            from m in _context.Modules
            join l in _context.Lessons
            on m.Id equals l.ModuleId into ls
            where m.CourseId == courseId
            select new
            {
                m.Id,
                m.OrderIndex,
                lessons = ls.ToList()
            })
            .AsNoTracking()
            .OrderBy(e => e.OrderIndex)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);


        Lesson nextLesson = null!;
        if (userLessons.Select(e => e.ModuleItemId).Contains(id))
            nextLesson = lesson;
        else
        {
            foreach (var module in orderLessons)
            {
                foreach (var l in module.lessons)
                {
                    if (!userLessons.Where(e => e.IsComplete).Select(e => e.ModuleItemId).Contains(l.Id))
                    {
                        nextLesson = l;
                        break;
                    }
                }
                if (nextLesson is not null)
                    break;
            }
        }

        var userLesson = userLessons.SingleOrDefault(e => e.ModuleItemId == id && e.UserId == userId);

        if (id != nextLesson!.Id)
            return Result.Failure<UserLessonResponse>(EnrollmentErrors.InvalidGettingLesson);

        if (userLesson is null)
        {
            userLesson = new UserLesson
            {
                ModuleItemId = id,
                UserId = userId,
                UserCourseId = userCourse.Id
            };

            await _context.AddAsync(userLesson,cancellationToken);
        }

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
                c.Thumbnail,
                userCourse.Progress,
                userCourseId = userCourse.Id,
                userCourse.IsCompleted,
                userCourse.LastInteractDate,
                userCourse.FinshedDate,
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
                c.Thumbnail,
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
                c.Key.Thumbnail,
                c.Key.Duration,
                c.Key.Rating,
                c.Key.userCourseId,
                c.Key.IsCompleted,
                c.Key.LastInteractDate,
                c.Key.FinshedDate,
                c.Key.Progress,
                Categories = c.Select(e => e.Categories).Adapt<List<CategoryResponse>>()

            }).Single();     


        var moduleQuery = await (
            from m in _context.Modules
            join mi in _context.ModuleItems
            on m.Id equals mi.ModuleId
            join umi in _context.UserModuleItems
            on mi.Id equals umi.ModuleItemId into umis
            from umi in umis.DefaultIfEmpty()
            where m.CourseId == courseId
            select new
            {
                m.Id,
                m.Title,
                m.Description,
                m.Duration,
                moduleItemId = mi.Id,
                moduleItemTitle = mi.Title,
                moduleItemDuration = mi.Duration,
                mi.ItemType,
                IsComplete = umi == null ? false : umi.IsComplete,
                startDate = umi == null ? default : umi.StartDate,
                endDate = umi.EndDate ?? null
            }).ToListAsync(cancellationToken);


        var modules = moduleQuery
            .GroupBy(m => new { m.Id, m.Title, m.Description, m.Duration })
            .Select(x => new { x.Key.Id, x.Key.Title, x.Key.Description, x.Key.Duration, moduleItem = x.Select(um => new UserModuleItemResponse(um.moduleItemId, um.moduleItemTitle, um.moduleItemDuration, um.IsComplete, um.startDate, um.endDate, (int)um.ItemType)).ToList() })
            .Select(x => new UserModuleResponse(x.Id, x.Title, x.Description, x.Duration, x.moduleItem))
            .ToList();



        var respons = new UserCourseResponse(
            course.Id,
            course.Title,
            course.Description,
            course.Level,
            course.Thumbnail,
            course.Duration,
            course.Rating,
            course.Progress,
            course.userCourseId,
            course.IsCompleted,
            course.LastInteractDate,
            course.FinshedDate,
            modules,
            course.Categories
            );

        return Result.Success(respons);
    }

    public async Task<Result> CompleteLessonAsync(Guid id, Guid courseId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.UserCourses.SingleOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken) is not { } userCourse)
            return EnrollmentErrors.NotFoundEnrollment;

        if (userCourse.IsCompleted)
            return Result.Success();

        if (await _context.UserLessons.SingleOrDefaultAsync(e => e.ModuleItemId == id && e.UserId == userId, cancellationToken) is not { } userLesson)
            return EnrollmentErrors.NotFoundEnrollment;

        if (userLesson.IsComplete)
            return Result.Success();

        var lessonCnt = await _context.Modules
            .Where(e => e.CourseId == courseId)
            .Join(_context.Lessons, m => m.Id, l => l.ModuleId, (m, l) => l.Id)
            .CountAsync(cancellationToken);

        var completeLesson = await _context.UserLessons
            .CountAsync(e => e.UserCourseId == userCourse.Id && e.UserId == userId, cancellationToken);
        var p = (float)(completeLesson) / (float)lessonCnt;
        userCourse.Progress = p;

        userLesson.IsComplete = true;
        userLesson.EndDate = DateTime.UtcNow;
        userLesson.LastInteractDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> CompleteCourseAsync(Guid courseId, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.UserCourses.Include(e => e.UserLessons).SingleOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId, cancellationToken) is not { } userCourse)
            return EnrollmentErrors.NotFoundEnrollment;

        var userLessons = userCourse.UserLessons.Where(e => e.IsComplete).ToList();

        var lessonCnt = await _context.Modules
            .Where(e => e.CourseId == courseId)
            .Join(_context.Lessons, m => m.Id, l => l.ModuleId, (m, l) => l.Id)
            .CountAsync(cancellationToken);

        return userLessons.Count == lessonCnt ? Result.Success() : EnrollmentErrors.InvalidCompleteCourse;
    } 
    public async Task<IEnumerable<UserCourseResponse>> GetMyCoursesAsync(string userId, CancellationToken cancellationToken = default)
    {

        var query = await (
            from c in _context.Courses
            join cCat in _context.CourseCategories
            on c.Id equals cCat.CourseId into cc
            from cCats in cc.DefaultIfEmpty()
            join cats in _context.Categories
            on cCats.CategoryId equals cats.Id into cats
            from categories in cats.DefaultIfEmpty()
            join uc in _context.UserCourses
            on c.Id equals uc.CourseId 
            where uc.UserId == userId
            select new
            {
                c.Id,
                c.Title,
                c.Description,
                c.Level,
                c.Rating,
                c.Duration,
                c.Thumbnail,
                uc.Progress,
                userCourseId = uc.Id,
                uc.IsCompleted,
                uc.LastInteractDate,
                uc.FinshedDate,
                Categories = categories
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var courses = query
            .GroupBy(c => new {
                c.Id,
                c.Title,
                c.Description,
                c.Level,
                c.Rating,
                c.Duration,
                c.Thumbnail,
                c.userCourseId,
                c.IsCompleted,
                c.LastInteractDate,
                c.FinshedDate,
                c.Progress
            })
            .Select(c => new UserCourseResponse(
                c.Key.Id,
                c.Key.Title,
                c.Key.Description,
                c.Key.Level,
                c.Key.Thumbnail,
                c.Key.Duration,
                c.Key.Rating,
                c.Key.Progress,
                c.Key.userCourseId,
                c.Key.IsCompleted,
                c.Key.LastInteractDate,
                c.Key.FinshedDate,
                null!,
                c.Select(e => e.Categories).Adapt<List<CategoryResponse>>()
            )).ToList();

        if (courses is null)
            return [];
            
        return courses;
    }
    
    // another serivce
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
}
