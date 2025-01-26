using Courses.Business.Abstract.Constants;
using Courses.Business.Contract.Role;
using System.Diagnostics;

namespace Courses.DataAccess.Services;
public class RoleService(
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<string>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default)
    {

        if (await _roleManager.RoleExistsAsync(request.Name))
            return Result.Failure<string>(RoleErrors.InvalidName);

        var permissions = Permissions.GetAllPermissions;

        if (request.Permissions.Except(permissions).Any())
            return Result.Failure<string>(RoleErrors.InvalidPermission);

        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.CreateVersion7().ToString()
        };

        var result = await _roleManager.CreateAsync(role);
        
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure<string>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var permissionClaim = request.Permissions
            .Select(e => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = e,
                RoleId = role.Id
            });

        await _context.RoleClaims.AddRangeAsync(permissionClaim, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(role.Id);
    }
    public async Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken = default)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return RoleErrors.NotFound;

        if (await _roleManager.Roles.AnyAsync(e => e.Name == request.Name && e.Id != id, cancellationToken))
            return RoleErrors.InvalidName;

        var permissions = Permissions.GetAllPermissions;

        if (request.Permissions.Except(permissions).Any())
            return RoleErrors.InvalidPermission;


        var currentPermissions = await _context.RoleClaims
            .Where(e => e.RoleId == id && e.ClaimType == Permissions.Type)
            .Select(e => e.ClaimValue)
            .ToListAsync(cancellationToken);

        var deletedPermissions = currentPermissions.Except(request.Permissions);

        var addedPermissions = request.Permissions.Except(currentPermissions)
            .Select(e => new IdentityRoleClaim<string>
            {
                RoleId = id,
                ClaimType = Permissions.Type,
                ClaimValue = e
            });



        await _context.RoleClaims
            .Where(e => e.RoleId == id && deletedPermissions.Contains(e.ClaimValue!))
            .ExecuteDeleteAsync(cancellationToken);

        await _context.RoleClaims.AddRangeAsync(addedPermissions, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return Result.Success();
    }
    public async Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await  _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure(UserErrors.EmailNotConfirmed);

        role.IsDeleted = !role.IsDeleted;

        await _roleManager.UpdateAsync(role);

        return Result.Success();
    }
    public async Task<Result<RoleDetailsResponse>> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailsResponse>(RoleErrors.NotFound);

        var claims = await _roleManager.GetClaimsAsync(role);
        var permissions = claims.Select(e => e.Value);

        RoleDetailsResponse response = new(role.Id, role.Name!, role.IsDeleted, permissions);

        return Result.Success(response);
    }

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisable = false, CancellationToken cancellationToken = default)
    {
        var roles = await _roleManager.Roles
            .Where(e => !e.IsDefualt && (!e.IsDeleted || includeDisable))
            .AsNoTracking()
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);

        return roles;
    }
    
}
