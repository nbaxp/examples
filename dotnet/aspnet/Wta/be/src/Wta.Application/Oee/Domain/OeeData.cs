using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE数据", Order = 40)]
public class OeeData : BaseEntity, IEntityTypeConfiguration<OeeData>
{
    [DataType(DataType.Date)]
    [Display(Name = "日期")]
    public DateOnly Date { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-shift/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "班次")]
    public Guid ShfitId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-asset/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "资产")]
    public string AssetNumber { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "oee-part/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [Display(Name = "零件")]
    public string PartNumber { get; set; } = default!;

    [Display(Name = "开始")]
    public DateTime Start { get; set; }

    [Display(Name = "结束")]
    public DateTime? End { get; set; }

    [Display(Name = "时长")]
    public int Duration { get; set; }

    [Display(Name = "标准速率")]
    public float StandardUpm { get; set; } = 0f;

    [Display(Name = "实际速率")]
    public float SpeedUpm { get; set; } = 0f;

    [Display(Name = "总产出")]
    public int TotalItems { get; set; }

    [Display(Name = "不良品")]
    public int ScrapItems { get; set; }

    [Display(Name = "OP CODE")]
    public string? OpCode { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-status/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "状态")]
    public Guid? StatusId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "userName")]
    [KeyValue("label", "name")]
    [Display(Name = "操作员")]
    public string Operator { get; set; } = default!;

    public OeeShift? Shift { get; set; }
    public OeeStatus? Status { get; set; }

    public void Configure(EntityTypeBuilder<OeeData> builder)
    {
        builder.HasOne(o => o.Shift).WithMany(o => o.Datas).HasForeignKey(o => o.ShfitId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Status).WithMany(o => o.Datas).HasForeignKey(o => o.StatusId).OnDelete(DeleteBehavior.Cascade);
    }
}
