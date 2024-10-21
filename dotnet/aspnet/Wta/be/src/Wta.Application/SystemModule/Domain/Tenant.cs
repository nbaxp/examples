using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[SystemSettings, Display(Name = "租户", Order = 7)]
[DependsOn<SystemDbContext>]
public class Tenant : Entity
{
    public string Name { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    public string Number { get; set; } = default!;

    public bool Disabled { get; set; }

    [KeyValue("hideForList", true)]
    [KeyValue("hideForQuery", true)]
    [UIHint("select")]
    [KeyValue("url", "permission/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [NotMapped]
    public List<string> Permissions { get; set; } = [];
}
