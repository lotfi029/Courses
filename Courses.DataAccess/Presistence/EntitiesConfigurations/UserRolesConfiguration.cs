
using Courses.Business.Abstract.Constants;

namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string> {
                UserId = DefaultUsers.Admin.Id,
                RoleId = DefaultRoles.Admin.Id,
            }
        );
    }
}
