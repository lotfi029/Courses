namespace Courses.Business.Contract.Role;
public record RoleRequest (
    string Name,
    IList<string> Permissions
    );
