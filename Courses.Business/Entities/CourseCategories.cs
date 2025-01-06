using Microsoft.EntityFrameworkCore;

namespace Courses.Business.Entities;

public class CourseCategories
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid CategoryId { get; set; }
    public Guid CourseId { get; set; }

    public Course Course { get; set; } = default!;
    public Category Category { get; set; } = default!;
}