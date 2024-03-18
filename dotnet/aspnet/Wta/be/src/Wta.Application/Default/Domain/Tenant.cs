using Wta.Infrastructure.Application.Domain;

namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Order = 6)]
public class Tenant : Entity
{
    public string Name { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    public string Number { get; set; } = default!;

    public bool Disabled { get; set; }
}
