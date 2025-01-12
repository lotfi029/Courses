﻿using Courses.Business.Contract.Category;
using Courses.Business.Contract.Module;

namespace Courses.Business.Contract.Course;

public record CourseResponse (
    Guid Id, 
    string Title,
    string Description,
    string Level,
    Guid ThumbnailId,
    TimeSpan? Duration,
    float Rating,
    bool IsPublished,
    double Price,
    IEnumerable<string> Tags,
    IEnumerable<CategoryResponse> Categories,
    IEnumerable<ModuleResponse> Modules
    //UserCourseResponse? UserCourse
);
public record UserCourseResponse(
    Guid Id,
    string CompleteStatus,
    DateTime LastInteractDate,
    DateTime? FinshedDate
    );

