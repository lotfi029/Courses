namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.Property(e => e.Text)
            .HasMaxLength(450);
    }
}
