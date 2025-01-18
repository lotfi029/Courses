namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
{
    public void Configure(EntityTypeBuilder<UserCourse> builder)
    {
        builder.Property(e => e.IsCompleted)
            .HasDefaultValue (0);
    }
}
