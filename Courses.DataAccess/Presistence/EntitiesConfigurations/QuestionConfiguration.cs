namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(e => e.Text)
            .HasMaxLength(450);

        builder.Property(e => e.IsDisable)
            .HasDefaultValue(true);

        builder.HasMany(e => e.Options)
            .WithOne(e => e.Question)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}