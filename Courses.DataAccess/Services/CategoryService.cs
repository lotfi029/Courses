using Courses.Business.Contract.Category;
using Courses.DataAccess.Presistence;

namespace Courses.DataAccess.Services;
public class CategoryService(ApplicationDbContext context) : ICategoryService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Guid>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = request.Adapt<Category>();

        await _context.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _context.Categories
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.Description, request.Description)
                .SetProperty(e => e.Title, request.Title),
                cancellationToken
            );

        if (result == 0)
            return CategoryErrors.NotFound;

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (await _context.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken) is not { } category)
            return CategoryErrors.NotFound;

        category.IsDisable = !category.IsDisable;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<CategoryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (await _context.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken) is not { } category)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound);

        var response = category.Adapt<CategoryResponse>();

        return Result.Success(response);
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _context.Categories.Where(e => !e.IsDisable).ToListAsync(cancellationToken);
        
        var response = categories.Adapt<IEnumerable<CategoryResponse>>();

        return response;
    }
    public async Task<IEnumerable<CategoryResponse>> GetAllAsync(bool iscludeDisable, CancellationToken cancellationToken = default)
    {
        var categories = await _context.Categories
            .ToListAsync(cancellationToken);
        
        var response = categories.Adapt<IEnumerable<CategoryResponse>>();

        return response;
    }


}
