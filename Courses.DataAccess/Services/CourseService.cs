using Courses.Business.Contract.Category;
using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;
using Courses.Business.Contract.Tag;


namespace Courses.DataAccess.Services;
public partial class CourseService(
    ApplicationDbContext context,
    IFileService fileService) : ICourseService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<Guid>> AddAsync(AddCourseRequest request, CancellationToken cancellationToken = default)
    {
        var course = request.Adapt<Course>();

        // save Image
        course.ThumbnailId = await _fileService.UploadAsync(request.Thumbnail, cancellationToken);
        
        var tags = await _context.Tags
            .Where(t => request.Tags.Contains(t.Title))
            .ToListAsync(cancellationToken);

        if (tags.Count != request.Tags.Count)
        {
            var newTags = request.Tags.Except(tags.Select(e => e.Title), StringComparer.OrdinalIgnoreCase);
            foreach (var newTag in newTags)
                tags.Add(new Tag { Title = newTag });
        }

        foreach (var id in course.CourseCategories)
            id.CourseId = course.Id;

        course.Tags = tags;

        await _context.AddAsync(course, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(course.Id);
    }

    public async Task<Result> UpdateAsync(string userid, Guid id, UpdateCourseRequest request, CancellationToken cancellationToken = default)
    {

        var result = await _context.Courses
            .Where(e => e.Id == id && e.CreatedById == userid)
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.Title, request.Title)
                .SetProperty(e => e.Description, request.Description)
                .SetProperty(e => e.Level, request.Level)
                .SetProperty(e => e.Price, request.Price),
                cancellationToken
            );

        if (result == 0)
            return CourseErrors.NotFound;

        return Result.Success();
    }

    public async Task<Result> ToggleIsPublishAsync(string userid, Guid id, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.FindAsync([id], cancellationToken) is not { } course)
            return Result.Failure(CourseErrors.NotFound);

        course.IsPublished = !course.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> AssignCourseToCategoryAsync(string userid, Guid id, Guid categoryId, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.FindAsync([id], cancellationToken);

        if (course is null || course.CreatedById != userid)
            return CourseErrors.NotFound;

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
    
    public async Task<Result> UnAssignCourseToCategoriesAsync(string userid, Guid id, Guid categoryId, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses.FindAsync([id], cancellationToken);

        if (course is null || course.CreatedById != userid)
            return CourseErrors.NotFound;

        if (await _context.CourseCategories.SingleOrDefaultAsync(e => e.CourseId == id && e.CategoryId == categoryId, cancellationToken) is not { } courseCategory)
            return CourseErrors.InvalidCategory;

        _context.Remove(courseCategory);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> AssignCourseToTagsAsync(string userid, Guid id, TagsRequest tags, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.Include(e => e.Tags).SingleOrDefaultAsync(e => e.Id == id, cancellationToken) is not { } course)
            return CourseErrors.NotFound;

        var tagsDb = _context.Tags
            .Where(e => tags.Tags.Contains(e.Title));

        var addedTags = tags.Tags
            .Except(tagsDb.Select(e => e.Title), StringComparer.OrdinalIgnoreCase)
            .Select(e => new Tag { Title = e })
            .ToList();

        await _context.AddRangeAsync(addedTags, cancellationToken);

        addedTags.AddRange(tagsDb);
        foreach (var tag in addedTags)
            if (!course.Tags.Contains(tag))
                course.Tags.Add(tag);

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UnAssignCourseToTagsAsync(string userid, Guid id, TagsRequest tags, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.Include(e => e.Tags).SingleOrDefaultAsync(e => e.Id == id, cancellationToken) is not { } course)
            return Result.Failure(CourseErrors.NotFound);

        var tagsExists = !tags.Tags.Except(course.Tags.Select(e => e.Title), StringComparer.OrdinalIgnoreCase).Any();

        if (!tagsExists)
            return Result.Failure(TagsError.NotFound);

        var remvedTags = await _context.Tags.Where(e => tags.Tags.Contains(e.Title)).ToListAsync(cancellationToken);

        foreach (var tag in remvedTags)
            course.Tags.Remove(tag);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
public partial class CourseService
{

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


        var response = (course, modules).Adapt<CourseResponse>();

        return Result.Success(response);
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

    //private async Task<IEnumerable<CourseResponse>> LoadCoursesAsync(Guid? id, string userId, CancellationToken cancellationToken = default)
    //{

    //}

    private async Task<IEnumerable<CourseResponse>> GetCoursesAsync(Guid? id, string? userId = null, CancellationToken cancellationToken = default)
    {
        //var query = await (
        //    from c in _context.Courses
        //    join cCat in _context.CourseCategories
        //    on c.Id equals cCat.CourseId into cc
        //    from cCats in cc.DefaultIfEmpty()
        //    join cats in _context.Categories
        //    on cCats.CategoryId equals cats.Id into cats
        //    from categories in cats.DefaultIfEmpty()
        //    join userCourse in _context.UserCourses
        //    on new { CourseId = c.Id, UserId = userId } equals new { userCourse.CourseId, userCourse.UserId } into userCourseJoin
        //    from userCourse in userCourseJoin.DefaultIfEmpty()
        //    where id == null || c.Id == id
        //    select new
        //    {
        //        c.Id,
        //        c.Title,
        //        c.Description,
        //        c.Level,
        //        c.Rating,
        //        c.Duration,
        //        c.Price,
        //        c.ThumbnailId,
        //        c.IsPublished,
        //        Tags = c.Tags.Select(e => e.Title),
        //        categories,
        //        userCourse
        //    })
        //    .AsNoTracking()
        //    .ToListAsync(cancellationToken);

        //var course = query.GroupBy(
        //    group => new
        //    {
        //        group.Id,
        //        group.Title,
        //        group.Description,
        //        group.Level,
        //        group.Rating,
        //        group.Duration,
        //        group.Price,
        //        group.IsPublished,
        //        group.ThumbnailId,
        //        group.userCourse
        //    })
        //    .Select(
        //    x => new CourseResponse
        //    (
        //        x.Key.Id,
        //        x.Key.Title,
        //        x.Key.Description,
        //        x.Key.Level,
        //        x.Key.ThumbnailId,
        //        x.Key.Duration,
        //        x.Key.Rating,
        //        x.Key.IsPublished,
        //        x.Key.Price,
        //        x.SelectMany(e => e.Tags ?? []).Distinct().ToList(),
        //        x.Select(e => e.categories).Distinct().Adapt<List<CategoryResponse>>()
        //        //x.Key.userCourse.Adapt<UserCourseResponse>()
        //    ));

        return  []; 
    }

}