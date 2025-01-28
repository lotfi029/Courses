namespace Courses.DataAccess.Presistence.EntitiesConfigurations;
public class ModuleItemConfiguration : IEntityTypeConfiguration<ModuleItem>
{
    public void Configure(EntityTypeBuilder<ModuleItem> builder)
    {
        builder.UseTptMappingStrategy();

        builder.HasIndex(e => new { e.ModuleId, e.OrderIndex })
            .IsUnique();


    }
}
