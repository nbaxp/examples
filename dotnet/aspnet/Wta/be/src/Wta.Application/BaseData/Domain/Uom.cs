using Wta.Application.Platform;

namespace Wta.Application.BaseData.Domain;

[DependsOn<PlatformDbContext>, UomGroup, Display(Name = "计量单位", Order = 20)]
public class Uom : BaseNameNumberEntity, IEntityTypeConfiguration<Uom>
{
    [UIHint("select")]
    [KeyValue("url", "unit-category/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "类型")]
    public Guid CategoryId { get; set; }

    public UnitCategory? Category { get; set; }

    [Display(Name = "比例")]
    public float Ratio { get; set; } = 1f;

    public void Configure(EntityTypeBuilder<Uom> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Uoms).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.Cascade);
    }
}

[DependsOn<PlatformDbContext>, UomGroup, Display(Name = "计量单位分类", Order = 10)]
public class UnitCategory : BaseNameNumberEntity
{
    public List<Uom> Uoms { get; set; } = [];
}
