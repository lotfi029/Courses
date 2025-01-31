namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class UserModuleItemConfiguration : IEntityTypeConfiguration<UserModuleItem>
{
    public void Configure(EntityTypeBuilder<UserModuleItem> builder)
    {
        builder.UseTptMappingStrategy();


    }
}
