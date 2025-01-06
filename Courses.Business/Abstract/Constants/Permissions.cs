namespace Courses.Business.Abstract.Constants;
public static class Permissions
{
    public static string Type { get; } = "permissions";

    public const string AddCategory = "category:add";
    public const string GetCategory = "category:read";
    public const string UpdateCategory = "category:update";
    public const string ToggleCategory = "category:delete";

    public const string AddCourse = "course:add";
    public const string UpdateCourse = "course:update";
    public const string ToggleCourse = "course:toggle";

    public const string AddModule = "module:add";
    public const string UpdateModule = "module:update";
    public const string ToggleModule = "module:toggle";

    public const string AddLesson = "lesson:add";
    public const string UpdateLesson = "lesson:update";
    public const string ToggleLesson = "lesson:toggle";

    public const string enroll = "enrollment:add";

    public const string AddRole = "role:add";
    public const string UpdateRole = "role:update";
    public const string ToggleRole = "role:toggle";

    public static IList<string> GetPermissions =>
        typeof(Permissions).GetFields().Select(e => e.GetValue(e) as string).ToList()!;
}
