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

        if (await _context.Modules.AnyAsync(e => e.CourseId == courseId && request.Title == e.Title, cancellationToken))
            return Result.Failure<Guid>(ModuleErrors.DuplicatedTitle);

        var module = request.Adapt<CourseModule>();

        module.CourseId = courseId;

        await _context.Modules.AddAsync(module, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(module.Id);
    }
    public async Task<Result> UpdateAsync(Guid id, ModuleRequest request, string userId, CancellationToken cancellationToken = default)
    {
        //if (await _context.Modules.FindAsync([id], cancellationToken) is not { } module)
        //    return Result.Failure(ModuleErrors.NotFound);

        //if (module.CreatedById != userId)
        //    return Result.Failure(UserErrors.UnAutherizeUpdate);

        //module = request.Adapt(module);

        //await _context.SaveChangesAsync(cancellationToken);

        // TODO:
        var result = await _context.Modules
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.Description, request.Description)
                .SetProperty(e => e.Title, request.Title)
                .SetProperty(e => e.Order, request.Order),
                cancellationToken
            );

        if (result == 0)
            return ModuleErrors.NotFound;


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
