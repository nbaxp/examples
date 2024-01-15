using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Models;

namespace Wta.Application.Identity.Models;

public class DepartmentModel : IBaseModel<Department>
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public Guid? ParentId { get; set; }
}
