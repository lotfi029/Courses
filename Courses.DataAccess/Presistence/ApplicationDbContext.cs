using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Courses.DataAccess.Presistence;
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor contextAccessor) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<CourseCategories> CourseCategories { get; set; } = default!;
    public DbSet<Exam> Exams { get; set; } = default!;
    public DbSet<Lesson> Lessons { get; set; } = default!;
    public DbSet<CourseModule> Modules { get; set; } = default!;
    public DbSet<ModuleItem> ModuleItems { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }
    public DbSet<UserLesson> UserLessons { get; set; }
    public DbSet<UserModuleItem> UserModuleItems { get; set; }
    public DbSet<UserExam> UserExams { get; set; }
    public DbSet<UploadedFile> UploadedFiles { get; set; }
    public DbSet<Question> Questions { get; set; } = default!;
    public DbSet<Option> Options {  get; set; } = default!;
    public DbSet<Recourse> Recourses { get; set; } = default!;
    public DbSet<ExamQuestion> ExamQuestion { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var cascadeFKs = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(e => e.DeleteBehavior == DeleteBehavior.Cascade && !e.IsOwnership);

        foreach(var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entiries = ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _contextAccessor.HttpContext!.User.GetUserId()!;

        foreach (var entity in entiries)
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
                entity.Property(e => e.CreatedById).CurrentValue = currentUserId;
            }
            else if (entity.State == EntityState.Modified)
            {
                entity.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                entity.Property(e => e.UpdatedById).CurrentValue = currentUserId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
