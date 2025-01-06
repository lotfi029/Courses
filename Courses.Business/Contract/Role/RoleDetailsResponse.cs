namespace Courses.Business.Contract.Role;

public record RoleDetailsResponse (
    string Id,
    string Name,
    bool IsDelete,
    IEnumerable<string> Permissions
    );
