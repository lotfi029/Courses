namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public sealed class ModuelConfiguration : IEntityTypeConfiguration<CourseModule>
{
    public void Configure(EntityTypeBuilder<CourseModule> builder)
    {
        builder.ToTable("Modules");

        builder.Property(e => e.Title)
            .HasMaxLength(450);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.HasIndex(e => new { e.Title, e.CourseId })
            .IsUnique();

        builder.HasIndex(e => new {e.CourseId, e.OrderIndex})
            .IsUnique();

    }
}