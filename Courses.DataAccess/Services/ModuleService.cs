using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;
using Courses.DataAccess.Presistence;

namespace Courses.DataAccess.Services;
public class ModuleService(ApplicationDbContext context) : IModuleService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Guid>> AddAsync(string userId, Guid courseId, ModuleRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Courses.Select(e => new {e.Id, e.CreatedById}).SingleOrDefaultAsync(e => e.Id == courseId, cancellationToken) is not { } course)
            return Result.Failure<Guid>(CourseErrors.NotFound);

        if (course.CreatedById != userId)
            return Result.Failure<Guid>(CourseErrors.NotFound);

        var modules = await _context.Modules
            .Where(e => e.CourseId == courseId)
            .ToListAsync(cancellationToken);

        int maxOrder = 0;

        if (modules?.Count > 1)
        {
            if (modules.Any(e => e.CourseId == courseId && e.Title == request.Title))
                return Result.Failure<Guid>(ModuleErrors.DuplicatedTitle);

            maxOrder = modules.Max(e => e.Order);
        }

        var module = request.Adapt<CourseModule>();

        module.CourseId = courseId;
        module.Order = maxOrder + 1;

        await _context.Modules.AddAsync(module, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(module.Id);
    }
    public async Task<Result> UpdateAsync(Guid id, ModuleRequest request, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Modules.FindAsync([id], cancellationToken) is not { } module)
            return Result.Failure(ModuleErrors.NotFound);

        if (module.CreatedById != userId)
            return Result.Failure(UserErrors.UnAutherizeAccess);

        if (request.Title != module.Title && await _context.Modules.AnyAsync(e => e.Id != id && e.CourseId == e.CourseId && e.Title == request.Title, cancellationToken))
            return ModuleErrors.DuplicatedTitle;

        module = request.Adapt(module);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateOrderAsync(Guid id, string userId, UpdateOrderRequest request ,CancellationToken cancellationToken = default)
    {
        if (await _context.Modules.OrderBy(e => e.Order).ToListAsync(cancellationToken) is not { } modules)
            return Result.Failure(ModuleErrors.NotFound);

        if (modules.Find(e => e.Id == id) is not { } module)
            return Result.Failure(ModuleErrors.NotFound);

        if (module.CreatedById != userId)
            return Result.Failure(UserErrors.UnAutherizeAccess);

        if (request.Order == module.Order)
            return Result.Success();

        var lessonCnt = modules.Count;
        var oldOrder = module.Order;
        var newOrder = request.Order;

        if (newOrder > lessonCnt)
            newOrder = lessonCnt;
        else if (newOrder < 1)
            newOrder = 1;

        if (newOrder > oldOrder) {
            foreach(var mdl in modules)
            {
                if (mdl.Id == module.Id)
                    mdl.Order = newOrder;
                else if (mdl.Order <= newOrder && mdl.Order >= oldOrder)
                    mdl.Order -= 1;
            }
        }
        else {
            foreach (var mdl in modules)
            {
                if (mdl.Id == module.Id)
                    mdl.Order = newOrder;
                else if (mdl.Order >= newOrder && mdl.Order <= oldOrder)
                    mdl.Order += 1;
            }
        }

        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
    public async Task<Result> ToggleStatusAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        if (await _context.Modules.FindAsync([id], cancellationToken) is not { } module)
            return Result.Failure(ModuleErrors.NotFound);

        if (module.CreatedById != userId)
            return Result.Failure(UserErrors.UnAutherizeAccess);

        module.IsDisable = !module.IsDisable;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result<ModuleResponse>> GetAsync(Guid id, string? userId = null, CancellationToken cancellationToken = default)
    {
        if (await _context.Modules.FindAsync([id], cancellationToken) is not { } module)
            return Result.Failure<ModuleResponse>(ModuleErrors.NotFound);

        var response = module.Adapt<ModuleResponse>();

        return Result.Success(response);
    }
    public async Task<IEnumerable<ModuleResponse>> GetAllAsync(Guid courseId, string? userId = null, CancellationToken cancellationToken = default)
    {
        var modules = await _context.Modules
            .Where(e => e.CourseId == courseId)
            .OrderBy(e => e.Order)
            .AsNoTracking()
            .ProjectToType<ModuleResponse>()
            .ToListAsync(cancellationToken);


        return modules;
    }

}
