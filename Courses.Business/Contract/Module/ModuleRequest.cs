
namespace Courses.Business.Contract.Module;

public sealed record ModuleRequest (
   string Title,
   string Description,
   int Order
);