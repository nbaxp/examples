using System.ComponentModel.DataAnnotations.Schema;

namespace Wta.Application.Platform.Domain;

[SystemSettings, Display(Name = "租户", Order = 7)]
[DependsOn<PlatformDbContext>]
public class Tenant : Entity, IEntityTypeConfiguration<Tenant>
{
    [Display(Name = "名称")]
    public string Name { get; set; } = default!;

    [Required]
    [ReadOnly(true)]
    [Display(Name = "编码")]
    public string Number { get; set; } = default!;

    [Display(Name = "图标")]
    public string? Logo { get; set; }

    [Display(Name = "版权")]
    public string? Copyright { get; set; }

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

    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.Property(o => o.Name).IsRequired();
        builder.Property(o => o.Number).IsRequired();
        builder.HasIndex(o => o.Number).IsUnique();
    }
}
