﻿using Courses.Business.Contract.Course;
using Courses.Business.Contract.Lesson;
using Courses.Business.Contract.Module;
using Courses.Business.Contract.User;
using Mapster;
using Serilog.Configuration;

namespace Courses.Business.Mapping;
public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<AddCourseRequest, Course>()
            .Ignore(e => e.Tags)
            .Map(
                dest => dest.CourseCategories,
                src => src.CategoryIds.Select(e => new CourseCategories { CategoryId = e })
            );

        config.NewConfig<Lesson, LessonResponse>()
            .Map(dest => dest.Resources, src => src.Resources.Adapt<List<RecourseResponse>>());

        config.NewConfig<(ApplicationUser user, IList<string> userRoles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.userRoles);

        
        config.NewConfig<(Course course, List<ModuleResponse> modules), CourseResponse>()
            .Map(dest => dest.Modules, src => src.modules)
            .Map(dest => dest, src => src.course);



        config.NewConfig<IEnumerable<string>, Tag>()
            .Map(dest => dest.Title, src => src);
    }
}
