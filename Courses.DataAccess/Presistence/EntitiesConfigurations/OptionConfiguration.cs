using Courses.Business.Entities;

namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.Property(e => e.Text)
            .HasMaxLength(450);

        builder.HasIndex(e => new { e.Text, e.QuestionId }).IsUnique();
    }
}
