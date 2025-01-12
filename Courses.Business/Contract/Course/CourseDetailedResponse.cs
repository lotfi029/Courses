﻿using Courses.Business.Contract.Category;
using Courses.Business.Contract.Module;

namespace Courses.Business.Contract.Course;
public record CourseDetailedResponse (
    Guid Id, 
    string Title,
    string Description,
    string Level,
    Guid ThumbnailId,
    TimeSpan? Duration,
    float Rating,
    Guid UserCourseId,
    string CompleteStatus,
    DateTime LastInteractDate,
    DateTime? FinshedDate,
    IList<UserModuleResponse>? Modules,
    IList<string> Tags,
    IList<CategoryResponse> Categories
);
