
namespace Courses.DataAccess.Presistence.EntitiesConfigurations;
public class UserExamConfiguration : IEntityTypeConfiguration<UserExam>
{
    public void Configure(EntityTypeBuilder<UserExam> builder)
    {
        builder.Property(e => e.StartDate)
            .HasDefaultValueSql("GETDATE()");
    }
}
