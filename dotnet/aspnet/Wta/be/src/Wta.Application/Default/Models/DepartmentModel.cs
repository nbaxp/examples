using Wta.Application.Default.Domain;
using Wta.Infrastructure.Models;

namespace Wta.Application.Default.Models;

public class DepartmentModel : IBaseModel<Department>
{
    public Guid? Id { get; set; }
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
}
