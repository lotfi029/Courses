using Courses.Business.Contract.Category;
using Courses.Business.Contract.Course;

namespace Courses.DataAccess.Services;
public class UserCourseService(ApplicationDbContext context) : IUserCourseService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<RegularUserCourseResponse>> GetCourseAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .Include(e => e.Tags)
            .SingleOrDefaultAsync(e => e.Id == id && e.IsPublished, cancellationToken);

        if (course is null)
            return Result.Failure<RegularUserCourseResponse>(CourseErrors.NotFound);

        var categoryResponse = await _context.CourseCategories
            .AsNoTracking()
            .Include(e => e.Category)
            .Where(e => e.CourseId == id)
            .Select(e => new CategoryResponse(e.Category.Id, e.Category.Title, e.Category.Description))
            .ToListAsync(cancellationToken);

        var response = (course, categoryResponse ?? [], course.Tags.ToList()).Adapt<RegularUserCourseResponse>();

        return Result.Success(response);
    }
    public async Task<IEnumerable<RegularUserCourseResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var courses = await (
            from c in _context.Courses.Include(e => e.Tags)
            join cCats in _context.CourseCategories
            on c.Id equals cCats.CourseId into cg
            from cCats in cg.DefaultIfEmpty()
            join g in _context.Categories
            on cCats.CategoryId equals g.Id into gs
            from catg in gs.DefaultIfEmpty()
            where c.IsPublished
            select new {
                c.Id,
                c.Title,
                c.Description,
                c.Level,
                c.ThumbnailId,
                c.Duration,
                catg,
                Tag = c.Tags.Select(e => e.Title).ToList()
            })
            .GroupBy(x => new { x.Id, x.Title, x.Description, x.Level, x.ThumbnailId, x.Duration })
            .Select(e => new RegularUserCourseResponse(
                e.Key.Id,
                e.Key.Title,
                e.Key.Description,
                e.Key.Level,
                e.Key.ThumbnailId,
                e.Key.Duration,
                e.Select(e => e.catg).Adapt<List<CategoryResponse>>(),
                e.SelectMany(e => e.Tag))
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        return courses;
    }
}
