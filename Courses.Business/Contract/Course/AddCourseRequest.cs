﻿namespace Courses.Business.Contract.Course;

public record AddCourseRequest (
    string Title,
    string Description,
    string Level,
    double Price,
    IFormFile Thumbnail,
    IList<Guid> CategoryIds
);
