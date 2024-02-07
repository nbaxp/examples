using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Models;

namespace Wta.Application.Identity.Models;

public class DictModel : IBaseModel<Dict>
{
    public Guid? Id { get; set; }
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
}
