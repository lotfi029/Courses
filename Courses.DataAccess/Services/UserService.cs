using Courses.Business.Contract.User;

namespace Courses.DataAccess.Services;
public class UserService(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(e => e.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .AsNoTracking()
            .SingleAsync();

        return Result.Success(user);
    }
    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        var result = await _userManager.Users
            .Where(e => e.Id == userId)
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.FirstName, request.FirstName)
                .SetProperty(e => e.LastName, request.LastName)
                .SetProperty(e => e.Level, request.Level)
                .SetProperty(e => e.DateOfBirth, request.DateOfBirth)
            );
        
        return Result.Success();
    }
    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.Users.SingleAsync(e => e.Id == userId);

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            
            return new Error(error.Code, error.Description, StatusCodes.Status400BadRequest);
        }

        return Result.Success();
    }
    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await (from u in _context.Users 
               join ur in _context.UserRoles
               on u.Id equals ur.UserId
               join r in _context.Roles
               on ur.RoleId equals r.Id into roles
               select new
               {
                   u.Id,
                   u.FirstName,
                   u.LastName,
                   u.Email,
                   u.Level,
                   u.Rating,
                   u.DateOfBirth,
                   Roles = roles.Select(e => e.Name!).ToList()
               })               
        .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.Level, u.Rating, u.DateOfBirth })
        .Select(u => new UserResponse 
        (
            u.Key.Id,
            u.Key.FirstName,
            u.Key.LastName,
            u.Key.Email,
            u.Key.Level,
            u.Key.Rating,
            u.Key.DateOfBirth,
            u.SelectMany(e => e.Roles).ToList()
        ))
       .ToListAsync(cancellationToken);

    public async Task<Result<UserResponse>> GetAsync(string id,  CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var userRoles = await _userManager.GetRolesAsync(user);

        var response = (user, userRoles).Adapt<UserResponse>();

        return Result.Success(response);
    }


}
