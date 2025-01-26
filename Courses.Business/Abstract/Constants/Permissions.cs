
namespace Courses.Business.Abstract.Constants;
public static class Permissions
{
    public static string Type { get; } = "permissions";

    public const string AddCategory = "category:add";
    public const string GetCategory = "category:read";
    public const string UpdateCategory = "category:update";
    public const string ToggleCategory = "category:delete";

    public const string AddCourse = "course:add";
    public const string GetCourse = "course:read";
    public const string UpdateCourse = "course:update";
    public const string ToggleCourse = "course:toggle";
    public const string BlockUserCourse = "course:blockuser";

    public const string AddModule = "module:add";
    public const string GetModule = "module:read";
    public const string UpdateModule = "module:update";
    public const string ToggleModule = "module:toggle";

    public const string AddLesson = "lesson:add";
    public const string GetLesson = "lesson:read";
    public const string UpdateLesson = "lesson:update";
    public const string ToggleLesson = "lesson:toggle";


    public const string AddRole = "role:add";
    public const string GetRole = "role:read";
    public const string UpdateRole = "role:update";
    public const string ToggleRole = "role:toggle";

    public const string AddExam = "exam:add";
    public const string GetExam = "exam:read";
    public const string UpdateExam = "exam:update";
    public const string ToggleExam = "exam:toggle";

    public const string AddQuestion = "question:add";
    public const string GetQuestion = "question:read";
    public const string UpdateQuestion = "question:update";
    public const string ToggleQuestion = "question:toggle";


    public const string UpdateAccount = "account:update";
    public const string ChangePassword = "account:changePassword";
    public const string GetAccount = "account:read";

    public const string AddAnswer = "answer:add";
    public const string GetAnswer = "answer:read";


    public const string AddEnrolment = "enrolment:add";
    public const string GetEnrolment = "enrolment:read";
    public const string UpdateEnrolment = "enrolment:update";

    public static IList<string> GetAllPermissions =>
        typeof(Permissions).GetFields().Select(e => e.GetValue(e) as string).ToList()!;

    
}
