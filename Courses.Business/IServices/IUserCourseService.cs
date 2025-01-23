using Courses.Business.Contract.Course;

namespace Courses.Business.IServices;
public interface IUserCourseService
{
    Task<Result<RegularUserCourseResponse>> GetCourseAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RegularUserCourseResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}
