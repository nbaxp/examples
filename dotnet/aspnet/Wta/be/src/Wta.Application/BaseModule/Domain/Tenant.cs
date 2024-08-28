using System.ComponentModel.DataAnnotations.Schema;

namespace Wta.Application.BaseModule.Domain;

[SystemSettings, Display(Name = "租户", Order = 7)]
public class Tenant : Entity
{
    public string Name { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    public string Number { get; set; } = default!;

    public bool Disabled { get; set; }

    [KeyValue("hideForList", true)]
    [UIHint("select")]
    [KeyValue("url", "permission/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [NotMapped]
    public List<string> Permissions { get; set; } = [];
}
