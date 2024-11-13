using Wta.Application.BaseData.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE班次", Order = 20)]
public class OeeShift : BaseNameNumberEntity
{
    [UIHint("select")]
    [KeyValue("url", "oee-asset/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "资产")]
    public Guid AssetId { get; set; }

    [Hidden]
    public OeeAsset? Asset { get; set; }

    [Display(Name = "开始")]
    [UIHint("time")]
    public TimeOnly Start { get; set; }

    [Display(Name = "结束")]
    [UIHint("time")]
    public TimeOnly End { get; set; }

    [Hidden]
    public List<OeeData> Datas { get; set; } = [];

    public void Configure(EntityTypeBuilder<OeeShift> builder)
    {
        builder.HasOne(o => o.Asset).WithMany(o => o.Shifts).HasForeignKey(o => o.AssetId).OnDelete(DeleteBehavior.Cascade);
    }
}
