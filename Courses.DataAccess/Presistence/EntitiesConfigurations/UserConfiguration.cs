using Courses.Business.Abstract.Constants;

namespace Courses.DataAccess.Presistence.EntitiesConfigurations;
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {

        builder.OwnsMany(e => e.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.Property(e => e.FirstName)
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .HasMaxLength(100);

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.Admin.Id,
            FirstName = "Course",
            LastName = "Admin",
            UserName = DefaultUsers.Admin.UserName,
            NormalizedUserName = DefaultUsers.Admin.UserName.ToUpper(),
            Email = DefaultUsers.Admin.Email,
            NormalizedEmail = DefaultUsers.Admin.Email.ToUpper(),
            EmailConfirmed = true,
            ConcurrencyStamp = DefaultUsers.Admin.ConcurrencyStamp,
            SecurityStamp = DefaultUsers.Admin.SecurityStamp,
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow),
            PasswordHash = "AQAAAAIAAYagAAAAEB1mj/z+fDc1fii5XJzaIEYd/69RgtrQkCyJ+fbd3r00ddSKPyjGrvAJm2WNy/okcw=="
        });
    }
}
