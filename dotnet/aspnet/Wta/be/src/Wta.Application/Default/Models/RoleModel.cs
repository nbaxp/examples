using Wta.Application.Default.Domain;
using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Attributes;

namespace Wta.Application.Default.Models;

public class RoleModel : IBaseModel<Role>
{
    [NotDefault]
    public Guid? Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Number { get; set; }

    public List<Guid> Permissions { get; set; } = new List<Guid>();
}
