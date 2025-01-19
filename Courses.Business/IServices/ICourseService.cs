using Courses.Business.Contract.Course;
using Courses.Business.Contract.Tag;
using Courses.Business.Contract.UploadFile;
using Courses.Business.Contract.User;

namespace Courses.Business.IServices;
public interface ICourseService
{
    Task<Result<Guid>> AddAsync(AddCourseRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Guid id, string userid, UpdateCourseRequest request, CancellationToken cancelToken = default);
    Task<Result> UpdateThumbnailAsync(Guid id, string userId, UploadImageRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleIsPublishAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<Result> AssignCourseToCategoryAsync(Guid id, string userId, Guid categories, CancellationToken cancellationToken = default);
    Task<Result> UnAssignCourseToCategoriesAsync(Guid id, string userId, Guid categoryId, CancellationToken cancellationToken = default);
    Task<Result> AssignCourseToTagsAsync(Guid id, string userId, TagsRequest tags, CancellationToken cancellationToken = default);
    Task<Result> UnAssignCourseToTagsAsync(Guid id, string userId, TagsRequest tags, CancellationToken cancellationToken = default);
    Task<Result> BlockedUserAsync(Guid id, string userId, UserIdentifierRequest request, CancellationToken cancellationToken = default);
    Task<Result> UnBlockedUserAsync(Guid id, string userId, UserIdentifierRequest request, CancellationToken cancellationToken = default);
    Task<Result<CourseResponse>> GetAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseResponse>> GetAllAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserResponse>> GetUsersInCourseAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    
    //Task<Result<CourseDetailedResponse>> GetDetailedAsync( Guid id,string? userId, CancellationToken cancellationToken = default);
}
