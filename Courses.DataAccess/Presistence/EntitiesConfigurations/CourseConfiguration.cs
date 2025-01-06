namespace Courses.DataAccess.Presistence.EntitiesConfigurations;
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(e => e.Title)
            .HasMaxLength(450);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Level)
            .HasMaxLength(100);
        
        //builder.Property(e => e.ThumbnailUrl)
        //    .HasMaxLength(100);

        builder.Property(e => e.IsPublished)
            .HasDefaultValue(1);
    }
}
