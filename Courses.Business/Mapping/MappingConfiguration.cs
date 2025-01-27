using Courses.Business.Contract.Category;
using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;
using Courses.Business.Contract.User;


namespace Courses.Business.Mapping;
public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<AddCourseRequest, Course>()
            .Map(
                dest => dest.CourseCategories,
                src => src.CategoryIds.Select(e => new CourseCategories { CategoryId = e })
            );

        config.NewConfig<Lesson, LessonResponse>()
            .Map(dest => dest.Resources, src => src.Resources.Adapt<List<RecourseResponse>>());

        config.NewConfig<(Lesson lesson, UserLesson userLesson), UserLessonResponse>()
            .Map(dest => dest, src => src.lesson)
            .Map(dest => dest, src => src.userLesson);

        config.NewConfig<(ApplicationUser user, IList<string> userRoles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.userRoles);

        
        config.NewConfig<(Course course, List<ModuleResponse> modules, int NoCompleted, int NoEnrollment), CourseResponse>()
            .Map(dest => dest.Modules, src => src.modules)
            .Map(dest => dest, src => src.course)
            .Map(dest => dest.NoCompleted, src => src.NoCompleted)
            .Map(dest => dest.NoStudent, src => src.NoEnrollment);

        config.NewConfig<(Course course, List<CategoryResponse> categories), RegularUserCourseResponse>()
            .Map(dest => dest.Categories, src => src.categories)
            .Map(dest => dest, src => src.course);
    }
}
