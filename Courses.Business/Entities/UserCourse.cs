﻿namespace Courses.Business.Entities;

public class UserCourse
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public Guid? LastAccessLessonId { get; set; }
    public TimeSpan? LastWatchTimestamp { get; set; }
    public string CompleteStatus { get; set; } = string.Empty; // 'Inprogess, complete
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime LastInteractDate { get; set; } = DateTime.UtcNow;
    public DateTime? FinshedDate { get; set; }
    public Course Course { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
    public Lesson LastAccessLesson { get; set; } = default!;
    
}
