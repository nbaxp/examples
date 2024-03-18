using Wta.Application.Default.Domain;
using Wta.Infrastructure.Application.Models;

namespace Wta.Application.Default.Models;

public class DictModel : IBaseModel<Dict>
{
    public Guid? Id { get; set; }
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
}
