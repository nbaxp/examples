using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE状态", Order = 30)]
public class OeeStatus : BaseNameNumberEntity, IEntityTypeConfiguration<OeeStatus>
{
    [UIHint("color")]
    [Display(Name = "颜色")]
    public string Color { get; set; } = "#ffffff";

    [UIHint("select")]
    [KeyValue("url", "oee-status-type/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "类型")]
    public Guid TypeId { get; set; } = default!;

    [Hidden]
    public OeeStatusType Type { get; set; } = default!;

    public void Configure(EntityTypeBuilder<OeeStatus> builder)
    {
        builder.HasOne(d => d.Type).WithMany().HasForeignKey(o => o.TypeId).OnDelete(DeleteBehavior.Cascade);
    }
}

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE状态", Order = 30)]
public class OeeStatusType : BaseNameNumberEntity
{
}
