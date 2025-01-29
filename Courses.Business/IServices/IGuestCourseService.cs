using Courses.Business.Contract.Course;

namespace Courses.Business.IServices;
public interface IGuestCourseService
{
    Task<Result<GuestUserCourseResponse>> GetCourseAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GuestUserCourseResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}
