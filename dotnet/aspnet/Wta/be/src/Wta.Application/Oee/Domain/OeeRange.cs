using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "Oee时间段", Order = 50)]
public class OeeRange : BaseEntity, IEntityTypeConfiguration<OeeRange>
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
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "资产")]
    public Guid AssetId { get; set; }

    [Display(Name = "开始")]
    public DateTime Start { get; set; }

    [Display(Name = "结束")]
    public DateTime End { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-reason/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "原因")]
    public Guid? ReasonId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "员工")]
    public Guid? UserId { get; set; }

    public OeeShift? Shift { get; set; }
    public OeeAsset? Asset { get; set; }
    public OeeReason? Reason { get; set; }
    public User? User { get; set; }

    public void Configure(EntityTypeBuilder<OeeRange> builder)
    {
        builder.HasOne(o => o.Shift).WithMany(o => o.Ranges).HasForeignKey(o => o.ShfitId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Asset).WithMany(o => o.Ranges).HasForeignKey(o => o.AssetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Reason).WithMany(o => o.Ranges).HasForeignKey(o => o.ReasonId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Reason).WithMany().HasForeignKey(o => o.ReasonId).OnDelete(DeleteBehavior.Cascade);
    }
}
