namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Name = "租户", Order = 7)]
public class Tenant : Entity
{
    public string Name { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    public string Number { get; set; } = default!;

    public bool Disabled { get; set; }
}
