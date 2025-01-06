using Courses.Business.Contract.Course;

namespace Courses.Business.IServices;
public interface IEnrollmentService 
{
    Task<Result> EnrollToCourseAsync(Guid courseId, string userId, CancellationToken cancellationToken = default);
    Task<Result<CourseDetailedResponse>> GetAsync(Guid courseId, string userId, CancellationToken token = default);
    // get user courses 
    // get user report of any course with courseid
}