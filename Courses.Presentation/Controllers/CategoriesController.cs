using Courses.Business.Abstract.Constants;
using Courses.Business.Contract.Category;
using Courses.Business.Authentication.Filters;

namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost("")]
    [HasPermission(Permissions.AddCategory)]
    public async Task<IActionResult> Add([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.AddAsync(request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value }, null) : result.ToProblem();
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> ToggleStatus([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _categoryService.ToggleStatusAsync(id, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetAllAsync(cancellationToken);

        return Ok(result);
    }
    [HttpGet("include-disable")]
    public async Task<IActionResult> GetAllIncludeDisable(CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetAllAsync(true, cancellationToken);

        return Ok(result);
    }
}
