using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE数据", Order = 50)]
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
    public Guid? ShiftId { get; set; } = default!;

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
    public float StandardUpm { get; set; }

    [Display(Name = "实际速率")]
    public float SpeedUpm { get; set; }

    [Display(Name = "总产出")]
    public int Total { get; set; }

    [Display(Name = "设备因素废品")]
    public int EequipmentScrap { get; set; }

    [Display(Name = "非设备因素废品")]
    public int NonEequipmentScrap { get; set; }

    //[Display(Name = "OP CODE")]
    //public string? OpCode { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-status/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "状态")]
    public Guid StatusId { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "oee-status/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "原因")]
    public Guid? ReasonId { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "userName")]
    [KeyValue("label", "name")]
    [Display(Name = "操作员")]
    public string Operator { get; set; } = default!;

    [Hidden]
    public OeeShift? Shift { get; set; }

    [Hidden]
    public OeeStatus? Status { get; set; }

    [Hidden]
    public OeeReason? Reason { get; set; } = default!;

    public void Configure(EntityTypeBuilder<OeeData> builder)
    {
        builder.HasOne(o => o.Shift).WithMany().HasForeignKey(o => o.ShiftId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.Status).WithMany().HasForeignKey(o => o.StatusId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Reason).WithMany().HasForeignKey(o => o.ReasonId).OnDelete(DeleteBehavior.SetNull);
    }
}
