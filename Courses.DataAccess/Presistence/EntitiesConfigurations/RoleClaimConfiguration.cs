using Courses.Business.Abstract.Constants;

namespace Courses.DataAccess.Presistence.EntitiesConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var roleClaim = new List<IdentityRoleClaim<string>>();

        var permissions = Permissions.GetAllPermissions;

        for (var i = 0; i < permissions.Count; i++)
        {
            roleClaim.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permissions.Type,
                ClaimValue = permissions[i],
                RoleId = DefaultRoles.Admin.Id
            });
        }

        builder.HasData(roleClaim);
    }
}
