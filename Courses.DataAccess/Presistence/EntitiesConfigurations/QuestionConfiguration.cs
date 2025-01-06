namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(e => e.Text)
            .HasMaxLength(450);
    }
}