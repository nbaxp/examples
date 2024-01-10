using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Models;

namespace Wta.Application.Identity.Models;

public class RoleModel : IBaseModel<Role>
{
    [NotDefault]
    public Guid? Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Number { get; set; }
}
