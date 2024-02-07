using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Models;

namespace Wta.Application.Identity.Models;

public class TenantModel : IBaseModel<Tenant>
{
    public Guid? Id { get; set; }

    [Required]
    public string? Name { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    public string? Number { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    public string UserName { get; set; } = default!;

    public bool Disabled { get; set; }

    public List<Guid> Permissions { get; set; } = new List<Guid>();
}
