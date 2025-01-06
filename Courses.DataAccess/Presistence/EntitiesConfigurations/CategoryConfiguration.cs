namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(e => e.Title)
            .HasMaxLength(450);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);   

        builder.Property(e => e.IsDisable)
            .HasDefaultValue(false);
    }
}
