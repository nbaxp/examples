using Wta.Application.Default.Domain;
using Wta.Infrastructure.Application.Models;

namespace Wta.Application.Default.Models;

public class TenantModel : IBaseModel<Tenant>
{
    public Guid? Id { get; set; }

    [Required]
    public string? Name { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    public string? Number { get; set; } = default!;

    public bool Disabled { get; set; }

    public List<string> Permissions { get; set; } = new List<string>();
}
