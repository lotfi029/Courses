namespace Courses.Business.Entities;

public class Tag
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public ICollection<Course> Courses { get; set; } = [];
}