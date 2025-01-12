﻿using Courses.Business.Contract.Course;
using Courses.Business.Contract.Tag;
namespace Courses.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]


public class CoursesController(ICourseService courseService) : ControllerBase
{
    private readonly ICourseService _courseService = courseService;
    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> Add([FromForm] AddCourseRequest request, CancellationToken cancellationToken)
    {
        var result = await _courseService.AddAsync(request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new {id = result.Value}, null) : result.ToProblem();
    }
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.UpdateAsync(userId,id, request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/assign-category/{categoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> AssignCourseToCategories([FromRoute] Guid id,[FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.AssignCourseToCategoryAsync(userId, id, categoryId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }
    [HttpPut("{id:guid}/unassign-category/{categoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnAssignCourseToCategories([FromRoute] Guid id, [FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.UnAssignCourseToCategoriesAsync(userId, id, categoryId, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/assign-tags")]
    [Authorize]
    public async Task<IActionResult> AssingTagsToCourse([FromRoute] Guid id, [FromBody] TagsRequest tags, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        
        var result = await _courseService.AssignCourseToTagsAsync(userId, id, tags, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{id:guid}/unassign-tags")]
    [Authorize]
    public async Task<IActionResult> UnAssingTagsToCourse([FromRoute] Guid id, [FromBody] TagsRequest tags, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;
        var result = await _courseService.UnAssignCourseToTagsAsync(userId, id, tags, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("toggle-status/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> ToggleIsPublish([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.ToggleIsPublishAsync(userId, id, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.GetAsync(id, userId, cancellationToken: cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!;

        var result = await _courseService.GetAllAsync(userId, cancellationToken: cancellationToken);

        return Ok(result);
    }
    
}