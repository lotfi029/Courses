using Courses.Business.Contract.Course;
using Courses.Business.Contract.Tag;

namespace Courses.Business.IServices;
public interface ICourseService
{
    Task<Result<Guid>> AddAsync(AddCourseRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string userid, Guid id, UpdateCourseRequest request, CancellationToken cancelToken = default);
    Task<Result> ToggleIsPublishAsync(string userid, Guid id, CancellationToken cancellationToken = default);
    Task<Result> AssignCourseToCategoryAsync(string userid, Guid id, Guid categories, CancellationToken cancellationToken = default);
    Task<Result> UnAssignCourseToCategoriesAsync(string userid, Guid id, Guid categoryId, CancellationToken cancellationToken = default);
    Task<Result> AssignCourseToTagsAsync(string userid, Guid id, TagsRequest tags, CancellationToken cancellationToken = default);
    Task<Result> UnAssignCourseToTagsAsync(string userid, Guid id, TagsRequest tags, CancellationToken cancellationToken = default);
    Task<Result<CourseResponse>> GetAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseResponse>> GetAllAsync(string userId, CancellationToken cancellationToken = default);
    //Task<Result<CourseDetailedResponse>> GetDetailedAsync( Guid id,string? userId, CancellationToken cancellationToken = default);
}
