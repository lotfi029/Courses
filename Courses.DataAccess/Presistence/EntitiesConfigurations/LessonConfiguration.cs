namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.Property(e => e.Title)
            .HasMaxLength(450);

        builder.OwnsMany(e => e.Resources, recourses =>
        {
            recourses.WithOwner(e => e.Lesson)
                .HasForeignKey(e => e.LessonId);

            recourses.ToTable("Recourses");
        });

        builder.HasIndex(e => new {e.ModuleId, e.Title})
            .IsUnique();
    }
}