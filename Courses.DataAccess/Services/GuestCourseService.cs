using Courses.Business.Contract.Category;
using Courses.Business.Contract.Course;

namespace Courses.DataAccess.Services;
public class GuestCourseService(ApplicationDbContext context) : IGuestCourseService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<GuestUserCourseResponse>> GetCourseAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id && e.IsPublished, cancellationToken);

        if (course is null)
            return Result.Failure<GuestUserCourseResponse>(CourseErrors.NotFound);

        var categoryResponse = await _context.CourseCategories
            .Include(e => e.Category)
            .Where(e => e.CourseId == id)
            .Select(e => new CategoryResponse(e.Category.Id, e.Category.Title, e.Category.Description))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var response = (course, categoryResponse ?? []).Adapt<GuestUserCourseResponse>();

        return Result.Success(response);
    }
    public async Task<IEnumerable<GuestUserCourseResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var courses = await (
            from c in _context.Courses
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
                c.Thumbnail,
                c.Duration,
                catg               
            })
            .GroupBy(x => new { x.Id, x.Title, x.Description, x.Level, x.Thumbnail, x.Duration })
            .Select(e => new GuestUserCourseResponse(
                e.Key.Id,
                e.Key.Title,
                e.Key.Description,
                e.Key.Level,
                e.Key.Thumbnail,
                e.Key.Duration,
                e.Select(e => e.catg).Adapt<List<CategoryResponse>>()
                )
            )
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        return courses;
    }
}
