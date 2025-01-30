namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.Property(e => e.Title)
            .HasMaxLength(450);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.IsDisable)
            .HasDefaultValue(true);

        builder.HasIndex(e => new { e.ModuleId, e.Title })
            .IsUnique();
    }
}
