using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE状态", Order = 20)]
public class OeeStatus : BaseNameNumberEntity, IEntityTypeConfiguration<OeeStatus>
{
    [UIHint("select")]
    [KeyValue("url", "oee-status-type/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "类型")]
    public Guid TypeId { get; set; } = default!;

    [UIHint("color")]
    [Display(Name = "颜色")]
    public string Color { get; set; } = "#CC9900";

    [UIHint("color")]
    [Display(Name = "标准时间")]
    public int BaseTime { get; set; }

    [Hidden]
    public OeeStatusType? Type { get; set; }

    public void Configure(EntityTypeBuilder<OeeStatus> builder)
    {
        builder.HasOne(d => d.Type).WithMany().HasForeignKey(o => o.TypeId).OnDelete(DeleteBehavior.Cascade);
    }
}

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE状态类型", Order = 10)]
public class OeeStatusType : BaseNameNumberEntity
{
}
