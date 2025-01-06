using Courses.Business.Abstract.Constants;
using Serilog.Configuration;

namespace Courses.DataAccess.Presistence.EntitiesConfigurations;
public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new ApplicationRole {
                Id = DefaultRoles.Admin.Id,
                Name = DefaultRoles.Admin.Name,
                NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp
            },
            new ApplicationRole {
                Id = DefaultRoles.Student.Id,
                Name = DefaultRoles.Student.Name,
                NormalizedName = DefaultRoles.Student.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Student.ConcurrencyStamp
            }
        ]);
    }
}
