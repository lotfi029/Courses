using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Courses.Business.Abstract;

public record Error (string Code, string Description, int? StatusCode)
{
    public static readonly Error Non = new(string.Empty, string.Empty, null);
    public static implicit operator Result(Error error) => new(false, error);
}