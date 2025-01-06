namespace Courses.Business.Entities;

public class Category
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDisable { get; set; }
    public ICollection<CourseCategories> CourseCategories { get; set; } = [];
}
